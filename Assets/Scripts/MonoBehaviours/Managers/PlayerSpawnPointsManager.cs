using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// Gather spawn points (Transforms) and teleports the player to it.
/// After teleporting, Makes every spawn point mesh disappear.
/// </summary>
public class PlayerSpawnPointsManager : MonoBehaviour
{

    private const string G1 = "PLAYER REFERENCE";
    [HorizontalLine]
    [Required, BoxGroup(G1), SerializeField] private GameObject _player;

    private const string G2 = "SPAWN POINTS SETTINGS";
    private const string DpName = nameof(_spawnPoints);
    [HorizontalLine]
    [BoxGroup(G2), Dropdown(DpName), SerializeField] private Transform _selectedSpawnPoint;
    [BoxGroup(G2), SerializeField] private Transform[] _spawnPoints = {};
    
    private void Start()
    {
        TryTeleportPlayerToActiveSpawnPoint();
        MakeEverySpawnPointMeshDisappear();
    }

    private void TryTeleportPlayerToActiveSpawnPoint()
    {
        if (_player is not null)
        {
            // Checks if the selected spawn point is valid. If it is, teleports the player to it.
            if (_selectedSpawnPoint is not null) _player.transform.position = _selectedSpawnPoint.position;
            else Debug.LogWarning("Spawn point index set to PlayerSpawnPointManager is invalid.");
        }
        else
        {
            Debug.LogWarning("Player is not set in PlayerSpawnPointsManager.");
        }
    }

    private void MakeEverySpawnPointMeshDisappear()
    {
        foreach (Transform spawnPoint in _spawnPoints)
        {
            Renderer[] renderers = spawnPoint.gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
                renderer.enabled = false;
        }
    }
    
}
