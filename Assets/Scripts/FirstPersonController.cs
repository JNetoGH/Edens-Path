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
    
    [Header("Camera Rotation")] 
    [SerializeField, Range(0, 0.2f)] private float _mouseThreshold = 0.1f; // Used to avoid jittering on the camera
    [SerializeField, Range(1, 100), Tooltip(TipUpperLook)] private float _upperLookLimitAngle = 80f; 
    [SerializeField, Range(1, 100), Tooltip(TipLowerLook)] private float _lowerLookLimitAngle = 80f; 
    private const string TipUpperLook = "How many degrees the player can look up before the camera stops moving";
    private const string TipLowerLook = "How many degrees the player can look down before the camera stops moving";
    public float LookSpeedX { get; set; } = 2; // Set by the user using the Game Handler via Menu
    public float LookSpeedY { get; set; } = 2; // Set by the user using the Game Handler via Menu
    private float _cameraRotationX;            // Holds the Camera ↑ ↓ rotation
 
    [Header("Camera Smoothing")] 
    [SerializeField] private bool _useSmoothing = true; // Used to avoid jittering on the camera
    [SerializeField, Range(0.001f, 0.01f)] private float _smoothTimeX = 0.001f;
    [SerializeField, Range(0.001f, 0.01f)] private float _smoothTimeY = 0.001f;
    private float _playerRotationVelocityY;    // Used at the smoothing method
    private float _cameraRotationVelocityX;    // Used at the smoothing method
    
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
        _playerCamera = GetComponentInChildren<Camera>();
        _groundDetector = GetComponentInChildren<GroundDetector>();
    }
    
    void Update()
    {
        UpdateInputs();
        if (_printDebuggingStatus) PrintDebuggingStatus();
        if (!CanMove) return;
        
        // Check for minimal mouse movement using a threshold to avoid camera jittering.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Debug.Log(mouseX + " | " + mouseY);
        if (Mathf.Abs(mouseX) > _mouseThreshold || Mathf.Abs(mouseY) > _mouseThreshold)
            UpdatePlayerAndCameraRotationFromMouseInput();
    }

    private void FixedUpdate()
    {
        if (_useCustomGravity) ApplyCustomGravity();
        if (_jumpInputBuffer && CanMove) Jump();
        if (CanMove) Move();
    }
    
    private void UpdateInputs()
    {
        // Get the inputs, then, normalizes the Dir Vector using clamp in order to keep the axis smoothing.
        _moveDir = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")); 
        _moveDir = Vector2.ClampMagnitude(_moveDir, 1f);
        
        // Jump Input Buffering 
        if (Input.GetButtonDown("Jump") && _groundDetector.IsGrounded)
            _jumpInputBuffer = true;
    }

    private void UpdatePlayerAndCameraRotationFromMouseInput()
    {
        // 1) Rotates the player and the camera (It's a child Object) on the Y axis (X: ↑↓, Y: ←→, Z: ↻↺) using the mouse's X axis (X: ←→, Y: ↑↓) 
        // 2) Smooths the player rotation to avoid jittering for the Y axis
        float targetPlayerRotationY = transform.eulerAngles.y + Input.GetAxis("Mouse X") * LookSpeedX;
        if (_useSmoothing)
            targetPlayerRotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetPlayerRotationY, ref _playerRotationVelocityY, _smoothTimeY);
        transform.rotation = Quaternion.Euler(0, targetPlayerRotationY, 0);
        
        // 1) Rotates (locally) ONLY the camera on the X axis (X: ↑↓, Y: ←→, Z: ↻↺) using the mouse's Y axis (X: ←→, Y: ↑↓) 
        // 2) Clamps the rotation of the camera to the limits on the X axis.
        // 3) Smooths the camera rotation to avoid jittering for the X axis.
        float targetCameraRotationX = _cameraRotationX - Input.GetAxis("Mouse Y") * LookSpeedY;
        targetCameraRotationX = Mathf.Clamp(targetCameraRotationX, -_upperLookLimitAngle, _lowerLookLimitAngle);
        if (_useSmoothing)
            _cameraRotationX = Mathf.SmoothDampAngle(_cameraRotationX, targetCameraRotationX, ref _cameraRotationVelocityX, _smoothTimeX);
        else
            _cameraRotationX = targetCameraRotationX;
        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotationX, 0, 0);
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

    private void PrintDebuggingStatus()
    {
        Debug.Log($"Velocity Lenght: {_rigidbody.velocity.magnitude}");
        Debug.Log($"Player's Velocity: {_rigidbody.velocity}");
        Debug.Log($"Transformed Vector3.forward: {transform.TransformDirection(Vector3.forward)}");
        Debug.Log($"Transformed Vector3.right: {transform.TransformDirection(Vector3.right)}");
    }
    
}
