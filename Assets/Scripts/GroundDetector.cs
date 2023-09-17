using UnityEngine;

public class GroundDetector : MonoBehaviour
{

    public bool IsGrounded { get; private set; } = false;
    
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _yLimitSpeed = 0.4f;
    [SerializeField] private bool _showGroundDetector = false;
    [SerializeField] private bool _printDebugStatus;
    
    // Components
    private Rigidbody _playerRigidbody; 
    private Renderer _renderer;
    
    private void Awake()
    {
        IsGrounded = false;
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _playerRigidbody = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        // Checks if the player is moving in Y (there is a tolerance) before checking for the layer.
        // It will only accuses ground when the player is in fact stopped on the ground,
        // instead of relying on the Detector's trigger contact only.
        if (Mathf.Abs(_playerRigidbody.velocity.y) > _yLimitSpeed)
            IsGrounded = false;
        else if (LayerMaskContainsLayer(_groundLayers, other.gameObject.layer))
            IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMaskContainsLayer(_groundLayers, other.gameObject.layer))
            IsGrounded = false;
    }

    private void Update()
    {
        if (_printDebugStatus)
            Debug.Log($"Is Grounded {IsGrounded}");
        
        // Verifies the control field and syncs the color.
        _renderer.enabled = _showGroundDetector;
        if (_renderer.enabled)
            _renderer.material.color = IsGrounded ? Color.cyan : Color.red;
    }

    private bool LayerMaskContainsLayer(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}