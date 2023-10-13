using UnityEngine;


public class ThrowSystem : MonoBehaviour
{
    
    [SerializeField] private Transform _pickupPosition;
    [SerializeField, Range(2f, 10f)] private float _throwForceIncrement = 2f;
    [SerializeField, Range(0.5f, 20f)] private float _maxThrowForce = 10f;
    
    private float _throwForce = 0;
    private GameObject _curObject = null;
    private PickupSystem _pickupSystem = null;
    private bool _hasCancelled = false;
    
    private void Start()
    {
        _pickupSystem = FindObjectOfType<PickupSystem>();
        if (_pickupSystem == null) 
            Debug.LogWarning("The Throw System could not find the Pickup System");
        if (_pickupPosition == null)
            Debug.LogWarning("The Throw System could not find the Pickup Position Transform");
    }

    private void Update()
    {
        if (!CheckGateWayConditions()) 
            return;

        // Temporary until I get a UI representation
        Debug.Log($"throw force: {_throwForce}");
        
        // When the player cancels a throw:
        // - He needs to press the throw key again to start to accumulate throw force once more.
        // - Otherwise it would simply restart accumulating throw force back from 0, without any break.
        if (_hasCancelled)
        {
           bool hasPressedThrowKeyAgain = Input.GetKeyDown(KeyCode.E);
           _hasCancelled = !hasPressedThrowKeyAgain;
        }
        if (_hasCancelled)
            return;
        
        // Throw or cancel input Management
        bool throwForceIncreaseInput = Input.GetKey(KeyCode.E);
        bool throwInput = Input.GetKeyUp(KeyCode.E);
        bool cancelThrowInput = Input.GetKeyDown(KeyCode.Q);

        if (cancelThrowInput)
        {
            _hasCancelled = true;
            _throwForce = 0;
            return;
        }
        if (throwForceIncreaseInput)
        {
            _throwForce += _throwForceIncrement * Time.deltaTime;
            _throwForce = Mathf.Min(_throwForce, _maxThrowForce);
        }
        if (throwInput)
        {
            ThrowCurrentPickableObject();
            _throwForce = 0;
        }
    }
    
    private void ThrowCurrentPickableObject()
    {
        _curObject = _pickupSystem.PickedObject.gameObject;
        Rigidbody curObjRb = _curObject.GetComponent<Rigidbody>();
        _pickupSystem.ReleaseCurrentObject(false);
        float throwForce = _throwForce;
        curObjRb.velocity = _pickupPosition.forward * throwForce;
    }
    
    private bool CheckGateWayConditions()
    {
        if (_pickupSystem == null || _pickupPosition == null)
        {
            return false;
            _throwForce = 0;
        }

        if (_pickupSystem.PickedObject == null)
        {
            return false;
            _throwForce = 0;
        }

        return true;
    }
    
}
