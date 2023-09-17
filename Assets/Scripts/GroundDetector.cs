using UnityEngine;
using UnityEngine.Serialization;

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
        // checks if the player is stopped in Y
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
        
        // Verifies the state and syncs the color
        _renderer.enabled = _showGroundDetector;
        if (_renderer.enabled)
        {
            if (IsGrounded)
                _renderer.material.color = Color.cyan;
            else
                _renderer.material.color = Color.red;
        }
       
    }

    private bool LayerMaskContainsLayer(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
