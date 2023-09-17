using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    
    [Header("General")]
    [SerializeField] private bool _printDebuggingStatus;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpForce = 10f;
    
    [Header("Custom Gravity")]
    [SerializeField] private bool _useCustomGravity = true;
    [SerializeField] private float _customGravity = 0.5f;
    [SerializeField, Range(1, 10), Tooltip(TipThreshold)] private float _customGravityThreshold = 10f;
    private const string TipThreshold = "It's the limit of the Y axis velocity";
    
    [Header("Camera")] 
    [SerializeField] private bool _allowCameraFollow = true;
    [SerializeField, Range(1, 100), Tooltip(TipUpperLook)] private float _upperLookLimitAngle = 80f; 
    [SerializeField, Range(1, 100), Tooltip(TipLowerLook)] private float _lowerLookLimitAngle = 80f; 
    private const string TipUpperLook = "How many degrees the player can look up before the camera stops moving";
    private const string TipLowerLook = "How many degrees the player can look down before the camera stops moving";
    public float LookSpeedX { get; set; } = 1; // Set by the user using the Game Handler via Menu
    public float LookSpeedY { get; set; } = 1; // Set by the user using the Game Handler via Menu
    private Transform _cameraFollowTarget;     // The point where the camera will sync with its position
    private float _cameraRotationX;            // Holds the Camera ↑ ↓ rotation
    private float _cameraRotationY;            // Holds the Camera ← → rotation
    
    // Status
    public bool CanMove { get; set; } = true;
    
    // Components
    private Rigidbody _rigidbody;
    private Camera _playerCamera;
    private GroundDetector _groundDetector;
    
    // Inputs
    private Vector2 _moveDir;
    private bool _jumpInputBuffer;
    
    private void Awake()
    {
        CanMove = true;
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerCamera = FindObjectOfType<Camera>();
        _groundDetector = GetComponentInChildren<GroundDetector>();
        _cameraFollowTarget = GameObject.Find("Camera Follow Target").transform;
    }
    
    void Update()
    {
        if (_printDebuggingStatus) PrintDebuggingStatus();
        if (!CanMove) return;
        UpdateInputs();
        HandleMouseLook();
    }

    private void FixedUpdate()
    {
        if (_useCustomGravity) ApplyCustomGravity();
        if (_jumpInputBuffer && CanMove) Jump();
        if (CanMove) Move();
        if (_allowCameraFollow) CameraFollow();
    }
    
    private void UpdateInputs()
    {
        // Get the inputs, then, normalizes the Dir Vector using clamp in order to keep the axis smoothing.
        _moveDir = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")); 
        _moveDir = Vector2.ClampMagnitude(_moveDir, 1f);
        
        // Jump Input Buffering 
        if (Input.GetButtonDown("Jump") && _groundDetector.IsGrounded)
            _jumpInputBuffer = true;
        
        // Camera Follow
        if (Input.GetButtonDown("Camera Follow"))
            _allowCameraFollow = !_allowCameraFollow;
    }
    
    private void HandleMouseLook()
    {
        // Rotates the player, it doesn't need to be clamped because the player can freely rotate
        // in 360° around the Y axis, unlike the X axis where the player's neck would break.
        // The mouse's X axis (X: ← →, Y: ↑ ↓) rotates the player in the Y axis (X: ↑ ↓ "front", Y: ← →, Z: ↻ ↺ "lateral")
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeedX, 0);
        
        // Rotates the camera on the Y axis:
        // Just syncs the rotation with the Player in this axis.
        _cameraRotationY = this.transform.rotation.eulerAngles.y;
        
        // Rotates the camera on the X axis:
        // The mouse's Y axis (X: ← →, Y: ↑ ↓) rotates the camera in the X axis (X: ↑ ↓ "front", Y: ← →, Z: ↻ ↺ "lateral")
        // Then clamps the rotation of the camera to the limits
        _cameraRotationX -= Input.GetAxis("Mouse Y") * LookSpeedY;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -_upperLookLimitAngle, _lowerLookLimitAngle);
        
        // Applies the rotations to the camera
        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotationX, _cameraRotationY, 0);
    }
    
    private void ApplyCustomGravity()
    {
        // Applies custom gravity to an aux variable Y axis, if grounded, removes it.
        Vector3 newVelocity = _rigidbody.velocity;
        newVelocity.y = _groundDetector.IsGrounded ? 0 : newVelocity.y - _customGravity;
        
        // Checks ig the Y axis of the velocity has reached the threshold.
        if (newVelocity.y < -_customGravityThreshold)
            newVelocity.y = -_customGravityThreshold;
        
        // Applies the aux variable to the velocity.
        _rigidbody.velocity = newVelocity;
    }

    private void Jump()
    {   
        // Simply overrides the velocity and clears the input buffer.
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
        _jumpInputBuffer = false;
    }
    
    private void Move()
    {
        // Moves the Object According to the inputs, but keeps the Y velocity as it was before applying
        // the new velocity, in order to do not override the custom/default gravity.
        Vector3 newVelocity = (transform.forward * _moveDir.x * _moveSpeed) + (transform.right * _moveDir.y * _moveSpeed);
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;
    }

    private void CameraFollow()
    {
        _playerCamera.transform.position = _cameraFollowTarget.transform.position;
    }

    private void PrintDebuggingStatus()
    {
        Debug.Log($"Velocity Lenght: {_rigidbody.velocity.magnitude}");
        Debug.Log($"Player's Velocity: {_rigidbody.velocity}");
        Debug.Log($"Transformed Vector3.forward: {transform.TransformDirection(Vector3.forward)}");
        Debug.Log($"Transformed Vector3.right: {transform.TransformDirection(Vector3.right)}");
    }
    
}
