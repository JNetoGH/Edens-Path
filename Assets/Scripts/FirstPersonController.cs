using UnityEngine;
using UnityEngine.Serialization;

public class FirstPersonController : MonoBehaviour
{
    // Status
    public bool CanMove { get; private set; } = true;

    [FormerlySerializedAs("_printDebugStatus")]
    [Header("General")]
    [SerializeField] private bool _printDebuggingStatus;
    
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _customGravity = 0.5f;
    [SerializeField] private float _customGravityThreshold = 100f;
    
    [Header("Camera")] 
    [SerializeField, Range(1, 10)] private float _lookSpeedX = 2f; 
    [SerializeField, Range(1, 10)] private float _lookSpeedY = 2f;
    [SerializeField, Range(1, 100), Tooltip(TipUpperLook)] private float _upperLookLimitAngle = 80f; 
    [SerializeField, Range(1, 100), Tooltip(TipLowerLook)] private float _lowerLookLimitAngle = 80f; 
    private const string TipUpperLook = "How many degrees the player can look up before the camera stops moving";
    private const string TipLowerLook = "How many degrees the player can look down before the camera stops moving";
    
    // Components
    private Camera _playerCamera;
    private GroundDetector _groundDetector;
    private Rigidbody _rigidbody;

    // Others
    private Vector2 _moveInputs;
    private bool _jumpInputBuffer;
    private float _cameraRotationX;

    private void Awake()
    {
        // Setting the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerCamera = GetComponentInChildren<Camera>();
        _groundDetector = GetComponentInChildren<GroundDetector>();
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
        ApplyCustomGravity();
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
        // The mouse's Y axis (X: ← →, Y: ↑ ↓) rotates the camera in the X axis (X: ↑ ↓ "front", Y: ← →, Z: ↻ ↺ "lateral")
        _cameraRotationX -= Input.GetAxis("Mouse Y") * _lookSpeedY;
        // Clamps the rotation of the camera to the limits
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -_upperLookLimitAngle, _lowerLookLimitAngle);
        // Rotates the camera in its local coordinates
        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotationX, 0, 0);
        
        // Rotates the player, it doesn't need to be clamped because the player can freely rotate
        // in 360° around the Y axis, unlike the X axis where the player's neck would break.
        // The mouse's X axis (X: ← →, Y: ↑ ↓) rotates the player in the Y axis (X: ↑ ↓ "front", Y: ← →, Z: ↻ ↺ "lateral")
        // which will also rotate the camera because its a child object
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeedX, 0);
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
        // Applies the move speed to the inputs 
        _moveInputs = _moveInputs * _walkSpeed;
        Vector3 auxVelocityHolder = (transform.TransformDirection(Vector3.forward) * _moveInputs.x) + 
                                    (transform.TransformDirection(Vector3.right) * _moveInputs.y);
        
        // Conservatives the Y velocity before applying in order to do not override the gravity
        auxVelocityHolder.y = _rigidbody.velocity.y;
        _rigidbody.velocity = auxVelocityHolder;
        
        // Verifique se o personagem está no ar e tocando em uma parede e para o movimento horizontal.
        if (!_groundDetector.IsGrounded && IsTouchingWall())
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
    }
    
    private bool IsTouchingWall()
    {
        float rayLength = 2f; // Ajuste o comprimento do raio conforme necessário.
        Vector3 rayDirection = transform.forward; // Ajuste a direção conforme necessário (pode ser transform.right para paredes laterais).

        RaycastHit hit;
        if (Physics.Raycast(transform.position, rayDirection, out hit, rayLength))
        {
            // Verifique se a colisão é com uma camada de parede específica.
            if (hit.collider.CompareTag("Wall"))
                return true;
        }
        return false;
    }
    
    private void PrintDebuggingStatus()
    {
        Debug.Log($"player's velocity: {_rigidbody.velocity}");
        Debug.Log($"Transformed Vector3.forward: {transform.TransformDirection(Vector3.forward)}");
        Debug.Log($"Transformed Vector3.right: {transform.TransformDirection(Vector3.right)}");
    }

}
