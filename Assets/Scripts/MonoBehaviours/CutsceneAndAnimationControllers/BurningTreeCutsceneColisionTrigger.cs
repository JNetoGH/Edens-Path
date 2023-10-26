using UnityEngine;

/// <summary>
/// This script represents a collision trigger for a burning tree cutscene.
/// It triggers the cutscene when the object with the specified tag ("Torch") collides or enters its trigger zone.
/// </summary>
public class BurningTreeCutsceneColisionTrigger : MonoBehaviour
{
    
    // A flag to ensure the cutscene is triggered only once
    private bool _hasTriggeredAlready;

    private void Start()
    {
        _hasTriggeredAlready = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasTriggeredAlready)
            return;

        if (collision.gameObject.tag.Equals("Torch"))
        {
            FindObjectOfType<BurningTreeACutsceneController>().PlayCutscene();
            _hasTriggeredAlready = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggeredAlready)
            return;

        if (other.gameObject.tag.Equals("Torch"))
        {
            FindObjectOfType<BurningTreeACutsceneController>().PlayCutscene();
            _hasTriggeredAlready = true;
        }
    }
}
