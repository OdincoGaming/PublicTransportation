using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public List<int> possibleExits;
    public List<Cell> cells; 
    public List<Cell> cellsRemaining;
    public List<Cell> cellsTouched;
    public List<Vector3> cellPositions;
    public List<Cell> validPositions;
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
                // add cell positions to list
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

        CarveExit();
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

    private void CarveExit()
    {
        cellsRemaining = new List<Cell>(cells);

        List<Cell> exits = new List<Cell>();
        foreach (int i in possibleExits)
            exits.Add(cells[i]);

        int randomInt = Random.Range(0, exits.Count - 1);

        exits[randomInt]._isExit = true;
        exits[randomInt]._isWalkway = true;
        cellsRemaining.Remove(exits[randomInt]);
        cellsTouched.Add(exits[randomInt]);
        Carve(exits[randomInt]);
    }

    private void Carve(Cell c)
    {
        if(cellsRemaining.Contains(c))
            cellsRemaining.Remove(c);

        if (!cellsTouched.Contains(c))
            cellsTouched.Add(c);

        CheckForValidPositions(c);
        if(validPositions.Count > 0)
        {
            Carve(PickPosition(validPositions));
        }
        else
        {
            c._isEndPoint = true;
            if(validPositions.Count > 0)
            {
                Cell temp = validPositions[0];
                validPositions.RemoveAt(0);
                Carve(temp);
            }
        }
    }


    private void CheckForValidPositions(Cell c)
    {
        validPositions.Clear();
        bool isValid = false;
        foreach (Cell n in c._neighbors)
        {
            foreach(Cell subN in n._neighbors)
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
        cells[randomInt]._isWalkway = true;
        return cells[randomInt];
    }

    private void Shuffle(List<Cell> alpha)
    {
        for(int i = 0; i < alpha.Count; i++)
        {
            Cell temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }
}
