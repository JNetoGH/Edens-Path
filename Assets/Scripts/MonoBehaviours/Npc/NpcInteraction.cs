using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This class is the main class of the NPC Interaction System.
///
/// It contemplates events that will be called, when the player presses the interaction key,
/// while the player is in range for the interaction, and looking at the look collider.
/// 
/// This class requires a Look collider, which ensures the player is facing the npc while inside the presence collider.
/// It prevents the player to talk with NPCs not looking to them.
/// This functionality is outsourced to NpcInteractionLookColliderFinder.cs
/// </summary>
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
    /// This property encapsulates the system and can be used in other scripts.
    /// It's used in NpcLookAtPlayerWhileInPresenceTrigger.cs to rotate the npc towards the player.
    /// </summary>
    public bool IsPlayerInRange 
    {
        get
        {
            if (_player is null)
                return false;
            
            float distanceToPlayer = (_player.transform.position - transform.position).magnitude;
            bool isPlayerInRange = (distanceToPlayer <= _interactionRange);
            return isPlayerInRange;
        }
    }
    
    private const string G1 = "INTERACTION SYSTEM SETTINGS";
    [HorizontalLine]
    [BoxGroup(G1), SerializeField] private Collider _interactionLookCollider;
    [BoxGroup(G1), Range(1, 30), SerializeField] private float _interactionRange = 5;
    
    private const string G2 = "INTERACTION MENSAGE";
    [HorizontalLine]
    [BoxGroup(G2), SerializeField] private string _name = "None";
    private TextMeshProUGUI _uiMsg;
    
    private const string G3 = "EVENTS";
    [HorizontalLine] 
    [BoxGroup(G3), SerializeField] private UnityEvent _onPlayerInteract;
    
    private GameObject _player;
    
    private void Start()
    {
        _uiMsg = GameManager.NpcInteractableAnimationMsg;
        _player = FindObjectOfType<PlayerController>().gameObject;
        if (_player is null)
            Debug.LogWarning($"NpcInteraction.cs at {gameObject.name} could no find Player");
    }
    
    private void Update()
    {
        // Disables the uiMsg.
        _uiMsg.transform.parent.gameObject.SetActive(false);
        
        if (!IsPlayerInRange)
            return;
        
        if (!IsPlayerLookingAtLookCollider)
            return;
        
        // Player is in range and looking at the LookCollider
        _uiMsg.text = $"press E to talk with {_name}";
        _uiMsg.transform.parent.gameObject.SetActive(true);
        
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        Debug.Log($"player interacted with {gameObject.name} NpcInteraction.cs Trigger");
        _onPlayerInteract.Invoke();
    }
    
    
    /// <summary>
    /// Draw a wire sphere representing the interaction range in the Scene view
    /// </summary>
    private void OnDrawGizmos()
    {
        // Draws a wire sphere representing the interaction range in the Scene view
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _interactionRange);

        Vector3 dotPosition = transform.position + Vector3.up * _interactionRange;

        // Draws a solid cyan dot on top of the wire sphere to represent a point
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(dotPosition, 0.25f); // 0.25f is the radius of the solid sphere
    }
    
}
