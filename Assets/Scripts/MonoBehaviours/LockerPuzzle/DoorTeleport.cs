using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DoorTeleport : MonoBehaviour
{
    [SerializeField, Scene] private string _targetScene = "Name";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() is not null)
            SceneManager.LoadScene(_targetScene);
    }
}
