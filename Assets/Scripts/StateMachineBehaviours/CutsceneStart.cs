﻿using Cinemachine;
using UnityEngine;


/// <summary>
/// State machine behaviour to handle the start of a cutscene. 
/// Activates necessary game objects and disables the PickupSystem during the cutscenes.
/// </summary>
public class CutsceneStart : StateMachineBehaviour
{

    [SerializeField] private bool _useCinemachine = true;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Locks many systems
        GameManager.EnterCutsceneMode();

        if (_useCinemachine)
        {
            // Activate CinemachineBrain object to enable virtual camera control during the cutscene.
            FindObjectOfType<CinemachineBrain>(includeInactive: true).gameObject.SetActive(true);
        }
        
    }
}

