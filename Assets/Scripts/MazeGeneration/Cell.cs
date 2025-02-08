using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    // josh stuff, not used in latest maze gen
    public int xPos = -1;
    public int zPos = -1;
    public int[] northNeighbor = { -1, -1 };
    public int[] southNeighbor = { -1, -1 };
    public int[] westNeighbor = { -1, -1 };
    public int[] eastNeighbor = { -1, -1 };
    public bool isAssigned = false;
    public int step = 0;
    public char originDir = ' ';
    public Vector3 _location;
    // end josh stuff

    public Vector2 gridPointVector =Vector2.zero;
    public List<Cell> neighbors = new List<Cell>();
    public CellState cellState = CellState.Wall;
    public bool visited = false;


    public void PopulateNeighbors(Cell[,] cells)
    {
        if (xPos + 1 < cells.GetLength(0))
        {
            northNeighbor = new int[] { xPos + 1, zPos };
            neighbors.Add(cells[northNeighbor[0], northNeighbor[1]]);
        }
        if (xPos - 1 >= 0)
        {
            southNeighbor = new int[] { xPos - 1, zPos };
            neighbors.Add(cells[southNeighbor[0], southNeighbor[1]]);
        }
        if (zPos + 1 < cells.GetLength(1))
        {
            eastNeighbor = new int[] { xPos, zPos + 1 };
            neighbors.Add(cells[eastNeighbor[0], eastNeighbor[1]]);
        }
        if (zPos - 1 >= 0)
        {
            westNeighbor = new int[] { xPos, zPos - 1 };
            neighbors.Add(cells[westNeighbor[0], westNeighbor[1]]);
        }
    }

    public void PopulateNeighbors(Cell[,] cells, bool dumbAlt)
    {
        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        if(xPos + 1 < width)
        {
            neighbors.Add(cells[xPos + 1, zPos]);
        }

        if(xPos -1 >= 0)
        {
            neighbors.Add(cells[xPos - 1, zPos]);
        }

        if(zPos + 1 < height)
        {
            neighbors.Add(cells[xPos, zPos + 1]);
        }

        if(zPos - 1 >= 0)
        {
            neighbors.Add(cells[xPos, zPos - 1]);
        }

        ShuffleList(neighbors);
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
}



public enum CellState
{
    Wall,
    Walkway,
    Start,
    Exit
}