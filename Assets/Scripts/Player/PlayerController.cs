using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private Transform _camera;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 3;
    private float _vInput;
    private float _hInput;
    private Vector3 NormInput => Vector3.ClampMagnitude(new Vector3(_vInput, 0, _hInput), 1);
    
    [Header("Jump")]
    [SerializeField, Range(1, 100)] private float _jumpForce = 10f;
    private bool _jumpInput;
    
    [Header("Custom Gravity")]
    [SerializeField, Range(1, 100)] private float _gravityForce = 30f;
    [SerializeField, Range(0.1f, 3f), Tooltip(MultTip)] private float _gravityMultiplier = 1;
    private const string MultTip = "How stong gravity feels like (1 is good)";
    private float _verticalVelocity; 
    
    // Components
    private CharacterController _characterController;
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        UpdateInputs();
        RotatePlayerAccordingToCamera();
        Move();
    }
    
    private void UpdateInputs()
    {
        // Movement
        _vInput = Input.GetAxis("Vertical");
        _hInput = Input.GetAxis("Horizontal");
        
        // Jump
        if (Input.GetButtonDown("Jump") && _characterController.isGrounded)
            _jumpInput = true;
    }
    
    private void RotatePlayerAccordingToCamera()
    {
        transform.rotation = Quaternion.Euler(0, _camera.rotation.eulerAngles.y, 0);
    }
    
    private void Move()
    {
        // Moves the player towards the current rotation. Delta Time is required, because its a constant force.
        Vector3 delta = (transform.forward * NormInput.x) + (transform.right * NormInput.z);
        delta = delta * _moveSpeed * Time.deltaTime; 
        
        // Applies the jump force to the vertical velocity delta Time is not required, because its a instantaneous force.
        if (_jumpInput) _verticalVelocity = _jumpForce;
            
        // Applies custom gravity, Delta Time is required because its a constant force
        // Reset of vertical velocity when on the ground to prevent accumulation,
        // this is important to ensure that the character doesn't start floating after landing.
        if (!_characterController.isGrounded) _verticalVelocity -= _gravityMultiplier * _gravityForce * Time.deltaTime;
        else if (!_jumpInput) _verticalVelocity = -0.5f; 
        
        // Resets the jump input.
        _jumpInput = false; 
        
        // Applies the vertical velocity to the movement. Delta Time is required because its a constant force.
        delta.y += _verticalVelocity * Time.deltaTime;
        
        // Moves the character
        _characterController.Move(delta);
    }
    
}
