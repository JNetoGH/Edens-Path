using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// Gather spawn points (Transforms) and teleports the player to it.
/// After teleporting, Makes every spawn point mesh disappear.
/// </summary>
public class PlayerSpawnPointsManager : MonoBehaviour
{
    
    private const string G0 = "DEBBUGING";
    [HorizontalLine]
    [BoxGroup(G0), ReadOnly, SerializeField] private bool hasAlreadyTeleported;
    
    private const string G1 = "PLAYER REFERENCE";
    [HorizontalLine]
    [Required, BoxGroup(G1), SerializeField] private GameObject _player;

    private const string G2 = "SPAWN POINTS SETTINGS";
    private const string DP = nameof(_spawnPoints);
    [HorizontalLine]
    [BoxGroup(G2), Dropdown(DP), SerializeField] private Transform _selectedSpawnPoint;
    [BoxGroup(G2), SerializeField] private Transform[] _spawnPoints = {};
    
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
        if (!hasAlreadyTeleported)
        {
            TryTeleportPlayerToActiveSpawnPoint();
            MakeEverySpawnPointMeshDisappear();
            hasAlreadyTeleported = true;
        }
    }
    
    private void TriggerTeleport()
    {
        hasAlreadyTeleported = false;
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
            hasAlreadyTeleported = true;
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
