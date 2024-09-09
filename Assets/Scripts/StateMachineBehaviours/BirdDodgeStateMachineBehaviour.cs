using UnityEngine;

public class BirdDodgeStateMachineBehaviour : StateMachineBehaviour
{
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BirdDodgeTrigger.Instance.hasFinishedPreviousDodge = false;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BirdDodgeTrigger.Instance.hasFinishedPreviousDodge = true;
    }
    
}
