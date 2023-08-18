using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.Dynamics;

public class PuppetOptimization : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PuppetMaster pm = other.GetComponentInChildren<PuppetMaster>();
        if(pm != null)
        {
            pm.mode = PuppetMaster.Mode.Active;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PuppetMaster pm = other.GetComponentInChildren<PuppetMaster>();
        if (pm != null)
        {
            pm.mode = PuppetMaster.Mode.Disabled;
        }
    }
}
