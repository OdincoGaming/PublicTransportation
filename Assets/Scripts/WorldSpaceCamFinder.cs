using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WorldSpaceCamfinder : MonoBehaviour
{
    private Canvas c;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        c = GetComponent<Canvas>();
        c.worldCamera = Camera.main;
    }
}
