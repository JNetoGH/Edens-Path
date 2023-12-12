using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SphereCollider))]
public class GroundDetector : MonoBehaviour
{
    
    // Used to other scripts to get the detector's state.
    public bool IsGrounded { get; private set; } = false;
    
    [Header("Settings")]
    [SerializeField] private LayerMask _considerLayers;
    [SerializeField] private LayerMask _ignoreLayers;
    
    [Header("Debugging")]
    [SerializeField] private bool _showGroundDetector = false;
    [SerializeField, ShowIf("_showGroundDetector")] private Color _groundedColor = Color.green;
    [SerializeField, ShowIf("_showGroundDetector")] private Color _notGroundedColor = Color.red;
    
    [Header("Debugging (Ready-Only)")] 
    [SerializeField, ShowAssetPreview(300, 300), ReadOnly] private GameObject _collidingWith;
    
    // Components
    private Renderer _groundDetectorRenderer;
    
    private void Awake()
    {
        IsGrounded = false;
        _groundDetectorRenderer = GetComponent<Renderer>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (ValidateColliderLayer(other))
        {
            IsGrounded = true;
            _collidingWith = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ValidateColliderLayer(other))
        {
            IsGrounded = false;
            _collidingWith = null;
        }
    }
    
    private void Update()
    {
        SyncVisualizationColor();
    }
    
    private bool ValidateColliderLayer(Collider other)
    {
        bool isInValidLayer = LayerMaskContainsLayer(_considerLayers, other.gameObject.layer);
        bool isInIgnoredLayer = LayerMaskContainsLayer(_ignoreLayers, other.gameObject.layer);
        bool isPlayer = other.gameObject.GetComponent<PlayerController>() != null;
        bool isPresenceChecker = other.gameObject.GetComponent<PickableObjectPresenceChecker>() != null;
        return isInValidLayer && !isInIgnoredLayer && !isPlayer && !isPresenceChecker;
    }

    private bool LayerMaskContainsLayer(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
    
    private void SyncVisualizationColor()
    {
        // Verifies the state and syncs the color
        _groundDetectorRenderer.enabled = _showGroundDetector;
        if (_groundDetectorRenderer.enabled)
        {
            if (IsGrounded) _groundDetectorRenderer.material.color = _groundedColor;
            else _groundDetectorRenderer.material.color = _notGroundedColor;
        }
    }
    
}
