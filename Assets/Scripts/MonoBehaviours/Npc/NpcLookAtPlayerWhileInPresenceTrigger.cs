using System;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// This class is an optional part of the NPC Interaction System.
/// It makes the NPC look at player while he is inside the presence trigger.
/// </summary>
public class NpcLookAtPlayerWhileInPresenceTrigger : MonoBehaviour
{
    
    private const string G1 = "REQUIRED REFERENCES";
    [HorizontalLine]
    [BoxGroup(G1), Required, SerializeField] private NpcInteraction _relatedNpcInteraction;

    private const string G2 = "NPC SETTINGS";
    [HorizontalLine]
    [BoxGroup(G2), SerializeField] private float _angularSpeed = 10;
    
    // Npc
    private Quaternion _originalRotation;
    
    // Player
    private GameObject _player;
    
    private void Awake()
    {
        _originalRotation = transform.rotation;
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().gameObject;
        if (_player is null)
            Debug.LogWarning($"NpcLookAtPlayer.cs at {gameObject.name} could not find player.");
    }

    private void Update()
    {
        if (_player is null)
            return;
        
        if (_relatedNpcInteraction.IsPlayerInRange)
        {
            // Rotates towards player using Slerp.
            Vector3 distanceToPlayer = _player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(distanceToPlayer), _angularSpeed * Time.deltaTime);

            // Ignores all axis except Y.
            Vector3 curRot = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(_originalRotation.eulerAngles.x, curRot.y, _originalRotation.eulerAngles.z);
        }
        else
        {
            // Rotates back to original rotation using Slerp.
            transform.rotation = Quaternion.Slerp(transform.rotation, _originalRotation, _angularSpeed * Time.deltaTime);
        }
        
    }
}
