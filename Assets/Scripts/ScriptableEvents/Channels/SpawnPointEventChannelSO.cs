using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that send a RaycastHit
/// Example: An event when clicking on something.
/// </summary>

[CreateAssetMenu(menuName = "Events/SpawnPoint Event Channel")]
public class SpawnPointEventChannelSO : DescriptionBaseSO
{
	public event UnityAction<Cell> OnEventRaised;

	public void RaiseEvent(Cell cell)
	{
		if (OnEventRaised != null)
        {
			OnEventRaised.Invoke(cell);
		}
	}
}