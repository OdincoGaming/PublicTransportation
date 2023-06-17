using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that send a RaycastHit
/// Example: An event when clicking on something.
/// </summary>

[CreateAssetMenu(menuName = "Events/RaycastHit Event Channel")]
public class RaycastHitEventChannelSO : DescriptionBaseSO
{
	public event UnityAction<RaycastHit> OnEventRaised;

	public void RaiseEvent(RaycastHit hit)
	{
		if (OnEventRaised != null)
        {
			OnEventRaised.Invoke(hit);
		}
	}
}