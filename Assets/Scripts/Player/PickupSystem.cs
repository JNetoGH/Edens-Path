using UnityEngine;


public class PickupSystem : MonoBehaviour
{
    
    public bool IsPickingUp { get; private set; } = false;  // Flag to track if picking up is in progress
    
    [SerializeField, Range(1, 10)] private float _pickupRange = 2.5f;  
    [SerializeField, Range(1, 10)] private float _objMoveSpeed = 6f;
    [SerializeField, Range(1, 5)] private float _objMaxDisFromCamera = 3.5f; // should be higher than the _pickupRange
    [SerializeField] private Transform _pickedUpPosition;
    [SerializeField] private Material _beingPickedMaterial;
    
    private Pickable _pickedObject; // Reference to the object being picked up
    private Vector3 _objLastVelocity = Vector3.zero; // used in order to keep the release force of the object.
    
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
    
    private void UpdateObjectPosition(bool useRigidbodyMovement)
    {
        // Get the Rigidbody component of the picked-up object, and sets it
        Rigidbody objRb = _pickedObject.GetComponent<Rigidbody>();
        objRb.velocity = Vector3.zero;
        objRb.maxAngularVelocity = 3;
        objRb.drag = 0;
        
        // checks if the object got stuck too far away from the camera and releases it if so
        Vector3 distanceFromCamera = transform.position - _pickedObject.transform.position;
        if (distanceFromCamera.magnitude > _objMaxDisFromCamera)
        {
            ReleaseObject();
            return;
        }

        // Calculate the target position where the object should be held.
        Vector3 targetPosition = _pickedUpPosition.position;
        
        // Interpolate the object's position towards the target position using Lerp (USING THE RIGIDBODY).
        // Even though it isn't moved with the rigidbody the velocity must be stored in order to keep the release force.
        // otherwise the object will just fall in a straight line when released.
        Vector3 newPosition = Vector3.Lerp(objRb.position, targetPosition, Time.fixedDeltaTime * _objMoveSpeed);
        _objLastVelocity = (newPosition - objRb.position) / Time.fixedDeltaTime;

        if (useRigidbodyMovement)
        {
            objRb.velocity = _objLastVelocity;
        }
        else
        {
            // Interpolate the object's position towards the target position using Lerp (USING TRANSFORM).
            newPosition = Vector3.Lerp(_pickedObject.transform.position, targetPosition, Time.deltaTime * _objMoveSpeed);
            _pickedObject.transform.position = newPosition;
        }
    }

    private void ReleaseObject()
    {
        if (IsPickingUp && _pickedObject != null)
        {
            // applies the release force on the released object
            _pickedObject.GetComponent<Rigidbody>().velocity = _objLastVelocity;
            // Restores the original material of the object
            _pickedObject.ResetToOriginalMaterial();
            // Resets the flag and reference to indicate that we are not picking up an object anymore
            IsPickingUp = false;
            // Updates the picked object internal state
            _pickedObject.IsBeingCarried = false;
            // Sets the current picked object one to be null
            _pickedObject = null;
        }
    }
}

