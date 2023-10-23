using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    
    private void Start()
    {
        // Teleports the player.
        _player.transform.position = transform.position;
        
        // Disappears on play.
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
            renderer.enabled = false;
        
    }
}
