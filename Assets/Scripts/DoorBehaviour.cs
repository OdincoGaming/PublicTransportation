using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<SpawnPointBehaviour> SPBs;

    public void SetAsExit()
    {
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        animator.SetTrigger("Close");
    }

    public bool doesSPBsContain(SpawnPointBehaviour spb)
    {
        bool result = false;
        if (SPBs.Contains(spb))
            result = true;

        return result;
    }
}
