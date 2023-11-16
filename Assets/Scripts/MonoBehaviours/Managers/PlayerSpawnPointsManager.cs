using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// Gather spawn points (Transforms) and teleports the player to it.
/// After teleporting, Makes every spawn point mesh disappear.
/// </summary>
public class PlayerSpawnPointsManager : MonoBehaviour
{

    [Header("Debugging Read-Only")]
    [SerializeField, ReadOnly] private bool _hasAlreadyTeleported;
    
    [Header("Player Reference")]
    [SerializeField, Required] private GameObject _player;
    
    [Header("Spawn Point Settings")]
    [SerializeField, Dropdown("_spawnPoints")] private Transform _selectedSpawnPoint;
    [SerializeField] private Transform[] _spawnPoints = {};
    
    private void Start()
    {
        TriggerTeleport();        
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
            TriggerTeleport();
    }

    private void LateUpdate()
    {
        if (!_hasAlreadyTeleported)
        {
            TryTeleportPlayerToActiveSpawnPoint();
            MakeEverySpawnPointMeshDisappear();
            _hasAlreadyTeleported = true;
        }
    }
    
    private void TriggerTeleport()
    {
        _hasAlreadyTeleported = false;
    }

    private void TryTeleportPlayerToActiveSpawnPoint()
    {
        if (_player is not null)
        {
            // Checks if the selected spawn point is valid. If it is, teleports the player to it.
            if (_selectedSpawnPoint is not null)
            {
                _player.transform.position = _selectedSpawnPoint.position;
                Debug.Log($"Player has teleported to the spawn point successfully");
            }
            else
            {
                Debug.LogWarning("Spawn point index set to PlayerSpawnPointManager is invalid.");
            }
            _hasAlreadyTeleported = true;
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
