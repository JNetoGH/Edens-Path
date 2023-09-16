using UnityEngine;
using UnityEngine.Serialization;

public class GroundDetector : MonoBehaviour
{

    public bool IsGrounded { get; private set; } = false;
    
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _yLimitSpeed = 0.4f;
    [SerializeField] private bool _printDebugStatus;
    
    private Rigidbody _playerRigidbody; 
    
    private void Awake()
    {
        IsGrounded = false;
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
        Debug.Log(_playerRigidbody.velocity);
        if (_printDebugStatus)
            Debug.Log($"Is Grounded {IsGrounded}");
    }

    private bool LayerMaskContainsLayer(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
