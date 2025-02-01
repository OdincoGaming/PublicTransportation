using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMazeGenSpawner : MonoBehaviour
{
    public int width;
    public int height;
    public float xDensity = 0.0f;
    public float zDensity = 0.0f;
    public List<string> possibleExits; // this is outdated, now using SpawnPointBehaviour to mark significant points
    public List<SpawnPointBehaviour> possibleExitsUseThisOne;
    public List<SpawnPointBehaviour> possiblePlayerSpawns;
    public Cell[,] cells = new Cell[0, 0];
    public List<Cell> cellsRemaining;
    public List<Cell> validPositions;

    private void Awake()
    {
        DrawGrid();
    }

    private IEnumerator CarveExitEnum()
    {
        yield return new WaitForSeconds(2.0f);
        CarveExit();

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
        StartCoroutine(CarveExitEnum());
    }

    /*private void CarveExit()
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
        HandleSpawnPoints();
    }*/

    private void HandleSpawnPoints()
    {
        List<SpawnPointBehaviour> li = new List<SpawnPointBehaviour>(GameObject.FindObjectsOfType<SpawnPointBehaviour>());

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                string des = i + "-" + j;
                foreach (SpawnPointBehaviour spb in li)
                {
                    if (des == spb.designation)
                    {
                        spb.Respond(cells[i, j]);
                        li.Remove(spb);
                        break;
                    }
                }
            }
        }

    }

    private void CarveExit()
    {
        if (possibleExitsUseThisOne.Count > 0 && possiblePlayerSpawns.Count > 0)
        {
            // Select random exit and player spawn points
            int exitIndex = Random.Range(0, possibleExitsUseThisOne.Count);
            int playerIndex = Random.Range(0, possiblePlayerSpawns.Count);
            
            SpawnPointBehaviour exit = possibleExitsUseThisOne[exitIndex];
            SpawnPointBehaviour playerSpawn = possiblePlayerSpawns[playerIndex];
            
            // Get corresponding cells using designationVector
            Cell exitCell = cells[(int)exit.designationVector.x, (int)exit.designationVector.y];
            Cell playerCell = cells[(int)playerSpawn.designationVector.x, (int)playerSpawn.designationVector.y];
            
            // Mark exit and start points
            exitCell.cellState = CellState.Exit;
            playerCell.cellState = CellState.Start;
            
            // Start recursive backtracking from exit to player spawn
            RecursiveBacktrack(exitCell, playerCell, new List<Cell>());
        }
        HandleSpawnPoints();
    }
      private bool RecursiveBacktrack(Cell current, Cell target, List<Cell> visited)
      {
          current.cellState = CellState.Walkway; // Mark cell immediately
          current.isAssigned = true; // Mark as assigned
          visited.Add(current);

          if (current == target)
              return true;
        
          // Get all neighbors
          List<Cell> neighbors = new List<Cell>();
          if (current.northNeighbor[0] > -1) neighbors.Add(cells[current.northNeighbor[0], current.northNeighbor[1]]);
          if (current.southNeighbor[0] > -1) neighbors.Add(cells[current.southNeighbor[0], current.southNeighbor[1]]);
          if (current.eastNeighbor[0] > -1) neighbors.Add(cells[current.eastNeighbor[0], current.eastNeighbor[1]]);
          if (current.westNeighbor[0] > -1) neighbors.Add(cells[current.westNeighbor[0], current.westNeighbor[1]]);
        
          // Randomize neighbor order
          for (int i = neighbors.Count - 1; i > 0; i--)
          {
              int j = Random.Range(0, i + 1);
              Cell temp = neighbors[i];
              neighbors[i] = neighbors[j];
              neighbors[j] = temp;
          }
        
          foreach (Cell neighbor in neighbors)
          {
              if (!visited.Contains(neighbor) && CheckForValidPaths(neighbor))
              {
                  if (RecursiveBacktrack(neighbor, target, visited))
                  {
                      neighbor.cellState = CellState.Walkway;
                      return true;
                  }
              }
          }
        
          return false;
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

        /*int randomInt = Random.Range(0, 100);
        randomInt = Random.Range(0, 100);
        if (randomInt > 25)
            isPath = false;*/
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
