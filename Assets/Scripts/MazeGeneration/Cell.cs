using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : ScriptableObject
{
    public int xPos = -1;
    public int zPos = -1;
    public int[] northNeighbor = { -1, -1 };
    public int[] southNeighbor = { -1, -1 };
    public int[] westNeighbor = { -1, -1 };
    public int[] eastNeighbor = { -1, -1 };
    public Vector3 _location;
    public CellState cellState = CellState.Wall;
    public bool isAssigned = false;
    public int step = 0;
    public char originDir = ' ';

    public void CreatTile(char od)
    {
        originDir = od;
    }

    public void PopulateNeighbors(Cell[,] cells)
    {
        if (xPos + 1 < cells.GetLength(0))
        {
            northNeighbor = new int[] { xPos + 1, zPos };
        }
        if (xPos - 1 >= 0)
        {
            southNeighbor = new int[] { xPos - 1, zPos };
        }
        if (zPos + 1 < cells.GetLength(1))
        {
            eastNeighbor = new int[] { xPos, zPos + 1 };
        }
        if (zPos - 1 >= 0)
        {
            westNeighbor = new int[] { xPos, zPos - 1 };
        }
    }
}

public enum CellState
{
    Wall,
    Walkway,
    Start,
    Exit
}