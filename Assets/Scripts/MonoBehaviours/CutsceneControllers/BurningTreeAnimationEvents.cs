using System;
using UnityEngine;


public class BurningTreeAnimationEvents : MonoBehaviour
{

    [SerializeField] private bool _overrideBurningTreeSeqStart = false;

    private void Update()
    {
        if (!_overrideBurningTreeSeqStart)
            return;
        Level1Manager.Instance.TriggerBurningTreeCutscene();
        _overrideBurningTreeSeqStart = false;
    }

    // Called by an animation event at the Burning tree sequence cutscene.
    public void TriggerBirdFlight()
    {
        Level1Manager.Instance.bird.GetComponent<Animator>().SetTrigger("Fly");
        Level1Manager.Instance.bird.GetComponent<Collider>().enabled = false;
        
        // Gets the 3D model og bird
        Level1Manager.Instance.bird.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Fly");
    }
    
    // Called by an animation event at the Burning tree sequence cutscene.
    public void SetDiscToBeInteractive()
    {
        Rigidbody rb = Level1Manager.Instance.vinylDisc.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;

        PickableObject pickObj = Level1Manager.Instance.vinylDisc.GetComponent<PickableObject>();
        pickObj.enabled = true;

        Level1Manager.Instance.vinylDisc.transform.parent = null;
    }
    
    // Called by an animation event at the Burning tree sequence cutscene.
    public void SetTreeToTheBurntModel()
    {
       Level1Manager.Instance.originalTreeModel.SetActive(false);
       Level1Manager.Instance.burntTreeModel.SetActive(true);
    }

    // Called by an animation event at the Burning tree sequence cutscene.
    public void DestroyBirdContainer()
    {
        Destroy(Level1Manager.Instance.birdContainer);
    }
    
}
