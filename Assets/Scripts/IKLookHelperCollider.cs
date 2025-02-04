using UnityEngine;

public class IKLookHelperCollider : MonoBehaviour
{
    [SerializeField] private IKLookHelperBehaviour m_LookHelper;
    [SerializeField] private LayerMask layer;

    private void OnTriggerEnter(Collider other)
    {
        if ((layer.value & (1 << other.gameObject.layer)) != 0)
        {
            m_LookHelper.Respond(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layer.value & (1 << other.gameObject.layer)) != 0)
        {
            m_LookHelper.Respond(false);
        }
    }
}
