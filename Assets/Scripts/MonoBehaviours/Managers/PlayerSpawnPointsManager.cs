using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Gather spawn points (Transforms) and teleports the player to it.
/// After teleporting, Makes every spawn point mesh disappear.
/// </summary>
public class PlayerSpawnPointsManager : MonoBehaviour
{

    [Header("Player Settings")]
    [SerializeField] private GameObject _player;
    
    [Header("Spawn Points Settings")]
    [SerializeField] private int _indexActiveSpawnPoint;
    [SerializeField] private List<Transform> _spawnPoints;
    private bool IsIndexValid => _indexActiveSpawnPoint < _spawnPoints.Count && _indexActiveSpawnPoint >= 0;
    
    private void Start()
    {
        TryTeleportPlayerToActiveSpawnPoint();
        MakeEverySpawnPointMeshDisappear();
    }

    private void TryTeleportPlayerToActiveSpawnPoint()
    {
        if (_player is not null)
        {
            // Checks if the index is valid. If it is, teleports the player to it.
            if (IsIndexValid) _player.transform.position = _spawnPoints[_indexActiveSpawnPoint].position;
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
