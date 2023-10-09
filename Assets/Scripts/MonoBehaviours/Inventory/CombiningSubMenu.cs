using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;


public class CombiningSubMenu : MonoBehaviour
{

    // Singleton Pattern
    public static CombiningSubMenu Instance { get; private set; }

    public List<CombiningRecipeData> recipes = new List<CombiningRecipeData>(); 

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

    // Called by the button on the Combining submenu
    public void TryCombine()
    {

        if (isLeftItemHolderFree || isRightItemHolderFree)
        {
            Debug.Log("Combining Failed: one or more item holders are empty");
            return;
        }

        PickableObjectData obj1 = itemHolderLeft.transform.GetChild(0).GetComponent<InventoryItemController>().pickableObjectData;
        PickableObjectData obj2 = itemHolderRight.transform.GetChild(0).GetComponent<InventoryItemController>().pickableObjectData;

        bool foundARecipe = false;

        foreach (CombiningRecipeData recipe in recipes)
        {
            
            if ((obj1 == recipe.obj1 && obj2 == recipe.obj2) || (obj1 == recipe.obj2 && obj2 == recipe.obj1))
            {
                Debug.Log("There is a valid recipe, Result: " + recipe.result?.name);
                foundARecipe = true;
                Inventory.Instance.Add(recipe.result);
                Destroy(itemHolderLeft.transform.GetChild(0).gameObject);
                Destroy(itemHolderRight.transform.GetChild(0).gameObject);
                Inventory.Instance.BuildInventoryItemsBasedOnList();
                return;
            }
        }

        if (!foundARecipe)
        {
            Debug.Log("Combining Failed: No Recipes found for the current combination");
        }
    }

    private void SendDockedItemsBackToInventory(InventoryItemController dockedItem)
    {
        Inventory.Instance.Add(dockedItem.pickableObjectData);
        dockedItem.transform.SetParent(Inventory.Instance.inventoryContent.transform);
        dockedItem.isChildOfInventoryParent = true;
    }

}
