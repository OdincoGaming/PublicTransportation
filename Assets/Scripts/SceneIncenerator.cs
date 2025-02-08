using System.Collections.Generic;
using UnityEngine;

public class SceneIncenerator : MonoBehaviour
{
    public List<GameObject> gos;

    private void OnDestroy()
    {
        foreach (GameObject go in gos)
            Destroy(go);

        Destroy(gameObject);
    }
}
