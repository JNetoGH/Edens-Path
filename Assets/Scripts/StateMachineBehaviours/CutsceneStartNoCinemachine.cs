﻿using Cinemachine;
using UnityEngine;


/// <summary>
/// State machine behaviour to handle the start of a cutscene. 
/// Activates necessary game objects and disables the PickupSystem during the cutscenes.
/// </summary>
public class CutsceneStartNoCinemachine : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        GameManager.IsInCutscene = true;

        // Deactivate screen cross.
        GameManager.DeactivateScreenCross();

        // Release the current object held by the PickupSystem and disable the PickupSystem.
        PickupSystem pickupSystem = FindObjectOfType<PickupSystem>();
        pickupSystem.ReleaseCurrentObject(false);
        pickupSystem.enabled = false;
        
        // Makes the player Immovable
        PlayerController.CanMove = false;
        
        // Disables the player unable to open the inventory during cutscenes
        GameManager.CanOpenInventory = false;
    }
}
