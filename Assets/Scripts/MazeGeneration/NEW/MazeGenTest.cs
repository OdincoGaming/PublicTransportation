using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;
using System.Linq;
using System.Collections;

public class MazeGenTest : MonoBehaviour
{
    [SerializeField, Min(1)] private int width;
    [SerializeField, Min(1)] private int height;
    private Cell[,] cells = new Cell[0, 0];

    [SerializeField] private List<SpawnPointBehaviour> possibleExitSpawns = new();
    private SpawnPointBehaviour exit;
    [SerializeField] private List<SpawnPointBehaviour> possiblePlayerSpawns = new();
    private SpawnPointBehaviour playerStart;
    private CellSPBehaviourPair playerStartCSPB;

    [SerializeField] private List<Cell> cellsRemaining = new();
    [SerializeField] private List<Cell> cellsCarved = new();
    [SerializeField] private List<CellSPBehaviourPair> CSPBs = new();

    private void Start()
    {
        DrawGrid();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (cells[i, j] != null)
                    Destroy(cells[i, j]);
    }

    private void DrawGrid() // generate SOs for cells
    {
        cells = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                Cell cell = ScriptableObject.CreateInstance("Cell") as Cell;
                cell._location = new Vector3(i + 0.5f, 0, j + 0.5f); // used for drawing gizmos for debugging in scene
                cell.xPos = i;
                cell.zPos = j;
                cell.gridPointVector = new Vector2(i, j); // the actual location of the cell on the grid
                cells[i, j] = cell;

            }
        }

        PopulateCellList();
    }

    private void PopulateCellList() // populate neighbors for each cell
    {
        foreach(Cell c in cells)
        {
            c.PopulateNeighbors(cells, true);
        }

        InitializeSpawnPoints();
    }

    private void InitializeSpawnPoints()
    {
        List<SpawnPointBehaviour> allSpawnPoints = new List<SpawnPointBehaviour>
            (GameObject.FindObjectsByType<SpawnPointBehaviour>(FindObjectsSortMode.None));

        exit = PickSPBAtRandom(possibleExitSpawns);
        if (possiblePlayerSpawns.Contains(exit))
        {
            possiblePlayerSpawns.Remove(exit);
        }
        playerStart = PickSPBAtRandom(possiblePlayerSpawns);

        foreach (SpawnPointBehaviour spb in allSpawnPoints)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (spb.designationVector == new Vector2(i, j))
                    {
                        if (spb == playerStart)
                        { 
                            cells[i, j].cellState = CellState.Start;
                            CellSPBehaviourPair start = new(cells[i, j], spb);
                            playerStartCSPB = start;
                            CSPBs.Add(start);
                            cells[i, j].cellState = CellState.Start;
                            cellsCarved.Add(cells[i, j]); 
                        }
                        else if (spb == exit)
                        { cells[i, j].cellState = CellState.Exit; 
                            CSPBs.Add(new(cells[i, j], spb));
                            cells[i, j].cellState |= CellState.Exit;
                            cellsCarved.Add(cells[i, j]);
                        }
                        else 
                        { 
                            CSPBs.Add(new(cells[i, j], spb));
                            cellsRemaining.Add(cells[i, j]);
                        }
                        i = width; j = height;
                    }
                }
            }
        }

        //VisitCell(cells[0, 0]);
        //VisitCell(cellsRemaining[0]);
        (int x, int y)? result = FindCoordinates(cells, playerStartCSPB);
        CarvePath(result.Value.x, result.Value.y);
        StartCoroutine(WaitForCellsToCarveAlt());
    }

    private SpawnPointBehaviour PickSPBAtRandom(List<SpawnPointBehaviour> behaviours)
    {
        return behaviours.Count > 0 ? behaviours[Random.Range(0, behaviours.Count)] : null;
    }

    IEnumerator WaitForCellsToCarve()
    {
        yield return new WaitForEndOfFrame();
        if(cellsRemaining.Count > 0)
        {
            StartCoroutine(WaitForCellsToCarve());
        }else{
            cellsCarved = cellsCarved.Distinct().ToList();
            HandleSpawns();
        }
    }

    IEnumerator WaitForCellsToCarveAlt()
    {
        yield return new WaitForEndOfFrame();

        if (timesCarved == 0)
        {
            HandleSpawns();
        }
        else
        {
            StartCoroutine(WaitForCellsToCarveAlt());
        }
    }

    private void VisitCell(Cell cell)
    {
        cell.visited = true;
        cell.cellState = CellState.Walkway;
        cellsCarved.Add(cell);
        cellsRemaining.Remove(cell);
        
        // Create a working copy of neighbors and shuffle them
        List<Cell> shuffledNeighbors = new List<Cell>(cell.neighbors);
        ShuffleList(shuffledNeighbors);
        
        
        foreach (Cell neighbor in shuffledNeighbors)
        {
            if (!neighbor.visited)
            {
                if (Random.Range(0, 3) > 1)
                {
                    neighbor.cellState = CellState.Walkway;
                    VisitCell(neighbor);
                }
                else
                {
                    neighbor.cellState = CellState.Wall;
                    neighbor.visited = true;
                    cellsRemaining.Remove(neighbor);
                    cellsCarved.Add(neighbor);
                }
            }
        }

        if(cellsRemaining.Count > 0)
        {
            VisitCell(cellsRemaining[0]);
        }
    }

    public void ShuffleList(List<Cell> list)
    {
        System.Random rng = new();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Cell value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    static (int, int)? FindCoordinates(Cell[,] array, CellSPBehaviourPair target)
    {
        Cell targ = target.cell;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j] == targ)
                {
                    return (i, j);
                }
            }
        }
        return null;
    }
    static (int, int)? FindCoordinates(Cell[,] array, Cell targ)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j] == targ)
                {
                    return (i, j);
                }
            }
        }
        return null;
    }

    private void HandleSpawns()
    {
        Debug.Log("here");
        foreach (CellSPBehaviourPair cspb in CSPBs)
        {
            cspb.SpawnInCell();
        }
    }

    private int timesCarved = 0;
    private void CarvePath(int x, int y)
    {
        Cell cell = cells[x, y];
        if (cell.isAssigned == true)
            return;
        timesCarved++;
        cell.isAssigned = true;
        if (cellsRemaining.Contains(cell)){
            cellsRemaining.Remove(cell);
            if(!cellsCarved.Contains(cell))
                cellsCarved.Add(cell);
        }
        // Get random directions
        List<(int dx, int dy)> directions = new List<(int dx, int dy)>
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };
        directions.Shuffle();

        foreach (var (dx, dy) in directions)
        {
            int nx = x + dx * 2;
            int ny = y + dy * 2;

            if (IsInsideGrid(nx, ny) && !cells[nx, ny].isAssigned)
            {
                if (cells[x + dx, y + dy].cellState != CellState.Start && cells[x + dx, y + dy].cellState != CellState.Exit)
                {
                    cells[x + dx, y + dy].cellState = CellState.Wall;
                }
                if (cells[nx, ny].cellState != CellState.Start && cells[nx, ny].cellState != CellState.Exit)
                {
                    cells[nx, ny].cellState = CellState.Walkway;
                }
                CarvePath(nx, ny);
            }
        }
        if(cellsRemaining.Count > 0)
        {
            (int x, int y)? result = FindCoordinates(cells, cellsRemaining[0]);
            CarvePath(result.Value.x, result.Value.y);
        }
        timesCarved--;
    }

    private bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
}

public static class Extensions
{
    private static System.Random rand = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}