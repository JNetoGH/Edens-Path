using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    
    [Header("Items list")]
    public List<PickableObjectData> _pickableObjectsList = new List<PickableObjectData> ();
    
    [Header("Instancing References")]
    [SerializeField] private Transform _inventoryItemsParent;
    [SerializeField] private GameObject _inventoryItemPrefab;
    
    private void OnEnable()
    {
        GameManager.ReleaseTheCursor();
        PlayerController.CanMove = false;
        BuildInventoryItemsBasedOnList();
    }

    private void OnDisable()
    {
        GameManager.LockTheCursor();
        PlayerController.CanMove = true;
    }

    public void Add(Pickable pickableObject)
    {
        _pickableObjectsList.Add(pickableObject.pickableObjectData);
    }

    public void Remove(Pickable pickableObject)
    {
        _pickableObjectsList.Remove(pickableObject.pickableObjectData);
    }

    private void BuildInventoryItemsBasedOnList()
    {
        // Clears the InventoryItem already Instantiated, before rebuilding, so they don't get cloned.
        // Transform Component implements the IEnumerable interface, when in a foreach, it iterates over its children.
        foreach (Transform existingChild in _inventoryItemsParent)
            Destroy(existingChild.gameObject);
        
        // Builds the InventoryItems bases on the ScriptableObjects list.
        foreach (PickableObjectData data in _pickableObjectsList)
            FromDataToInventoryItem(data);
    }
    
    private void FromDataToInventoryItem(PickableObjectData data)
    {
        GameObject inventoryObj = Instantiate(_inventoryItemPrefab, _inventoryItemsParent);
        
        TextMeshProUGUI itemNameText = inventoryObj.GetComponentInChildren<TextMeshProUGUI>();
        Image itemImage = inventoryObj.transform.Find("ItemImage").GetComponentInChildren<Image>();
        itemNameText.text = data.itemName;
       
        if (data.icon is not null) 
            itemImage.sprite = data.icon;
    }
    
}
