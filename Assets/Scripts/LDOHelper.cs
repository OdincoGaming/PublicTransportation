using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.Dynamics;

public class LDOHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> gos;

    public void ToggleGameObjects(bool state)
    {
        foreach(GameObject go in gos)
        {
            go.SetActive(state);
        }
    }

}
