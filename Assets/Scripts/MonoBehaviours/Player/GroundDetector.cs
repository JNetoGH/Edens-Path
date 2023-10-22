using UnityEngine;


[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SphereCollider))]
public class GroundDetector : MonoBehaviour
{
    
    // Used to communicate with other scripts.
    public bool IsGrounded { get; private set; } = false;
    
    [Header("Settings")]
    [SerializeField] private LayerMask _considerLayers;
    [SerializeField] private LayerMask _ignoreLayers;
    [SerializeField] private float _yLimitSpeedSafetyMargin = 0.4f;
    
    [Header("Visualization")]
    [SerializeField] private bool _showGroundDetector = false;
    [SerializeField] private Color _groundedColor = Color.red;
    [SerializeField] private Color _notGroundedColor = Color.green;
    [SerializeField] private bool _printDebugStatus;
    
    // Components
    private Renderer _groundDetectorRenderer;
    private PlayerController _playerController;
    
    private void Awake()
    {
        IsGrounded = false;
        _groundDetectorRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        if (_playerController == null)
            Debug.LogWarning("The Ground Detector could not find the PlayerController.cs");
    }

    private void OnTriggerStay(Collider other)
    {
        // checks if the player is stopped in Y
        //if (Mathf.Abs(_playerController.VerticalVelocity) > _yLimitSpeedSafetyMargin)
            //IsGrounded = false;
        if (LayerMaskContainsLayer(_considerLayers, other.gameObject.layer) && 
            !LayerMaskContainsLayer(_ignoreLayers, other.gameObject.layer))
            if (other.gameObject.GetComponent<PlayerController>() == null)
                IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMaskContainsLayer(_considerLayers, other.gameObject.layer) && 
            !LayerMaskContainsLayer(_ignoreLayers, other.gameObject.layer))
            if (other.gameObject.GetComponent<PlayerController>() == null)
                IsGrounded = false;
    }

    private void Update()
    {
        if (_printDebugStatus)
            Debug.Log($"Is Grounded {IsGrounded}");
        SyncVisualizationColor();
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

    private bool LayerMaskContainsLayer(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
