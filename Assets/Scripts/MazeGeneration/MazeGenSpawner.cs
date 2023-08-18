using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenSpawner : MonoBehaviour
{
    public int width;
    public int height;
    public float xDensity = 0.0f;
    public float zDensity = 0.0f;
    public List<string> possibleExits;
    public Cell[,] cells = new Cell[0, 0];
    public List<Cell> cellsRemaining;
    public List<Cell> validPositions;

    [SerializeField] private SpawnPointEventChannelSO channel;

    private void Awake()
    {
        DrawGrid();
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
                validPositions.Add(cells[i, j]);
            }
        }
        CarveExit();
    }

    private void CarveExit()
    {
        List<Cell> exits = new List<Cell>();
        if (possibleExits.Count > 0)
        {
            foreach (string s in possibleExits)
            {
                string[] cor = s.Split('-');
                int x = int.Parse(cor[0]);
                int z = int.Parse(cor[1]);
                exits.Add(cells[x, z]);
            }
            int randomInt = Random.Range(0, exits.Count - 1);

            exits[randomInt].cellState = CellState.Exit;
            Carve(exits[randomInt]);
        }
        else
        {
            Carve(cells[0, 0]);
        }
    }

    private void Carve(Cell c)
    {
        c.isAssigned = true;
        List<char> dirList = new List<char> { 'n', 's', 'w', 'e' };
        int ri = Random.Range(0, dirList.Count);
        ri = Random.Range(0, dirList.Count);
        while (dirList.Count > 0)
        {
            if (dirList.Count > 0)
            {
                if (dirList[ri] == 'n')
                {
                    if (c.northNeighbor[0] > -1)
                    {
                        if (cells[c.northNeighbor[0], c.northNeighbor[1]].isAssigned)
                        {
                            if (cells[c.northNeighbor[0], c.northNeighbor[1]].cellState == CellState.Wall)
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    if (CheckForValidPaths(cells[c.northNeighbor[0], c.northNeighbor[1]]))
                                    {
                                        cells[c.northNeighbor[0], c.northNeighbor[1]].step += (c.step + 1);
                                        cells[c.northNeighbor[0], c.northNeighbor[1]].cellState = CellState.Walkway;
                                        Carve(cells[c.northNeighbor[0], c.northNeighbor[1]]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (CheckForValidPaths(cells[c.northNeighbor[0], c.northNeighbor[1]]))
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    cells[c.northNeighbor[0], c.northNeighbor[1]].step += (c.step + 1);
                                }
                                cells[c.northNeighbor[0], c.northNeighbor[1]].cellState = CellState.Walkway;
                            }
                            Carve(cells[c.northNeighbor[0], c.northNeighbor[1]]);
                        }
                    }
                }
                else if (dirList[ri] == 's')
                {
                    if (c.southNeighbor[0] > -1)
                    {
                        if (cells[c.southNeighbor[0], c.southNeighbor[1]].isAssigned)
                        {
                            if (cells[c.southNeighbor[0], c.southNeighbor[1]].cellState == CellState.Wall)
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    if (CheckForValidPaths(cells[c.southNeighbor[0], c.southNeighbor[1]]))
                                    {
                                        cells[c.southNeighbor[0], c.southNeighbor[1]].step += (c.step + 1);
                                        cells[c.southNeighbor[0], c.southNeighbor[1]].cellState = CellState.Walkway;
                                    }
                                    Carve(cells[c.southNeighbor[0], c.southNeighbor[1]]);
                                }
                            }
                        }
                        else
                        {
                            if (CheckForValidPaths(cells[c.southNeighbor[0], c.southNeighbor[1]]))
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    cells[c.southNeighbor[0], c.southNeighbor[1]].step += (c.step + 1);
                                }
                                cells[c.southNeighbor[0], c.southNeighbor[1]].cellState = CellState.Walkway;
                            }
                            Carve(cells[c.southNeighbor[0], c.southNeighbor[1]]);
                        }
                    }
                }
                else if (dirList[ri] == 'e')
                {
                    if (c.eastNeighbor[0] > -1)
                    {
                        if (cells[c.eastNeighbor[0], c.eastNeighbor[1]].isAssigned)
                        {
                            if (cells[c.eastNeighbor[0], c.eastNeighbor[1]].cellState == CellState.Wall)
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    if (CheckForValidPaths(cells[c.eastNeighbor[0], c.eastNeighbor[1]]))
                                    {
                                        cells[c.eastNeighbor[0], c.eastNeighbor[1]].step += (c.step + 1);
                                        cells[c.eastNeighbor[0], c.eastNeighbor[1]].cellState = CellState.Walkway;
                                        Carve(cells[c.eastNeighbor[0], c.eastNeighbor[1]]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (CheckForValidPaths(cells[c.eastNeighbor[0], c.eastNeighbor[1]]))
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    cells[c.eastNeighbor[0], c.eastNeighbor[1]].step += (c.step + 1);
                                }
                                cells[c.eastNeighbor[0], c.eastNeighbor[1]].cellState = CellState.Walkway;
                            }
                            Carve(cells[c.eastNeighbor[0], c.eastNeighbor[1]]);
                        }
                    }
                }
                else if (dirList[ri] == 'w')
                {
                    if (c.westNeighbor[0] > -1)
                    {
                        if (cells[c.westNeighbor[0], c.westNeighbor[1]].isAssigned)
                        {
                            if (cells[c.westNeighbor[0], c.westNeighbor[1]].cellState == CellState.Wall)
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    if (CheckForValidPaths(cells[c.westNeighbor[0], c.westNeighbor[1]]))
                                    {
                                        cells[c.westNeighbor[0], c.westNeighbor[1]].step += (c.step + 1);
                                        cells[c.westNeighbor[0], c.westNeighbor[1]].cellState = CellState.Walkway;
                                        Carve(cells[c.westNeighbor[0], c.westNeighbor[1]]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (CheckForValidPaths(cells[c.westNeighbor[0], c.westNeighbor[1]]))
                            {
                                if (c.cellState == CellState.Walkway)
                                {
                                    cells[c.westNeighbor[0], c.westNeighbor[1]].step += (c.step + 1);
                                }
                                cells[c.westNeighbor[0], c.westNeighbor[1]].cellState = CellState.Walkway;
                            }
                            Carve(cells[c.westNeighbor[0], c.westNeighbor[1]]);
                        }
                    }
                }

                dirList.RemoveAt(ri);
                ri = Random.Range(0, dirList.Count);
                ri = Random.Range(0, dirList.Count);
            }
        }
        //channel.RaiseEvent(c);
    }

    private bool CheckForValidPaths(Cell c)
    {


        bool isPath = true;
        Cell nn = null;
        Cell sn = null;
        Cell en = null; ;
        Cell wn = null;

        if (c.northNeighbor[0] >= 0)
        {
            nn = cells[c.northNeighbor[0], c.northNeighbor[1]];
        }
        if (c.southNeighbor[0] >= 0)
        {
            sn = cells[c.southNeighbor[0], c.southNeighbor[1]];
        }
        if (c.eastNeighbor[0] >= 0)
        {
            en = cells[c.eastNeighbor[0], c.eastNeighbor[1]];
        }
        if (c.westNeighbor[0] >= 0)
        {
            wn = cells[c.westNeighbor[0], c.westNeighbor[1]];
        }

        if (nn != null && sn != null)
        {
            if (nn.cellState == CellState.Walkway && sn.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(wn, 'n') || HasWalkwayInDir(en, 'n') || HasWalkwayInDir(wn, 's') || HasWalkwayInDir(en, 's'))
                {
                    isPath = false;
                }
            }
            else if (nn.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(wn, 'n') || HasWalkwayInDir(en, 'n'))
                {
                    isPath = false;
                }
            }
            else if (sn.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(wn, 's') || HasWalkwayInDir(en, 's'))
                {
                    isPath = false;
                }
            }
        }
        else if (nn != null)
        {
            if (nn.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(wn, 'n') || HasWalkwayInDir(en, 'n'))
                {
                    isPath = false;
                }
            }
        }
        else if (sn != null)
        {

            if (sn.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(wn, 's') || HasWalkwayInDir(en, 's'))
                {
                    isPath = false;
                }
            }
        }

        if (en != null && wn != null)
        {
            if (en.cellState == CellState.Walkway && wn.cellState == CellState.Walkway)
            {

                if (HasWalkwayInDir(nn, 'e') || HasWalkwayInDir(sn, 'e') || HasWalkwayInDir(sn, 'w') || HasWalkwayInDir(nn, 'w'))
                {
                    isPath = false;
                }
            }
            else if (en.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(nn, 'e') || HasWalkwayInDir(sn, 'e'))
                {
                    isPath = false;
                }
            }
            else if (wn.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(nn, 'w') || HasWalkwayInDir(sn, 'w'))
                {
                    isPath = false;
                }
            }
        }
        else if (en != null)
        {
            if (en.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(nn, 'e') || HasWalkwayInDir(sn, 'e'))
                {
                    isPath = false;
                }
            }

        }
        else if (wn != null)
        {
            if (wn.cellState == CellState.Walkway)
            {
                if (HasWalkwayInDir(nn, 'w') || HasWalkwayInDir(sn, 'w'))
                {
                    isPath = false;
                }
            }
        }

        int randomInt = Random.Range(0, 100);
        randomInt = Random.Range(0, 100);
        /*if (c.isAssigned) 
        {
            if (randomInt > 85)
                isPath = false;
        }*/
        return isPath;
    }

    public bool HasWalkwayInDir(Cell c, char dir)
    {
        if (c == null)
        {
            return false;
        }
        if (c.cellState == CellState.Wall)
        {
            return false;
        }
        if (dir == 'n')
        {
            if (c.northNeighbor[0] > -1)
            {
                if (cells[c.northNeighbor[0], c.northNeighbor[1]].cellState == CellState.Walkway)
                {
                    return true;
                }
            }
        }
        else if (dir == 's')
        {
            if (c.southNeighbor[0] > -1)
            {
                if (cells[c.southNeighbor[0], c.southNeighbor[1]].cellState == CellState.Walkway)
                {
                    return true;
                }
            }
        }
        else if (dir == 'e')
        {
            if (c.eastNeighbor[0] > -1)
            {
                if (cells[c.eastNeighbor[0], c.eastNeighbor[1]].cellState == CellState.Walkway)
                {
                    return true;
                }
            }
        }
        else if (dir == 'w')
        {
            if (c.westNeighbor[0] > -1)
            {
                if (cells[c.westNeighbor[0], c.westNeighbor[1]].cellState == CellState.Walkway)
                {
                    return true;
                }
            }
        }
        return false;
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

    private void Shuffle(List<Cell> alpha)
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            Cell temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }
}
