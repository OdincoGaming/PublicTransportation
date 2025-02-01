using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointBehaviour : MonoBehaviour
{
    public string designation;
    public Vector2 designationVector;

    [SerializeField] private SpawnablesSO spawnables;
    private void Awake()
    {
        designation = transform.name + "-" + transform.parent.name;
        designationVector = new(int.Parse(transform.name), int.Parse(transform.parent.name));
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }

    public void Respond(Cell cell)
    {
        if (cell.cellState == CellState.Wall)
        {
            int randomInt = Random.Range(0, spawnables.possibleSpawns.Count);
            GameObject go = Instantiate(spawnables.possibleSpawns[randomInt], transform.position, transform.rotation);
            go.transform.SetParent(this.transform);
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<SphereCollider>().enabled = true;
        }
    }
}
