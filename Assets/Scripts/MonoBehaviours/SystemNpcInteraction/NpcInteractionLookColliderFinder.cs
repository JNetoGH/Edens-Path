using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// This class is a critical part of the NPC Interaction System.
/// 
/// This class casts a ray from the player's camera trying to find a valid look collider.
/// It manipulates the IsPlayerLookingAtLookCollider property.
/// It prevents the player to talk with NPCs not looking to them.
/// </summary>
public class NpcInteractionLookColliderFinder : MonoBehaviour
{
    
    /// <summary>
    /// Cached valid NpcInteractions found in the last frame.
    /// </summary>
    private List<NpcInteraction> _interactionsFound = new List<NpcInteraction>();

    /// <summary>
    /// Holds when the player is looking to any look collider
    /// </summary>
    /// <OBS>
    /// Displayed on inspector in read-only mode for debugging. 
    /// </OBS>
    [SerializeField, ReadOnly] private bool _isPlayerLookingAtAnyInteraction;

    private void Update()
    {
        ClearNpcInteractionsCache();
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 10F);
        
        bool foundAnything = hits.Length > 0;
        if (!foundAnything)
            return;
        
        // Checks if there are any Valid Npc Interaction found this frame.
        foreach (RaycastHit hit in hits)
        {
            NpcInteraction npcInteraction = hit.collider.gameObject.GetComponent<NpcInteraction>();
            
            // Is not an NpcInteraction
            if (npcInteraction is null)
                continue;

            // Is turned on 
            if (npcInteraction.enabled == false)
                continue;
            
            // If the collider found is not the look collider.
            if (hit.collider != npcInteraction.InteractionLookCollider)
                continue;
            
            _interactionsFound.Add(npcInteraction);
        }
        
        // Disables the ui msg and return in case the player isn't looking to any look collier. 
        _isPlayerLookingAtAnyInteraction = _interactionsFound.Count > 0;
        if (!_isPlayerLookingAtAnyInteraction)
        {
            GameManager.NpcInteractableAnimationMsg.transform.parent.gameObject.SetActive(false);
            return;
        }
        
        // Notify all Valid Npc Interactions, that they have been found.
        foreach (NpcInteraction npcInteraction in _interactionsFound)
            npcInteraction.IsPlayerLookingAtLookCollider = true;
    }

    /// <summary>
    /// Clears the cache of NpcInteractions and resets their IsPlayerLookingAtLookCollider property.
    /// </summary>
    private void ClearNpcInteractionsCache()
    {
        foreach (NpcInteraction npcInteraction in _interactionsFound)
            npcInteraction.IsPlayerLookingAtLookCollider = false;
        _interactionsFound = new List<NpcInteraction>();
    }
    
}
