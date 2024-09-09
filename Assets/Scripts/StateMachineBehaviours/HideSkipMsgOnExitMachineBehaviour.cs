using UnityEngine;


public class HideSkipMsgOnExitMachineBehaviour : StateMachineBehaviour
{
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        GameManager.SkipCutsceneMsg.SetActive(false);
    }
    
}
