using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    // Status
    public bool CanMove { get; private set; } = true;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _gravity = 30f;
    
    [Header("Camera")] 
    [SerializeField, Range(1, 10)] private float _lookSpeedX = 2f; 
    [SerializeField, Range(1, 10)] private float _lookSpeedY = 2f;
    [SerializeField, Range(1, 100), Tooltip(TipUpperLook)] private float _upperLookLimit = 80f; 
    [SerializeField, Range(1, 100), Tooltip(TipLowerLook)] private float _lowerLookLimit = 80f; 
    private const string TipUpperLook = "How many degrees the player can look up before the camera stops moving";
    private const string TipLowerLook = "How many degrees the player can look down before the camera stops moving";
    
    // Components
    private Camera _playerCamera;
    private GroundDetector _groundDetector;
    private CharacterController _characterController;
    private Rigidbody _rigidbody;

    // Others
    private Vector2 _inputs;
    private Vector3 _instantaneousVelocity;
    private float _rotationX;

    private void Awake()
    {
        // Setting the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerCamera = GetComponentInChildren<Camera>();
        _groundDetector = GetComponentInChildren<GroundDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMove) return;
        UpdateInputs();
        HandleMouseLock();
        Move();
    }

    private void UpdateInputs()
    {
        _inputs = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        // Normalizes the Dir Vector using clamp in order to not give up on the axis smoothening.
        _inputs = Vector2.ClampMagnitude(_inputs, 1f);
   
    }
    
    private void HandleMouseLock()
    {
        
    }

    private void Move()
    {
        // Applies the move speed to the inputs 
        _inputs = _inputs * _walkSpeed;
        
        // Applies gravity
        
        
        // Debug.Log(transform.TransformDirection(Vector3.forward));
        // Debug.Log(transform.TransformDirection(Vector3.right));
        
        _instantaneousVelocity = (transform.TransformDirection(Vector3.forward) * _inputs.x) + 
                                 (transform.TransformDirection(Vector3.right) * _inputs.y);
        
        _instantaneousVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = _instantaneousVelocity;
    }
}
