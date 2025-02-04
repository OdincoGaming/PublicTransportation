using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RecursiveBackTrackingMazeGen : MonoBehaviour
{
    // the size of the maze
    [SerializeField, Min(1)] private int width;
    [SerializeField, Min(1)] private int height;

    // manually filled out to control where the possible exits are (somehwere on the outside edges of the maze)
    [SerializeField] private List<SpawnPointBehaviour> possibleExitSpawns;
    // manually filled ou tto control where the possible player start positions (somewhere in the middle of the maze)
    [SerializeField] private List<SpawnPointBehaviour> possiblePlayerSpawns;
    // filled out automatically at runtime, spawnpointbehaviours are manually placed in the scene to fit environment geometry
    [SerializeField] private List<SpawnPointBehaviour> allSpawnPoints;
    // automatically filled out when scriptableobjects are created, each cell pairs to a spawnpointbehaviour
    [SerializeField] private Cell[,] cells = new Cell[0, 0];
    // cells left to carve
    [SerializeField] private List<CellSPBehaviourPair> cellsRemaining;
    // cells that have been carved
    [SerializeField] private List<CellSPBehaviourPair> cellsCarved;

    private void Awake()
    {
        if (possibleExitSpawns == null || possiblePlayerSpawns == null || possibleExitSpawns.Count == 0 || possiblePlayerSpawns.Count == 0)
            Debug.LogError("Spawn points not properly configured");

        cellsRemaining = new List<CellSPBehaviourPair>();
        cellsCarved = new List<CellSPBehaviourPair>();
        DrawGrid();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (cells[i, j] != null)
                    Destroy(cells[i, j]);
    }

    private void DrawGrid()
    {
        cells = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                Cell cell = ScriptableObject.CreateInstance("Cell") as Cell;
                cell._location = new Vector3(i + 0.5f, 0, j + 0.5f);
                cell.xPos = i;
                cell.zPos = j;
                cells[i, j] = cell;

            }
        }

        PopulateCellList();
    }

    private void PopulateCellList()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                cells[i, j].PopulateNeighbors(cells);
            }
        }
        InitializeSpawnPoints();
    }

    private void InitializeSpawnPoints()
    {
        allSpawnPoints = new List<SpawnPointBehaviour>(GameObject.FindObjectsByType<SpawnPointBehaviour>(FindObjectsSortMode.None));

        SpawnPointBehaviour exit = PickSPBAtRandom(possibleExitSpawns);
        if (possiblePlayerSpawns.Contains(exit))
        {
            possiblePlayerSpawns.Remove(exit);
        }

        SpawnPointBehaviour playerStart = PickSPBAtRandom(possiblePlayerSpawns);

        foreach (SpawnPointBehaviour spb in allSpawnPoints)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (spb.designationVector == new Vector2(i, j))
                    {
                        if (spb == playerStart)
                        { cells[i, j].cellState = CellState.Start; cellsCarved.Add(new(cells[i, j], spb)); i = width; j = height; } // setting start counts as cell being carved
                        else if (spb == exit)
                        { cells[i, j].cellState = CellState.Exit; cellsCarved.Add(new(cells[i, j], spb)); i = width; j = height; } // setting exit counts as cell being carved
                        else { cellsRemaining.Add(new(cells[i, j], spb)); i = width; j = height; }

                    }
                }
            }
        }
        Carve();
    }

    private SpawnPointBehaviour PickSPBAtRandom(List<SpawnPointBehaviour> behaviours)
    {
        return behaviours.Count > 0 ? behaviours[Random.Range(0, behaviours.Count)] : null;
    }

    private void HandleSpawnPoints()
    {
        List<CellSPBehaviourPair> uniqueCells = cellsCarved.Distinct().ToList();
        foreach (CellSPBehaviourPair csPair in uniqueCells)
        {
            csPair.SpawnInCell();
        }
    }
    private void Carve()
    {
        Stack<CellSPBehaviourPair> stack = new Stack<CellSPBehaviourPair>();
        CellSPBehaviourPair current = cellsCarved[0];

        while (cellsRemaining.Count > 0)
        {
            List<CellSPBehaviourPair> unvisitedNeighbors = new List<CellSPBehaviourPair>();

            foreach (Cell neighbor in current.cell.neighbors)
            {
                CellSPBehaviourPair neighborPair = cellsRemaining.Find(x => x.cell == neighbor);
                if (neighborPair != null)
                {
                    unvisitedNeighbors.Add(neighborPair);
                }
            }

            if (unvisitedNeighbors.Count > 0)
            {
                CellSPBehaviourPair next = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                stack.Push(current);

                DetermineCellState(next.cell);
                cellsRemaining.Remove(next);
                cellsCarved.Add(next);
                current = next;
            }
            else if (stack.Count > 0)
            {
                current = stack.Pop();
            }
            else if (cellsRemaining.Count > 0)
            {
                current = cellsRemaining[0];
                //DetermineCellState(current.cell);
                //cellsRemaining.RemoveAt(0);
                //cellsCarved.Add(current);
            }
        }

        HandleSpawnPoints();
    }

    private void DetermineCellState(Cell cell)
    {
        if (cell.cellState != CellState.Exit && cell.cellState != CellState.Start)
        {
            int carvedNeighbors = 0;
            foreach (Cell neighbor in cell.neighbors)
            {
                if (neighbor.cellState == CellState.Walkway ||
                    neighbor.cellState == CellState.Start ||
                    neighbor.cellState == CellState.Exit)
                {
                    carvedNeighbors++;
                }
            }

            if (carvedNeighbors <= 1)
            {
                cell.cellState = CellState.Walkway;
            }
            else
            {
                cell.cellState = CellState.Wall;
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        if (cells.GetLength(0) > 0)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cell c = cells[i, j];
                    if (c.cellState == CellState.Start)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(c._location, 0.25f);
                    }
                    else if (c.cellState == CellState.Walkway)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawSphere(c._location, 0.25f);
                    }
                    else if (c.cellState == CellState.Exit)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(c._location, 0.25f);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(c._location, 0.25f);
                    }
                }
            }
        }
    }
}
