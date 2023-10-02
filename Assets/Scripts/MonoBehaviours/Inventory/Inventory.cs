﻿using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    
    [FormerlySerializedAs("_pickableObjectsList")] [Header("Items list")]
    public List<PickableObjectData> pickableObjectsList = new List<PickableObjectData> ();
    
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

    public void Add(PickableObject pickableObjectObject)
    {
        pickableObjectsList.Add(pickableObjectObject.pickableObjectData);
    }
    
    public void Remove(PickableObjectData pickableObjectData)
    {
        pickableObjectsList.Remove(pickableObjectData);
    }
    
    public void BuildInventoryItemsBasedOnList()
    {
        // Clears the InventoryItem already Instantiated, before rebuilding, so they don't get cloned.
        // Transform Component implements the IEnumerable interface, when in a foreach, it iterates over its children.
        foreach (Transform existingChild in _inventoryItemsParent)
            Destroy(existingChild.gameObject);
        
        // Builds the InventoryItems bases on the ScriptableObjects list.
        for (int i = 0; i < pickableObjectsList.Count; i++)
        {
            PickableObjectData data = pickableObjectsList[i];
            FromDataToInventoryItem(data, i);
        }
    }
    
    private void FromDataToInventoryItem(PickableObjectData data, int indexInInventory)
    {
        // Creates a GameObject from the Inventory Item Prefab
        GameObject inventoryObj = Instantiate(_inventoryItemPrefab, _inventoryItemsParent);

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