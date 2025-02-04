using UnityEngine;

public class FaceRandomDirection : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    private void Awake()
    {
        int randomRotation = Random.Range(-179, 179);
        _transform.Rotate(new(0, randomRotation, 0));
    }
}
