using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Represents a controller for inventory items, managing their behavior and interactions.
/// </summary>
[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(EventTrigger)), RequireComponent(typeof(Rigidbody2D))]
public class InventoryItemController: MonoBehaviour
{
    
    // All set by the inventory system at inventory build
    [HideInInspector] public PickableObjectData pickableObjectData;
    [HideInInspector] public int indexInInventory;
    
    // Controlling
    [HideInInspector] public bool isChildOfInventoryParent; // Changed by the CombiningSubmenu.cs
    private bool _isMousePressed = false;
    private bool _hasBeenReleased = false;
    private bool _isOnHitbox = false;
    private bool _isInHolder = false;
    
    private RectTransform _rectTransform;
    
    void Start()
    {
        if (transform.parent.tag.Equals("InventoryContent"))
            isChildOfInventoryParent = true;
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Debug.Log($"{gameObject.name} is in a Holder: {_isInHolder}");
        // Debug.Log($"{gameObject.name} has been Released: {_hasBeenReleased}");
        
        if (_isMousePressed)
            MoveToMousePosition();

        // Case Dropped at free space: it's instantiated and removed from inventory.
        if (_hasBeenReleased && !_isOnHitbox && !_isInHolder)
        {
            InstantiateCorrespondingToPickableObjectData();
            DestroyInventoryItem();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // Checks for a Hitbox overlaps.
        if (_isMousePressed)
        {
            if (other.gameObject == Inventory.Instance.inventoryHitbox)
            {
                // Debug.Log($"{gameObject.name} dragged on Inventory");
                _isOnHitbox = true;
            }
            else if (other.gameObject == CombiningSubmenu.Instance.combiningSubmenuHitbox)
            {
                // Debug.Log($"{gameObject.name} dragged on Combining submenu");
                _isOnHitbox = true;
            }
        }
        
        if (_hasBeenReleased)
        {
            ManageRelease(other);
            _hasBeenReleased = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _isOnHitbox = false;
    }
    
    /// <summary>
    /// Called when the pointer is pressed on the inventory item via Event Trigger Component.
    /// </summary>
    public void OnPointerDown()
    {
        if (isChildOfInventoryParent) Inventory.Instance.Remove(pickableObjectData); // removes from the data list
        _isMousePressed = true;
        _hasBeenReleased = false;
        _isInHolder = false;
        _rectTransform.SetParent(FindObjectOfType<Inventory>().GetComponent<RectTransform>());
        _rectTransform.SetAsLastSibling();
    }

    /// <summary>
    /// Called when the pointer is released from the inventory item via Event Trigger Component.
    /// </summary>
    public void OnPointerUp()
    {
        _isMousePressed = false;
        _hasBeenReleased = true;
    }
    
    /// <summary>
    /// Manages the release behavior of the inventory item based on the specified collider.
    /// </summary>
    /// <param name="other">The collider with which the inventory item interacts.</param>
    private void ManageRelease(Collider2D other)
    {
        // Released on top of the inventory content hitbox.
        if (other.gameObject == Inventory.Instance.inventoryHitbox)
        {
            _rectTransform.SetParent(Inventory.Instance.inventoryContent.transform);
            Debug.Log($"{gameObject.name} has been released on the inventory");
            // Adds the data back to inventory list
            Inventory.Instance.Add(pickableObjectData);
            if (transform.parent.tag.Equals("InventoryContent"))
                isChildOfInventoryParent = true;
        }

        // Released on top of the combining submenu hitbox.
        else if (!_isInHolder && other.gameObject == CombiningSubmenu.Instance.combiningSubmenuHitbox)
        {
            // Case there is one vague.
            if (CombiningSubmenu.Instance.IsLeftItemHolderFree)
            {
                Debug.Log($"{gameObject.name} has been released on left item holder");
                _rectTransform.SetParent(CombiningSubmenu.Instance.itemHolderLeft.transform);
                _isInHolder = true;
                isChildOfInventoryParent = false;
            }
            else if (CombiningSubmenu.Instance.IsRightItemHolderFree)
            {
                Debug.Log($"{gameObject.name} has been released on right item holder");
                _rectTransform.SetParent(CombiningSubmenu.Instance.itemHolderRight.transform);
                _isInHolder = true;
                isChildOfInventoryParent = false;
            }

            // In case both item holders are occupied, sends back to inventory.
            if (!_isInHolder)
            {
                Debug.Log(
                    $"{gameObject.name} has been released on a item holder but both are occupied, it has been sent back to inventory");
                _rectTransform.SetParent(Inventory.Instance.inventoryContent.transform);
                // Adds the data back to inventory list
                Inventory.Instance.Add(pickableObjectData);
                if (transform.parent.tag.Equals("InventoryContent"))
                    isChildOfInventoryParent = true;
            }
        }
    }
    
    /// <summary>
    /// Instantiates a corresponding PickableObject based on the inventory item's data.
    /// </summary>
    private void InstantiateCorrespondingToPickableObjectData()
    {
        if (CheckForErrors()) 
            return;
        
        // Tries to find and set an Instantiation point using the PickableObjectInstantiationPoint taf
        GameObject instantiationPoint = GameObject.FindWithTag("PickableObjectInstantiationPoint");
        if (instantiationPoint == null)
        {
            const string msg = "Tried to instantiate a PickableObject from an InventoryItem " +
                               "but the PickableObjectInstantiationPoint tag couldn't be found";
            Debug.LogWarning(msg);
            return;
        }
        
        // Instantiates a PickableObject based o the corresponding prefab from the data and teleports it to target
        GameObject obj = Instantiate(pickableObjectData.correspondingPickablePrefab);
        obj.transform.position = instantiationPoint.transform.position;
    }

    /// <summary>
    /// Destroys the inventory item and rebuilds the inventory.
    /// </summary>
    private void DestroyInventoryItem()
    {
        // Destroys Itself
        Destroy(this.gameObject);

        // Rebuilds the inventory
        Inventory.Instance.BuildInventoryItemsBasedOnList();
    }

    /// <summary>
    /// Moves the inventory item to the current mouse position.
    /// </summary>
    private void MoveToMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        _rectTransform.position = mousePosition;
        // if (!_isOnHitbox) Debug.Log($"{gameObject.name} dragged on Free Space");
    }

    /// <summary>
    /// Checks for errors in the inventory item data.
    /// </summary>
    /// <returns>True if any error is found, false otherwise.</returns>
    private bool CheckForErrors()
    {
        if (pickableObjectData is null)
        {
            const string msg = "Tried to instantiate a PickableObject from an InventoryItem " +
                               "but the ScriptableObject data field was not set";
            Debug.LogWarning(msg);
            return true;
        }

        if (pickableObjectData.correspondingPickablePrefab is null)
        {
            const string msg = "Tried to instantiate a PickableObject from an InventoryItem " +
                               "but the Corresponding Prefab in the ScriptableObject data is missing";
            Debug.LogWarning(msg);
            return true;
        }

        return false;
    }
}
