using UnityEngine;

public class IKLookBehaviour : MonoBehaviour
{

    public Transform target;
    public bool isLooking = false;

    [SerializeField] private Animator animator;
    [SerializeField] private Vector2 lookAtWeightMinMax = new(0.5f,1);
    [SerializeField] private float lookAtSmoothTime = 0.5f;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private Transform defaultTarget;

    private float lookAtWeight = 1.0f;
    private float lookAtVelocity;
    private Vector3 velocity = Vector3.zero;
    private Vector3 currentLookAtPosition;

    private void OnAnimatorIK()
    {
        if(target != null) 
        {
            if (isLooking)
            {
                // Smoothly interpolate the look-at position
                currentLookAtPosition = Vector3.SmoothDamp(currentLookAtPosition, target.position, ref velocity, smoothTime);
                lookAtWeight = Mathf.SmoothDamp(lookAtWeight, lookAtWeightMinMax.y, ref lookAtVelocity, lookAtSmoothTime);

                // Set the look-at position using IK
                animator.SetLookAtWeight(lookAtWeight);
                animator.SetLookAtPosition(currentLookAtPosition);
            }
            else
            {
                // Smoothly interpolate the look-at position
                currentLookAtPosition = Vector3.SmoothDamp(currentLookAtPosition, defaultTarget.position, ref velocity, smoothTime);
                lookAtWeight = Mathf.SmoothDamp(lookAtWeight, lookAtWeightMinMax.x, ref lookAtVelocity, lookAtSmoothTime);

                // Set the look-at position using IK
                animator.SetLookAtWeight(lookAtWeight);
                animator.SetLookAtPosition(currentLookAtPosition);
            }
        }
    }
}
