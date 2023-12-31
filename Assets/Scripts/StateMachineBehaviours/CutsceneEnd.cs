﻿using Cinemachine;
using UnityEngine;


/// <summary>
/// State machine behaviour to handle the end of a cutscene.
/// Activates necessary game objects and enables the PickupSystem after the cutscenes.
/// </summary>
public class CutsceneEnd : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Unlocks many systems
        GameManager.EnterGameplayMode();

        // Deactivates the CinemachineBrain object to disable camera overriding after the cutscene.
        FindObjectOfType<CinemachineBrain>(includeInactive: true).gameObject.SetActive(false);
    }
}
