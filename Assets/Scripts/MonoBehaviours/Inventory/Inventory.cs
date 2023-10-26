using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages the inventory functionality, including adding, removing and displaying items.
/// </summary>
public class Inventory : MonoBehaviour
{
    
    // Singleton Pattern
    public static Inventory Instance { get; private set; }
    
    [Header("Required References (may be passed via Singleton)"), HorizontalLine] 
    [BoxGroup, Required, SerializeField] private Transform _inventoryItemsParent;
    [BoxGroup, Required] public GameObject inventoryHitbox;
    [BoxGroup, Required] public GameObject inventoryContent;
    [BoxGroup, Required] public GameObject inventoryItemPrefab;
    
    [Header("Items list")]
    [SerializeField] private List<PickableObjectData> _pickableObjectsList = new List<PickableObjectData> ();
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        BuildInventoryItemsBasedOnList();
    }
    
    /// <summary>
    /// Adds a pickable object to the inventory (PickableObject.cs).
    /// </summary>
    /// <param name="pickableObjectObject">The pickable object to be added.</param>
    public void Add(PickableObject pickableObjectObject)
    {
        _pickableObjectsList.Add(pickableObjectObject.pickableObjectData);
    }
    
    /// <summary>
    /// Adds a pickable object to the inventory using only its data (ScriptableObject).
    /// </summary>
    /// <param name="pickableObjectData">The data (ScriptableObject) of the pickable object to be added.</param>
    public void Add(PickableObjectData pickableObjectData)
    {
        _pickableObjectsList.Add(pickableObjectData);
    }
    
    /// <summary>
    /// Removes a pickable object from the inventory based on its data (ScriptableObject).
    /// </summary>
    /// <param name="pickableObjectData">The data (ScriptableObject) of the pickable object to be removed.</param>
    public void Remove(PickableObjectData pickableObjectData)
    {
        _pickableObjectsList.Remove(pickableObjectData);
    }
 
    /// <summary>
    /// Builds the inventory items based on the list of pickable objects.
    /// </summary>
    public void BuildInventoryItemsBasedOnList()
    {
        // Clears the InventoryItem already Instantiated, before rebuilding, so they don't get cloned.
        // Transform Component implements the IEnumerable interface, when in a foreach, it iterates over its children.
        foreach (Transform existingChild in _inventoryItemsParent)
            Destroy(existingChild.gameObject);
        
        // Builds the InventoryItems bases on the ScriptableObjects list.
        for (int i = 0; i < _pickableObjectsList.Count; i++)
        {
            PickableObjectData data = _pickableObjectsList[i];
            FromDataToInventoryItem(data, i);
        }
    }
    
    /// <summary>
    /// Converts a pickable object data (ScriptableObject) into an InventoryItem prefab and instantiates it.
    /// Its parent will be the _inventoryItemsParent which has a layout group.
    /// </summary>
    /// <param name="data">The data of the pickable object.</param>
    /// <param name="indexInInventory">The index of the item in the inventory.</param>
    private void FromDataToInventoryItem(PickableObjectData data, int indexInInventory)
    {
        // Creates a GameObject from the Inventory Item Prefab
        GameObject inventoryObj = Instantiate(inventoryItemPrefab, _inventoryItemsParent);

        // Sets its logical information via the InventoryItemController script
        InventoryItemController inventoryItemController = inventoryObj.GetComponent<InventoryItemController>();
        inventoryItemController.pickableObjectData = data;
        inventoryItemController.indexInInventory = indexInInventory;
        
        // Sets its style according to the PickableObjectData provided
        TextMeshProUGUI itemNameText = inventoryObj.GetComponentInChildren<TextMeshProUGUI>();
        Image itemImage = inventoryObj.transform.Find("ItemImage").GetComponentInChildren<Image>();
        itemNameText.text = data.itemName;
        if (data.icon is not null) 
            itemImage.sprite = data.icon;
    }
    
}
