using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.Dynamics;

public class LDOHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> gos;
    private bool isTouchingAtStart = false;
    public void ToggleGameObjects(bool state)
    {
        foreach(GameObject go in gos)
        {
            go.SetActive(state);
        }
    }
    private void Start()
    {
        StartCoroutine(DelayedDisableOnStart());
    }
    private void OnTriggerEnter(Collider other)
    {
        isTouchingAtStart = true;
    }
    IEnumerator DelayedDisableOnStart()
    {
        yield return new WaitForEndOfFrame();
        if (!isTouchingAtStart)
            ToggleGameObjects(false);
    }
}
