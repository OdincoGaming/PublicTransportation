using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellSPBehaviourPair
{
    public Cell cell;
    public SpawnPointBehaviour spawnPointBehaviour;

    // the following 3 vars are only used for debugging in the inspector
    [SerializeField] private Vector2 gridPointVector = Vector2.zero;
    [SerializeField] private List<Vector2> neighborGridPoints = new List<Vector2>();
    [SerializeField] private CellState cellState = CellState.Wall;

    public void SpawnInCell()
    {
        spawnPointBehaviour.Respond(cell);
    }

    public CellSPBehaviourPair(Cell cell, SpawnPointBehaviour spawnPointBehaviour)
    {
        this.cell = cell;
        this.spawnPointBehaviour = spawnPointBehaviour;

        // fill out debug info
        if(cell.gridPointVector == spawnPointBehaviour.designationVector)
        {
            this.gridPointVector = cell.gridPointVector;
        }

        foreach(Cell c in cell.neighbors)
        {
            neighborGridPoints.Add(c.gridPointVector);
        }

        cellState = cell.cellState;
        spawnPointBehaviour.StartCoroutine(DelayedSetCellState());
        // end fill out debug info
    }

    IEnumerator DelayedSetCellState()
    {
        yield return new WaitForSeconds(3f);
        cellState = cell.cellState;
    }
}
