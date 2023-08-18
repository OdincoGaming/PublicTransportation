using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is used for Events that have a bool argument.
/// Example: An event to toggle a UI interface
/// </summary>

[CreateAssetMenu(menuName = "Spawnables/NPC Prefab SO")]
public class SpawnablesSO : DescriptionBaseSO
{
    public List<GameObject> possibleSpawns;
}