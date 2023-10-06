using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSubMenu : MonoBehaviour
{

    // Singleton Pattern
    public static CraftingSubMenu Instance { get; private set; }

    [Header("Singleton: Crafting Sub Menu References")]
    public GameObject craftHitbox;
    public GameObject craftHolderLeft;
    public GameObject craftHolderRight;
    public bool isLeftCraftHolderFree => craftHolderLeft.transform.childCount == 0;
    public bool isRightCraftHolderFree => craftHolderRight.transform.childCount == 0;
    public bool anyHolderAvaliable => (!CraftingSubMenu.Instance.isLeftCraftHolderFree || !CraftingSubMenu.Instance.isRightCraftHolderFree);

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        SendDockedItemsBackToInventory();
    }

    private void SendDockedItemsBackToInventory()
    {
        if (!isLeftCraftHolderFree)
            craftHolderLeft.transform.GetChild(0).SetParent(Inventory.Instance.inventoryContent.transform);
        if (!isRightCraftHolderFree)
            craftHolderRight.transform.GetChild(0).SetParent(Inventory.Instance.inventoryContent.transform);
    }

}
