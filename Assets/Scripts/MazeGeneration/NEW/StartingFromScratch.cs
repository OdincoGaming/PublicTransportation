using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;
using KinematicCharacterController.Examples;

public class StartingFromScratch : MonoBehaviour
{
    [SerializeField] private List<GameObject> player;
    [SerializeField] private TimerCanvasBehaviour timerCanvasBehaviour;
    [SerializeField, Min(1)] private float difficulty = 2f;

    [SerializeField, Min(1)] private int width;
    [SerializeField, Min(1)] private int height;
    private Cell[,] cells = new Cell[0, 0];

    [SerializeField] private List<SpawnPointBehaviour> possibleExitSpawns = new();
    [SerializeField] private List<DoorBehaviour> doors = new();
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

    private void DrawGrid() // 1
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

    private void PopulateCellList() // 2
    {
        foreach(Cell c in cells)
        {
            c.PopulateNeighbors(cells, true);
        }

        InitializeSpawnPoints();
    }

    private void InitializeSpawnPoints() // 3
    {
        List<SpawnPointBehaviour> allSpawnPoints = new List<SpawnPointBehaviour>
            (GameObject.FindObjectsByType<SpawnPointBehaviour>(FindObjectsSortMode.None));

        exit = PickSPBAtRandom(possibleExitSpawns);
        if (possiblePlayerSpawns.Contains(exit))
        {
            possiblePlayerSpawns.Remove(exit);
        }
        playerStart = PickSPBAtRandom(possiblePlayerSpawns);

        CreateCSPBs(allSpawnPoints);
    }

    private void CreateCSPBs(List<SpawnPointBehaviour> allSpawnPoints) // 4
    {
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

                            foreach (GameObject go in player)
                            {
                                go.transform.SetLocalPositionAndRotation(spb.transform.position, Quaternion.identity);
                                go.SetActive(true);
                            }
                        }
                        else if (spb == exit)
                        {
                            cells[i, j].cellState = CellState.Exit;
                            CSPBs.Add(new(cells[i, j], spb));
                            cells[i, j].cellState |= CellState.Exit;
                            foreach (GameObject go in player)
                            {
                                go.transform.LookAt(spb.transform.position);
                            }
                            foreach(DoorBehaviour door in doors)
                            {
                                if (door.doesSPBsContain(spb))
                                    door.SetAsExit();
                            }
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
        CarveCells();
    }

    private SpawnPointBehaviour PickSPBAtRandom(List<SpawnPointBehaviour> behaviours)
    {
        return behaviours.Count > 0 ? behaviours[Random.Range(0, behaviours.Count)] : null;
    }

    private void CarveCells()
    {
        Cell c = cellsRemaining[0];

        int neighborWalkwayCount = CountNeighborWalkways(c);

        if (neighborWalkwayCount < 2) // if less than or equal to 2 neighbors are walkways then this cell is a walkway
        {
            c.cellState = CellState.Walkway;
        }
        else // if more than 2 neighbors are walkways then this is a wall
        {
            c.cellState = CellState.Wall;
        }

        CleanupAfterCarving(c);
    }

    private int CountNeighborWalkways(Cell c)
    {
        int neighborWalkwayCount = 0;
        foreach (Cell cn in c.neighbors) // check cells neighbors 
        {
            if (cn.cellState == CellState.Walkway)
                neighborWalkwayCount++;
        }
        return neighborWalkwayCount;
    }

    private void CleanupAfterCarving(Cell c)
    {
        if (cellsRemaining.Contains(c))
            cellsRemaining.Remove(c);
        if (!cellsCarved.Contains(c))
            cellsCarved.Add(c);

        if (cellsRemaining.Count > 0)
        {
            CarveCells();
        }
        else
        {
            HandleSpawns();
        }
    }

    private void HandleSpawns()
    {
        timerCanvasBehaviour.SetTimetoComplete(CalculateTimeToComplete());
        //timerCanvasBehaviour.SetPing(exit.transform.position);
        foreach (CellSPBehaviourPair cspb in CSPBs)
        {
            cspb.SpawnInCell();
        }
    }

    private float CalculateTimeToComplete()
    {
        float result = 0;
        float distance = Vector3.Distance(playerStart.transform.position, exit.transform.position);
        result = distance * difficulty;
        return result;
    }
}