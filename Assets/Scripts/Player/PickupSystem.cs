using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    
    [SerializeField, Range(1, 10)] private float _pickupRange = 2.5f;  
    [SerializeField] private Material _beingPickedMaterial;
    [SerializeField] private Transform _pickedUpPosition;
    
    public bool IsPickingUp { get; private set; } = false;  // Flag to track if picking up is in progress
    private Pickable _pickedObject; // Reference to the object being picked up
    
    private void Update()
    {
        // Checks for Fire1 button press, Attempts to pick up an object
        if (Input.GetButtonDown("Fire1"))
            TryPickupObject();
        
        // Checks for Fire1 button release: releases the currently held object
        if (Input.GetButtonUp("Fire1"))
            ReleaseObject();
        
        // If an object is being picked up, update its position
        if (IsPickingUp && _pickedObject != null)
            UpdateObjectPosition();
        
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

    private void UpdateObjectPosition()
    {
        // Reset the object's velocity to zero (prevents it from having any physics-based movement)
        Rigidbody rb = _pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = Vector3.zero;
        _pickedObject.transform.position = _pickedUpPosition.position;
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

