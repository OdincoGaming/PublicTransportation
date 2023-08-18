using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.Dynamics;

public class LongDistanceOptimization : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DoThething(true, other);
    }
    private void OnTriggerExit(Collider other)
    {
        DoThething(false, other);
    }
    
    private void DoThething(bool state, Collider other)
    {
        LDOHelper helper = other.transform.GetComponent<LDOHelper>();
        if (helper != null)
            helper.ToggleGameObjects(state);
    }
}
