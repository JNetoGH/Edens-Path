using System;
using UnityEngine;


public class PickupSystem : MonoBehaviour
{
    
    [SerializeField, Range(1, 10)] private float _pickupRange = 2.5f;  
    [SerializeField] private float _pickedUpObjMoveSpeed = 120;
    [SerializeField] private Material _beingPickedMaterial;
    [SerializeField] private Transform _pickedUpPosition;
    public bool IsPickingUp { get; private set; } = false;  // Flag to track if picking up is in progress
    private Pickable _pickedObject; // Reference to the object being picked up
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // Checks for Fire1 button press, Attempts to pick up an object
        if (Input.GetButtonDown("Fire1"))
            TryPickupObject();
        
        // Checks for Fire1 button release: releases the currently held object
        if (Input.GetButtonUp("Fire1"))
            ReleaseObject();
    }

    private void FixedUpdate()
    {
        // If an object is being picked up, update its position
        // Uses the physics accurate movement
        if (IsPickingUp && _pickedObject != null)
            if (_pickedObject.IsColliding)
                UpdateObjectPosition(true);
    }

    private void LateUpdate()
    {
        // If an object is being picked up, update its position
        // Uses the PRETTIER NON-physics accurate movement
        if (IsPickingUp && _pickedObject != null)
            if (!_pickedObject.IsColliding)
                UpdateObjectPosition(false);
    }

    private void TryPickupObject()
    {
        // Creates a ray from the camera into the scene
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Checks if the ray hits an object in the pickup layer
        if (Physics.Raycast(ray, out hit, _pickupRange))
        {
            // Gets the GameObject that was hit and check if its a pickable object
            // then changes its material to indicate that the object is being picked up, and set the control field.
            _pickedObject = hit.collider.gameObject.GetComponent<Pickable>();
            if (_pickedObject == null)
                return;
            _pickedObject.SetMaterial(_beingPickedMaterial);
            IsPickingUp = true;
            
            // Updates the picked object internal state
            _pickedObject.IsBeingCarried = true;
        }
    }

    /// <summary>
    /// Update the position of the currently held object to move it towards a target position.
    /// </summary>
    private void UpdateObjectPosition(bool usedRigidBodyMovement)
    {
        // Get the Rigidbody component of the picked-up object
        Rigidbody objRb = _pickedObject.GetComponent<Rigidbody>();
        objRb.velocity = Vector3.zero;
        objRb.maxAngularVelocity = 3;
        objRb.drag = 0;
        objRb.angularDrag = 0;
        
        // Calculate the target position
        Vector3 targetPosition = _pickedUpPosition.position;

        // Interpolate the object's position towards the target position
        if (usedRigidBodyMovement)
            objRb.MovePosition(Vector3.Lerp(objRb.position, targetPosition, Time.deltaTime * _pickedUpObjMoveSpeed));
        else
            _pickedObject.transform.position = Vector3.Lerp(_pickedObject.transform.position, targetPosition, Time.deltaTime * _pickedUpObjMoveSpeed);
    }

    private void ReleaseObject()
    {
        if (IsPickingUp && _pickedObject != null)
        {
            // Restores the original material of the object
            _pickedObject.ResetToOriginalMaterial();

            // Resets the flag and reference to indicate that we are not picking up an object anymore
            IsPickingUp = false;
            // Updates the picked object internal state
            _pickedObject.IsBeingCarried = false;
            // sets the current picked object one to be null
            _pickedObject = null;
        }
    }
}

