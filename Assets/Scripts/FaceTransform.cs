using UnityEngine;

public class FaceTransform : MonoBehaviour
{
    [SerializeField] private Transform target;
    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(target);
    }
}
