using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class IKLookHelperBehaviour : MonoBehaviour
{
    [SerializeField] private IKLookBehaviour lookBehaviour;
    private GameObject lookTarget;
    private int collidersTouching = 0;
    private void OnEnable()
    {
        lookTarget = GameObject.FindGameObjectWithTag("LookTarget");
        lookBehaviour.target = lookTarget.transform;
    }

    public void Respond(bool isTouching)
    {
        if (isTouching)
        {
            collidersTouching++;
        }
        else
        {
            collidersTouching--;
        }

        if (collidersTouching == 0)
        {
            lookBehaviour.isLooking = false;
        }
        else
        {
            lookBehaviour.isLooking = true;
        }
    }
}
