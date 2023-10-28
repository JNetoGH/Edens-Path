using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is part of the NPC Interaction System.
/// It contemplates events that will be called:
///     1) When the player enters the presence trigger.
///     2) When the player exits the presence trigger.
///     3) When the player presses the interaction key when the player is inside the presence trigger,
///        and looking to the look collider.
/// 
/// This class requires:
///     - A presence collider that needs to be a trigger, which ensures the player is in range.
///     - A Look collider, which ensures the player is facing the npc while inside the presence collider.
/// </summary>
[RequireComponent(typeof(Collider))]
public class NpcInteraction : MonoBehaviour
{
    
    /// <summary>
    /// The PlayerNpcInteractionRayCaster.cs must be looking at this collider, in order to execute te interaction.
    /// It prevents the player to talk with NPCs not looking to them.
    /// </summary>
    public Collider InteractionLookCollider => _interactionLookCollider;
    
    /// <summary>
    /// This property is manipulated by the NpcInteractionLookColliderFinder.cs,
    /// and indicates whether or not the player is looking at the look collider.
    /// </summary>
    public bool IsPlayerLookingAtLookCollider { get; set; }
    
    /// <summary>
    /// This property encapsulates the system to be used in other scripts.
    /// It's used in NpcLookAtPlayerWhileInPresenceTrigger.cs to rotate the npc towards the player.
    /// </summary>
    public bool IsPlayerInsidePresenceTrigger { get; private set; }
    
    private const string G1 = "REQUIRED REFERENCES";
    [SerializeField, BoxGroup(G1)] private Collider _interactionLookCollider;
    
    private const string G2 = "INTERACTION MENSAGE";
    [HorizontalLine]
    [BoxGroup(G2), SerializeField] private string _name = "None";
    private TextMeshProUGUI _uiMsg;
    
    private const string G3 = "EVENTS";
    [HorizontalLine] 
    [BoxGroup(G3), SerializeField] private UnityEvent _onPlayerInteraction;
    [BoxGroup(G3), SerializeField] private UnityEvent _onPlayerEnterTrigger;
    [BoxGroup(G3), SerializeField] private UnityEvent _onPlayerExitTrigger;
    
    private void Start()
    {
        _uiMsg = GameManager.NpcInteractableAnimationMsg;
        if (!GetComponent<Collider>().isTrigger)
            Debug.LogWarning($"NpcInteraction.cs in {gameObject.name} found a non-trigger Collider.");
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() is null)
            return;

        IsPlayerInsidePresenceTrigger = true;
        
        if (IsPlayerLookingAtLookCollider)
        {
            _uiMsg.text = $"press E to talk with {_name}";
            _uiMsg.transform.parent.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                _onPlayerInteraction.Invoke();
                Debug.Log($"player interacted with {gameObject.name} NpcInteraction.cs Trigger");
            }
        }
        else
        {
            _uiMsg.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() is null)
            return;
        
        Debug.Log($"player entered inside {gameObject.name} NpcInteraction.cs Trigger");
        _onPlayerEnterTrigger.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() is null)
            return;

        Debug.Log($"player exited inside {gameObject.name} NpcInteraction.cs Trigger");
        IsPlayerInsidePresenceTrigger = false;
        _onPlayerExitTrigger.Invoke();
        _uiMsg.transform.parent.gameObject.SetActive(false);
    }
    
}
