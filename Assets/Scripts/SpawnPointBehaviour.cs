using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointBehaviour : MonoBehaviour
{
    [SerializeField] private string designation;
    [SerializeField] private SpawnPointEventChannelSO channel;
    [SerializeField] private SpawnablesSO spawnables;
    private void Awake()
    {
        designation = transform.parent.name + "-" + transform.name;
    }

    private void OnEnable()
    {
        channel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        channel.OnEventRaised -= Respond;
    }

    private void Respond(Cell cell)
    {
        if (cell.cellState == CellState.Wall)
        {
            string des = cell.xPos + "-" + cell.zPos;
            if (des == designation)
            {
                int randomInt = Random.Range(0, spawnables.possibleSpawns.Count - 1);
                Instantiate(spawnables.possibleSpawns[randomInt], transform.position, transform.rotation);
            }
        }
    }
}
