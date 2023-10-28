﻿using System.Collections.Generic;
using UnityEngine;


/// <summary>
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
            if (npcInteraction is null)
                continue;
            if (hit.collider != npcInteraction.InteractionLookCollider)
                continue;
            _interactionsFound.Add(npcInteraction);
        }
        
        // Notify all Valid Npc Interactions, that they have been found.
        bool anyValidHits = _interactionsFound.Count > 0;
        if (!anyValidHits)
            return;
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