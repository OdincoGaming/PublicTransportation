using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A flexible handler for RaycastHit events in the form of a MonoBehaviour. Responses can be connected directly from the Unity Inspector.
/// </summary>
public class RaycastHitEventListener : MonoBehaviour
{
	[SerializeField] private RaycastHitEventChannelSO _channel = default;

	private void OnEnable()
	{
		if (_channel != null)
			_channel.OnEventRaised += Respond;
	}

	private void OnDisable()
	{
		if (_channel != null)
			_channel.OnEventRaised -= Respond;
	}

	private void Respond(RaycastHit hit)
	{
		Debug.Log(hit.collider.gameObject.name);
	}
}