using NaughtyAttributes;
using UnityEngine;


public class FloraCutsceneController : ACutsceneController
{
    
    [BoxGroup, Required, SerializeField] private GameObject _flora;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public override void PlayCutscene()
    {
        GameObject playerCamera = FindObjectOfType<CameraController>().gameObject;
        
        hasBeenAlreadyWatched = true;
        _audioSource.Play();
        
        GameManager.EnterCutsceneMode();
    }
    
}
