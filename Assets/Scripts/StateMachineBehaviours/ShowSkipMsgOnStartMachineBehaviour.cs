using UnityEngine;


public class ShowSkipMsgOnStartMachineBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.SkipCutsceneMsg.SetActive(true);
    }
    
}
