using UnityEngine;
using MText;
using System.Collections.Generic;
public class TextPhysicsEnabler : MonoBehaviour // reuse but not rename, now reenables bouncers
{
    [SerializeField] private List<GameObject> gos = new();

    public void EnableBouncers()
    {
        foreach (GameObject go in gos) 
        {
            go.SetActive(true);    
        }
    }
}
