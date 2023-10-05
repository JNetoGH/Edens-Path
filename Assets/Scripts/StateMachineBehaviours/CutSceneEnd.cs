using Cinemachine;
using UnityEngine;


/// <summary>
/// State machine behaviour to handle the end of a cutscene.
/// Activates necessary game objects and enables the PickupSystem after the cutscenes.
/// </summary>
public class CutSceneEnd : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Activate screen cross.
        GameManager.ActivateScreenCross();
        
        // Enables the PickupSystem.
        FindObjectOfType<PickupSystem>().enabled = true;

        // Deactivates the CinemachineBrain object to disable camera overriding after the cutscene.
        FindObjectOfType<CinemachineBrain>(includeInactive: true).gameObject.SetActive(false);
        
        // Makes the player Movable
        PlayerController.CanMove = true;
        
        // Enables the player to open inventory out of cutscenes
        GameManager.CanOpenInventory = true;
    }
}
