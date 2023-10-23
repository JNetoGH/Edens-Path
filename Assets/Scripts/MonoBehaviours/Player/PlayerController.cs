using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    
    public bool IsMoving => (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && GameManager.CanMovePlayer;
    public float VerticalVelocity { get; private set; } = 0;
    public const float GroundedDecrement = -0.5f;
    
    [Header("References")]
    [SerializeField] private Transform _camera;
    private CharacterController _characterController;
    
    [Header("Movement")]
    [SerializeField, Range(1, 10)] private float _forwardMoveSpeed = 3;
    [SerializeField, Range(1, 10)] private float _backwardMoveSpeed = 1.75f;
    private Vector3 ClampInput => Vector3.ClampMagnitude(new Vector3(_vInput, 0, _hInput), 1);
    private float _vInput;
    private float _hInput;
    
    [Header("Jump")]
    [SerializeField, Range(1, 100)] private float _jumpForce = 10f;
    private bool _jumpInput;
    private GroundDetector _groundDetector;
    
    [Header("Custom Gravity")]
    [SerializeField, Range(1, 100)] private float _gravityForce = 30f;
    [SerializeField, Range(0.1f, 3f), Tooltip(MultTip)] private float _gravityMultiplier = 1;
    [SerializeField, Range(-300, -1)] private float _terminalFallVelocity = -50f;
    private const string MultTip = "How stong gravity feels like (1 is good)";
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _groundDetector = GetComponentInChildren<GroundDetector>();
        if (_groundDetector == null)
            Debug.LogWarning("Player's GroundDetector.cs not found.");
    }

    private void Update()
    {
        UpdateInputs();
        RotatePlayerAccordingToCamera();
        // Calling the move in the update instead of fixed syncs better with the camera.
        if (GameManager.CanMovePlayer) 
            Move();
    }
    
    private void UpdateInputs()
    {
        // Movement
        _vInput = Input.GetAxis("Vertical");
        _hInput = Input.GetAxis("Horizontal");
        
        // Jump
        if (Input.GetButtonDown("Jump") && _groundDetector.IsGrounded)
            _jumpInput = true;
    }
    
    private void RotatePlayerAccordingToCamera()
    {
        transform.rotation = Quaternion.Euler(0, _camera.rotation.eulerAngles.y, 0);
    }
    
    private void Move()
    {
        // Moves the player towards the current rotation. Delta Time is required, because it's constant.
        Vector3 delta = Vector3.zero;
        delta = (transform.forward * ClampInput.x) + (transform.right * ClampInput.z);
        
        // The player move faster when going forward and slower backwards.
        // Checks if the player is moving forwards or backwards before appling the corresponding move speed.
        if (_vInput > 0)
            delta = delta * Time.deltaTime * _forwardMoveSpeed; 
        else
            delta = delta * Time.deltaTime * _backwardMoveSpeed;


        // Applies the jump force to the vertical velocity delta Time is not required, because it's an instantaneous force.
        if (_jumpInput) VerticalVelocity = _jumpForce;
            
        // Applies custom gravity, Delta Time is required because it's a constant force
        // Resets of vertical velocity when on the ground to prevent accumulation,
        // with is a non-0 value to prevent character controller failures like the ground detector not working properly.
        // this is important to ensure that the character doesn't start floating after landing.
        if (!_characterController.isGrounded)
        {
            // Calculates an acceleration corresponding to my custom gravity.
            VerticalVelocity -= _gravityMultiplier * _gravityForce * Time.deltaTime;
            
            // Checks for Terminal Velocity in Fall, and overrides it if required.
            if (VerticalVelocity < _terminalFallVelocity)
                VerticalVelocity = _terminalFallVelocity;
        }
        else if (!_jumpInput)
        {
            VerticalVelocity = GroundedDecrement;
        }

        // Resets the jump input.
        _jumpInput = false; 
        
        // Applies the vertical velocity to the movement. Delta Time is required because it's constant.
        delta.y += VerticalVelocity * Time.deltaTime;
        
        // Moves the character by applying the movement delta to it.
        _characterController.Move(delta);
    }
    
}
