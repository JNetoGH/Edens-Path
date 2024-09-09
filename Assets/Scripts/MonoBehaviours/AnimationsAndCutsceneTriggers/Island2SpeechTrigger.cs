using UnityEngine;


[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(AudioSource))]
public class Island2SpeechTrigger : ACutsceneController
{

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        
        // hides the square mesh while in game.
        GetComponent<MeshRenderer>().enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() is null)
            return;

        if (HasBeenAlreadyWatched)
            return;
        
        hasBeenAlreadyWatched = true;
        PlayCutscene();
    }

    public override void PlayCutscene()
    {
        _audioSource.PlayOneShot(_audioSource.clip);
    }
}