using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// This script controls the burning tree cutscene sequence.
/// It handles the cutscene and its to animation events.
/// </summary>
public class BurningTreeCutsceneController : ACutsceneController
{
    
    [Header("REQUIRED REFERENCES"), HorizontalLine]
    [BoxGroup, Required, SerializeField] private GameObject _originalTree;
    [BoxGroup, Required, SerializeField] private GameObject _burntTree;
    [BoxGroup, Required, SerializeField] private GameObject _birdContainer;
    [BoxGroup, Required, SerializeField] private GameObject _bird;
    [BoxGroup, Required, SerializeField] private GameObject _vinylDisc;
    
    // Called by the BurningTreeCollisionTrigger
    public  override void PlayCutscene()
    {
        animator.SetTrigger("StartSequence");
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    #region Called By Animation Events
    
        // Called by an animation event at the Burning tree sequence cutscene.
        private void TriggerBirdFlight()
        {
            _bird.GetComponent<Animator>().SetTrigger("Fly");
            _bird.GetComponent<Collider>().enabled = false;
            
            // Gets the 3D model og bird
            _bird.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Fly");
        }
        
        // Called by an animation event at the Burning tree sequence cutscene.
        private void SetDiscToBeInteractive()
        {
            Rigidbody rb = _vinylDisc.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;

            PickableObject pickObj = _vinylDisc.GetComponent<PickableObject>();
            pickObj.enabled = true;

            _vinylDisc.transform.parent = null;
        }
        
        // Called by an animation event at the Burning tree sequence cutscene.
        private void SetTreeToTheBurntModel()
        {
           _originalTree.SetActive(false);
           _burntTree.SetActive(true);
        }

        // Called by an animation event at the Burning tree sequence cutscene.
        private void DestroyBirdContainer()
        {
            Destroy(_birdContainer);
        }
    
    #endregion
    
}
