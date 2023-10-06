using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(EventTrigger)), RequireComponent(typeof(Rigidbody2D))]
public class InventoryItemController: MonoBehaviour
{
    
    // All set by the inventory system at inventory build
    [HideInInspector] public PickableObjectData pickableObjectData;
    [HideInInspector] public int indexInInventory;
    
    private RectTransform _rectTransform;
    private bool _isMousePressed;
    private bool _hasBeenReleased;
    private bool _isOnHitbox = false;
    private bool _isinHolder = false;
    
    void Start()
    {
        _hasBeenReleased = false;
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_isMousePressed)
        {
            Vector2 mousePosition = Input.mousePosition;
            _rectTransform.position = mousePosition;
            if (!_isOnHitbox)
            {
                Debug.Log("On Free Space");
            }
        }
        
        // Case Dropped at free space: it's instantiated and removed from inventory.
        if (_hasBeenReleased && !_isOnHitbox)
        {
            InstantiateCorrespondingToPickableObjectData();
            RemoveFromInventory();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isMousePressed)
        {
            if (other.gameObject == Inventory.Instance.inventoryHitbox)
            {
                Debug.Log("On Inventory");
                _isOnHitbox = true;
            }
            else if (other.gameObject == CraftingSubMenu.Instance.craftHitbox)
            {
                Debug.Log("On Crafting");
                _isOnHitbox = true;
            }
        }
        
        if (_hasBeenReleased)
        {
             // Released on top of the inventory content
            if (other.gameObject == Inventory.Instance.inventoryHitbox)
                _rectTransform.SetParent(Inventory.Instance.inventoryContent.transform);
            
            // Released on top of a vague craftHolder
            else if (other.gameObject == CraftingSubMenu.Instance.craftHitbox)
            {
                
                // In case both holders are occupied, sends back to inventory.
                bool anyHolderVague = CraftingSubMenu.Instance.anyHolderAvaliable;
                if (!_isinHolder && anyHolderVague)
                {
                    _rectTransform.SetParent(Inventory.Instance.inventoryContent.transform);
                }
                
                // Case there is one vague.
                if (CraftingSubMenu.Instance.isLeftCraftHolderFree && !_isinHolder)
                {
                   _rectTransform.SetParent(CraftingSubMenu.Instance.craftHolderLeft.transform);
                    _isinHolder = true;
                }
                else if (CraftingSubMenu.Instance.isRightCraftHolderFree && !_isinHolder)
                {
                    _rectTransform.SetParent(CraftingSubMenu.Instance.craftHolderRight.transform);
                    _isinHolder = true;
                }
                
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _isOnHitbox = false;
    }

    // Called by the event trigger
    public void OnPointerDown()
    {
        _isMousePressed = true;
        _hasBeenReleased = false;
        _isinHolder = false;
        _rectTransform.SetParent(FindObjectOfType<Inventory>().GetComponent<RectTransform>());
        _rectTransform.SetAsLastSibling();
    }

    // Called by the event trigger
    public void OnPointerUp()
    {
        _isMousePressed = false;
        _hasBeenReleased = true;
    }
    
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

    private void RemoveFromInventory()
    {
        // Removes itself from the inventory list
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.pickableObjectsList.RemoveAt(indexInInventory);
        
        // Rebuilds the inventory
        inventory.BuildInventoryItemsBasedOnList();
        
        // Destroys Itself
        Destroy(this.gameObject);
    }

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
