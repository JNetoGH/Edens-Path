using UnityEngine;
using UnityEngine.Serialization;


public class BurningTreeCutsceneController : MonoBehaviour
{
    
    [Header("Overriding")]
    [SerializeField] private bool _overrideBurningTreeCutscenePlay = false;
    
    [Header("References")]
    [SerializeField] private GameObject _originalTree;
    [SerializeField] private GameObject _burntTree;
    [SerializeField] private GameObject _birdContainer;
    [SerializeField] private GameObject _bird;
    [SerializeField] private GameObject _vinylDisc;

    // Animator
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_overrideBurningTreeCutscenePlay)
            return;
        TriggerBurningTreeCutscene();
        _overrideBurningTreeCutscenePlay = false;
    }
    
    // Called by the BurningTreeCollisionTrigger
    public void TriggerBurningTreeCutscene()
    {
        _animator.SetTrigger("StartSequence");
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
