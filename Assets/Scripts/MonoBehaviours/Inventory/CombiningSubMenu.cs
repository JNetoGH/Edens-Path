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
        SendDockedItemsBackToInventory();
    }

    private void SendDockedItemsBackToInventory()
    {
        if (!isLeftItemHolderFree)
            itemHolderLeft.transform.GetChild(0).SetParent(Inventory.Instance.inventoryContent.transform);
        if (!isRightItemHolderFree)
            itemHolderRight.transform.GetChild(0).SetParent(Inventory.Instance.inventoryContent.transform);
    }

}
