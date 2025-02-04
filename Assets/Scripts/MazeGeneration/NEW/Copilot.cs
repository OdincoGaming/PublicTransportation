using UnityEngine;

public class Copilot : MonoBehaviour
{
    /*
     using System;
using System.Collections.Generic;

public enum CellState { Start, Exit, Wall, Walkway }

public class Cell
{
    public CellState State { get; set; }
    public bool Visited { get; set; }

    public Cell()
    {
        State = CellState.Wall;
        Visited = false;
    }
}

public class Maze
{
    private int width, height;
    private Cell[,] grid;
    private Random rand = new Random();

    public Maze(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell();
            }
        }
    }

    public void GenerateMaze()
    {
        // Initialize start and exit points
        grid[0, 0].State = CellState.Start;
        grid[width - 1, height - 1].State = CellState.Exit;

        // Carve the maze starting from (0, 0)
        CarvePath(0, 0);
    }

    private void CarvePath(int x, int y)
    {
        grid[x, y].Visited = true;
        grid[x, y].State = CellState.Walkway;

        List<(int dx, int dy)> directions = new List<(int dx, int dy)>
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        // Shuffle the directions to create a random path
        directions.Shuffle();

        foreach (var (dx, dy) in directions)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (IsInsideGrid(nx, ny) && !grid[nx, ny].Visited)
            {
                CarvePath(nx, ny);
            }
        }
    }

    private bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
}

public static class Extensions
{
    private static Random rand = new Random();

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

class Program
{
        int width = 10;
        int height = 10;
        Maze maze = new Maze(width, height);
        maze.GenerateMaze();
}

     * */
}
