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

    /*[SerializeField] private List<Modular3DText> mText = new();
    [SerializeField] private List<Modular3DText> physicsText = new();

    public void EnableModules()
    {
        foreach(Modular3DText text in mText)
        {
            text.gameObject.SetActive(false);
        }

        foreach (Modular3DText text in physicsText)
        {
            text.gameObject.SetActive(true);
        }
    }*/
}
