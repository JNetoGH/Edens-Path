using UnityEngine;

/// <summary>
/// This script represents a collision trigger for a burning tree cutscene.
/// It triggers the cutscene when the object with the specified tag ("Torch") collides or enters its trigger zone.
/// </summary>
public class BurningTreeCutsceneColisionTrigger : MonoBehaviour
{

    private ACutsceneController _cutscene;
    
    // A flag to ensure the cutscene is triggered only once
    private bool _hasTriggeredAlready;

    private void Start()
    {
        _hasTriggeredAlready = false;
        _cutscene = FindObjectOfType<BurningTreeCutsceneController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // In case the cutscene has been already watched, the interactions will be simply ignored.
        if (_cutscene.HasBeenAlreadyWatched)
            return;
        
        if (_hasTriggeredAlready)
            return;
        
        if (collision.gameObject.tag.Equals("Torch"))
        {
            _cutscene.PlayCutscene();
            _hasTriggeredAlready = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // In case the cutscene has been already watched, the interactions will be simply ignored.
        if (_cutscene.HasBeenAlreadyWatched)
            return;
        
        if (_hasTriggeredAlready)
            return;

        if (other.gameObject.tag.Equals("Torch"))
        {
            _cutscene.PlayCutscene();
            _hasTriggeredAlready = true;
        }
    }
}
