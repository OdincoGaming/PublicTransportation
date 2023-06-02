using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : ScriptableObject
{
    public List<Cell> _neighbors = new List<Cell>();
    public Vector3 _location;
    public bool _isWalkway;
    public bool _isEndPoint;
    public bool _isExit;
    public bool _isSpawn;

    public void PopulateNeighbors(List<Cell> cells)
    {
        //Vector3 nextColumnNextRow = new Vector3(_location.x + 1, 0, _location.z + 1);
        //Vector3 nextColumnPreviousRow = new Vector3(_location.x + 1, 0, _location.z - 1);
        Vector3 nextColumnSameRow = new Vector3(_location.x + 1, 0, _location.z);

        //Vector3 previousColumnNextRow = new Vector3(_location.x - 1, 0, _location.z + 1);
       //Vector3 previousColumnPreviousRow = new Vector3(_location.x - 1, 0, _location.z - 1);
        Vector3 previousColumnSameRow = new Vector3(_location.x - 1, 0, _location.z);

        Vector3 sameColumnNextRow = new Vector3(_location.x, 0, _location.z + 1);
        Vector3 sameColumnPreviousRow = new Vector3(_location.x, 0, _location.z - 1);

        List<Vector3> posibleNeighbors = new List<Vector3>
        {
            //nextColumnNextRow,
            //nextColumnPreviousRow,
            nextColumnSameRow,
            //previousColumnNextRow,
            //previousColumnPreviousRow,
            previousColumnSameRow,
            sameColumnNextRow,
            sameColumnPreviousRow
        };

        foreach(Cell c in cells)
        {
            if (posibleNeighbors.Contains(c._location))
                _neighbors.Add(c);
        }
    }
}
