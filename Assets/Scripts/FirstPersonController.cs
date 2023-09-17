using UnityEngine;
using UnityEngine.Serialization;

public class FirstPersonController : MonoBehaviour
{
    // Status
    public bool CanMove { get; set; } = true;
    
    [Header("General")]
    [SerializeField] private bool _printDebuggingStatus;
    
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 3f;
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
    public float LookSpeedX { get; set; } = 1; // Set by the user using the Game Handler
    public float LookSpeedY { get; set; } = 1; // Set by the user using the Game Handler
    private Transform _cameraFollowTarget; // The point where the camera will sync with its position
    
    // Components
    private Camera _playerCamera;
    private GroundDetector _groundDetector;
    private Rigidbody _rigidbody;

    // Inputs
    private Vector2 _moveInputs;
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
        if (_printDebuggingStatus)
            PrintDebuggingStatus();
        if (!CanMove)
            return;
        UpdateInputs();
        HandleMouseLook();
    }

    private void FixedUpdate()
    {
        if (_useCustomGravity)
            ApplyCustomGravity();
        if (!CanMove)
            return;
        if (_jumpInputBuffer) 
            Jump();
        Move();
    }
    
    private void UpdateInputs()
    {
        _moveInputs = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        
        // Normalizes the Dir Vector using clamp in order to not give up on the axis smoothening.
        _moveInputs = Vector2.ClampMagnitude(_moveInputs, 1f);
        
        // Jump Input Buffering 
        if (Input.GetButtonDown("Jump") && _groundDetector.IsGrounded)
            _jumpInputBuffer = true;
    }
    
    private void HandleMouseLook()
    {
        // Rotates the player, it doesn't need to be clamped because the player can freely rotate
        // in 360° around the Y axis, unlike the X axis where the player's neck would break.
        // The mouse's X axis (X: ← →, Y: ↑ ↓) rotates the player in the Y axis (X: ↑ ↓ "front", Y: ← →, Z: ↻ ↺ "lateral")
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeedX, 0);
        
        // Rotates the camera on the Y axis:
        // Just syncs the rotation with the Player in this axis.
        Vector3 cam = _playerCamera.transform.rotation.eulerAngles;
        _playerCamera.transform.rotation = Quaternion.Euler(cam.x, transform.rotation.eulerAngles.y, 0);
        
        // Rotates the camera on the X axis:
        // The mouse's Y axis (X: ← →, Y: ↑ ↓) rotates the camera in the X axis (X: ↑ ↓ "front", Y: ← →, Z: ↻ ↺ "lateral")
        // Then clamps the rotation of the camera to the limits
        cam = _playerCamera.transform.rotation.eulerAngles;
        float camRotationX = cam.x; 
        camRotationX -= Input.GetAxis("Mouse Y") * LookSpeedY;
        camRotationX = Mathf.Clamp(camRotationX, -_upperLookLimitAngle, _lowerLookLimitAngle);
        _playerCamera.transform.localRotation = Quaternion.Euler(camRotationX, cam.y, 0);
    }
    
    private void ApplyCustomGravity()
    {
        // Applies custom gravity to an aux variable
        Vector3 auxVelocityHolder = _rigidbody.velocity;
        if (!_groundDetector.IsGrounded)
            auxVelocityHolder.y -= _customGravity;
        else
            auxVelocityHolder.y = 0;
        
        // Checks ig the Y axis of the velocity has reached the threshold
        if (auxVelocityHolder.y < -_customGravityThreshold)
            auxVelocityHolder.y = -_customGravityThreshold;
        
        // Applies the aux variable to the velocity
        _rigidbody.velocity = auxVelocityHolder;
    }

    private void Jump()
    {
        Vector3 auxVelocityHolder = _rigidbody.velocity;
        auxVelocityHolder.y = _jumpForce;
        _rigidbody.velocity = auxVelocityHolder;
        _jumpInputBuffer = false;
    }
    
    private void Move()
    {
        // Moves the Object According to the inputs
        Vector3 auxVelocityHolder = (transform.TransformDirection(Vector3.forward) * _moveInputs.x * _walkSpeed) + 
                                    (transform.TransformDirection(Vector3.right) * _moveInputs.y * _walkSpeed);
        
        // Conservatives the Y velocity before applying in order to do not override the gravity
        auxVelocityHolder.y = _rigidbody.velocity.y;
        _rigidbody.velocity = auxVelocityHolder;
        
        // Camera Follow
        if (_allowCameraFollow)
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
