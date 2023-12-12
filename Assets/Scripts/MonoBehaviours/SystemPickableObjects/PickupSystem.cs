using UnityEngine;


/// <summary>
/// Must be attached to the player's first person camera
/// </summary>
public class PickupSystem : MonoBehaviour
{
    
    public bool IsPickingUp { get; private set; } = false;  // Flag to track if picking up is in progress
    
    [Header("System Settings")]
    [SerializeField, Range(1, 10)] private float _pickupRange = 2.5f;  
    [SerializeField, Range(1, 15)] private float _objMaxDisFromCamera = 3.5f; // should be higher than the _pickupRange
    [SerializeField, Range(1, 10)] private float _objMoveSpeed = 6f;
    [SerializeField] private Transform _pickedUpPosition;
    
    [Header("Outline")]
    [SerializeField] private Color _outlineColor = Color.cyan;
    [SerializeField, Range(1, 40)] private float _outlineWidth = 10f;
    
    [Header("Being Picked Up Highlight")]
    [SerializeField] private Material _beingPickedMaterial;
    
    public PickableObject PickedObject => _pickedObject;
    private PickableObject _pickedObject; // Reference to the object being picked up
    private PickableObject _outlinedObject; // Reference to the previously highlighted object
    private Vector3 _objLastVelocity = Vector3.zero; // Used in order to keep the release force of the object.
    
    private void Update()
    {

        // Checking for player being able to move.
        if (!GameManager.CanMovePlayer)
            return;
        
        TryOutlinePickablesInRangeOfTheCameraRay();

        // Checks for Fire 2 button press, tries to add the outlined object to the inventory.
        if (Input.GetButtonUp("Fire2") && _outlinedObject != null)
            TrySendToPickedObjectToInventory();

        // Checks for Fire1 button press, Attempts to pick up an object.
        if (Input.GetButtonDown("Fire1"))
            TryPickupObject();
        
        // Checks for Fire1 button release: releases the currently held object
        // As long as the button is up it will be released.
        // GetButtonUp it can lead to glitches like, going into a menu stop pressing the button but because it
        // gets tru only on that frame, the release will get ignored, the objet will be stuck, and the player
        // will need to press and release the button again.
        if (!Input.GetButton("Fire1"))
            ReleaseCurrentObject(true);
        
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
    
    private void TrySendToPickedObjectToInventory()
    {
        Inventory inventory = FindObjectOfType<Inventory>(includeInactive: true);
        if (inventory == null)
            return;
        inventory.Add(_outlinedObject);
        Destroy(_outlinedObject.gameObject);
        IsPickingUp = false;
        _outlinedObject = null;
    }

    private void TryOutlinePickablesInRangeOfTheCameraRay()
    {
        // Checks for pickable objects in range
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hasHitSomething = Physics.Raycast(ray, out RaycastHit hit, _pickupRange);
        if (!hasHitSomething) // pointing to void
        {
            TryRemoveOutline(_outlinedObject);
            return;
        }

        // Checks if it's a Pickable and turn their outlines on
        PickableObject pickableObjectHit = hit.collider.gameObject.GetComponent<PickableObject>();
        bool hasHitPickable = pickableObjectHit != null;
        if (!hasHitPickable) // pointing to a non-pickable gameObj
        {
            TryRemoveOutline(_outlinedObject);
            return;
        }
        
        // If the currently highlighted object is not the same as the current object,
        // turn off the outline of the previously highlighted object
        if (_outlinedObject != pickableObjectHit)
        {
            if (_outlinedObject != null)
                _outlinedObject.IsBeingHitByPickUpRay = false;
            _outlinedObject = pickableObjectHit; // Updates the reference
            TryOutlinePickableObject(_outlinedObject);
        }
    }

    private void TryRemoveOutline(PickableObject outlined)
    {
        if (outlined is null) 
            return;
        outlined.IsBeingHitByPickUpRay = false;
        _outlinedObject = null; // Clear the reference
    }

    private void TryOutlinePickableObject(PickableObject outlined)
    {
        // checks if the object is valid and if the Pickable script is turned on
        if (outlined is null) 
            return;
        if (outlined.enabled == false)
            return;
        outlined.IsBeingHitByPickUpRay = true;
        outlined.OutlineScript.OutlineColor = _outlineColor;
        outlined.OutlineScript.OutlineWidth = _outlineWidth;
        outlined.OutlineScript.OutlineMode = Outline.Mode.OutlineVisible;
    }
    
    private void TryPickupObject()
    {
        // Creates a ray from the camera into the scene
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _pickupRange))
        {
            // Gets the GameObject that was hit and check if its a pickable object, and if the script is enabled
            // then changes its material to indicate that the object is being picked up, and set the control field.
            _pickedObject = hit.collider.gameObject.GetComponent<PickableObject>();
            if (_pickedObject == null)
                return;
            if (_pickedObject.enabled == false)
                return;
            _pickedObject.SetMaterial(_beingPickedMaterial);
            IsPickingUp = true;
            
            // Updates the picked object internal state
            _pickedObject.IsBeingCarried = true;
        }
    }
    
    private void UpdateObjectPosition(bool useRigidbodyMovement)
    {
        // Get the Rigidbody component of the picked-up object, and sets it velocity to zero
        // So it doesn't interferes with my custom systems/
        Rigidbody objRb = _pickedObject.GetComponent<Rigidbody>();
        objRb.velocity = Vector3.zero;
        
        // checks if the object got stuck too far away from the camera and releases it if so
        Vector3 distanceFromCamera = transform.position - _pickedObject.transform.position;
        if (distanceFromCamera.magnitude > _objMaxDisFromCamera)
        {
            ReleaseCurrentObject(true);
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

    public void ReleaseCurrentObject(bool useReleaseForce)
    {
        if (IsPickingUp && _pickedObject != null)
        {
            // applies the release force on the released object
            if (useReleaseForce) _pickedObject.GetComponent<Rigidbody>().velocity = _objLastVelocity;
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
    
    private void OnDrawGizmos()
    {
        // Draws a ray from the camera into the scene during editing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * _pickupRange);
    }
    
}