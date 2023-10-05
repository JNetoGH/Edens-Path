using UnityEngine;


public class BurningTreeAnimationEvents : MonoBehaviour
{
  
    // Called by an animation event at the Burning tree sequence cutscene.
    public void TriggerBirdFlight()
    {
        Level1Manager.Instance.bird.GetComponent<Animator>().SetTrigger("Fly");
        Level1Manager.Instance.bird.GetComponent<Collider>().enabled = false;
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
