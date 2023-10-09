using ScriptableObjects;
using UnityEngine;


public class CombiningSubMenu : MonoBehaviour
{

    // Singleton Pattern
    public static CombiningSubMenu Instance { get; private set; }

    [Header("Singleton: Combining Submenu References")]
    public GameObject combiningSubmenuHitbox;
    public GameObject itemHolderLeft;
    public GameObject itemHolderRight;
    public bool isLeftItemHolderFree => itemHolderLeft.transform.childCount == 0;
    public bool isRightItemHolderFree => itemHolderRight.transform.childCount == 0;
    public bool anyItemHolderAvaliable => (!Instance.isLeftItemHolderFree || !Instance.isRightItemHolderFree);

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        if (!isLeftItemHolderFree)  SendDockedItemsBackToInventory(itemHolderLeft.transform.GetChild(0).GetComponent<InventoryItemController>());
        if (!isRightItemHolderFree) SendDockedItemsBackToInventory(itemHolderRight.transform.GetChild(0).GetComponent<InventoryItemController>());
    }

    private void SendDockedItemsBackToInventory(InventoryItemController dockedItem)
    {
        Inventory.Instance.Add(dockedItem.pickableObjectData);
        dockedItem.transform.SetParent(Inventory.Instance.inventoryContent.transform);
        dockedItem.isChildOfInventoryParent = true;
    }

}
