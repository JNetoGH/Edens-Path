using System;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// This script controls the burning tree cutscene sequence.
/// It handles the cutscene and its to animation events.
/// </summary>
public class BurningTreeCutsceneController : ACutsceneController
{
    
    [Header("REQUIRED REFERENCES"), HorizontalLine]
    [SerializeField, Required, BoxGroup] private GameObject _originalTree;
    [SerializeField, Required, BoxGroup] private GameObject _burntTree;
    [SerializeField, Required, BoxGroup] private GameObject _birdContainer;
    [SerializeField, Required, BoxGroup] private GameObject _bird;
    [SerializeField, Required, BoxGroup] private GameObject _vinylDisc;
    [SerializeField, Required, BoxGroup] private Transform _birdDroppingTarget; // used only in case of HasBeenAlreadyWatched.
    
    
    // Called by the BurningTreeCollisionTrigger
    public  override void PlayCutscene()
    {
        animator.SetTrigger("StartSequence");
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // In case the cutscene has been already watched:
        // The trigger won't trigger it and it will be set as it would be in its complete state.
        if (HasBeenAlreadyWatched)
        {
            SetDiscToBeInteractive();
            _vinylDisc.transform.position = _birdDroppingTarget.position;
            SetTreeToTheBurntModel();
            DestroyBirdContainer();
        }
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
