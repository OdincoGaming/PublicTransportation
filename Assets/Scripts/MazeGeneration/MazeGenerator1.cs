using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator1 : MonoBehaviour
{
    public int width;
    public int height;
    public List<Cell> cells;
    public List<Cell> validPositions;
    public List<Vector3> cellPositions;
    private void DrawGridDebug()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {

#if UNITY_EDITOR
                Vector3 lineStart = new Vector3(i, 0, j);
                Vector3 lineForwardEnd = new Vector3(i, 0, j + 1);
                Vector3 lineSidewaysEnd = new Vector3(i + 1, 0, j);

                // z lines
                if(i == 0 || i == width - 1)
                {
                    if (j != height - 1)
                        Debug.DrawLine(lineStart, lineForwardEnd, Color.green);
                }
                else
                {
                    if (j != height - 1)
                        Debug.DrawLine(lineStart, lineForwardEnd, Color.blue);
                }

                // x lines
                if(j == 0 || j == height - 1)
                {
                    if (i != width - 1)
                        Debug.DrawLine(lineStart, lineSidewaysEnd, Color.green);
                }
                else
                {
                    if (i != width - 1)
                        Debug.DrawLine(lineStart, lineSidewaysEnd, Color.blue);
                }
#endif

            }
        }
    }

    private void DrawGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i != width - 1 && j != height - 1)
                {
                    Vector3 cellPos = new Vector3(i + 0.5f, 0, j + 0.5f);
                    cellPositions.Add(cellPos);
                }
            }
        }

        PopulateCellList();
    }

    private void PopulateCellList()
    {
        foreach(Vector3 v in cellPositions)
        {
            Cell c = ScriptableObject.CreateInstance("Cell") as Cell;
            c._location = v;
            cells.Add(c);
        }

        foreach(Cell cell in cells)
        {
            cell.PopulateNeighbors(cells);
        }
    }

    private void Update()
    {
        DrawGridDebug();
    }
    private void Awake()
    {
        DrawGrid();
    }

    private void OnDrawGizmosSelected()
    { 
        if(cellPositions.Count > 0)
        {
            foreach (Cell c in cells)
            {
                if (c._isEndPoint)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(c._location, 0.25f);
                }
                else if (c._isWalkway)
                {
                    Gizmos.color = Color.black;
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

    private void CheckForValidPositions(Cell c)
    {
        validPositions.Clear();
        bool isValid = false;
        foreach (Cell n in c._neighbors)
        {
            foreach (Cell subN in n._neighbors)
            {
                isValid = true;
                if (subN != c && subN != n)
                {
                    if (subN._isWalkway)
                        isValid = false;
                }
            }
            if (isValid)
                validPositions.Add(n);
        }
    }

    private Cell PickPosition(List<Cell> cells)
    {
        Shuffle(cells);
        int randomInt = Random.Range(0, cells.Count - 1);
        return cells[randomInt];
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

    private void HandleCell(Cell c)
    {
        c._isWalkway = true;

    }
}
