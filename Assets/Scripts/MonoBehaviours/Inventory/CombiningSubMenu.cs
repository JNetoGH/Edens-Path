using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;


public class CombiningSubMenu : MonoBehaviour
{

    // Singleton Pattern
    public static CombiningSubMenu Instance { get; private set; }

    [FormerlySerializedAs("recipesData")] [FormerlySerializedAs("recipes")] public List<CombiningRecipeData> combiningRecipes = new List<CombiningRecipeData>(); 

    [Header("Singleton: Combining Submenu References")]
    public GameObject combiningSubmenuHitbox;
    public GameObject itemHolderLeft;
    public GameObject itemHolderRight;
    public GameObject resultPreview;
    public bool isLeftItemHolderFree => itemHolderLeft.transform.childCount == 0;
    public bool isRightItemHolderFree => itemHolderRight.transform.childCount == 0;
    public bool anyItemHolderAvaliable => (Instance.isLeftItemHolderFree || Instance.isRightItemHolderFree);
    public bool isResultPreviewFree => resultPreview.transform.childCount == 0;

    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
        bool areBothHolderOccupied = !isRightItemHolderFree && !isLeftItemHolderFree;
        if (!areBothHolderOccupied)
        {
            // Clears previous preview item in case any of the holders has been changed.
            if (!isResultPreviewFree)
            {
                GameObject objPrev = resultPreview.transform.GetChild(0).gameObject;
                Destroy(objPrev);
            }
            return;
        }
        
        // can't child another obj to preview if there is already one, they will stack.
        if (!isResultPreviewFree)
            return;
        
        InventoryItemController item1 = itemHolderLeft.transform.GetChild(0).GetComponent<InventoryItemController>();
        InventoryItemController item2 = itemHolderRight.transform.GetChild(0).GetComponent<InventoryItemController>();
        CombiningRecipeData recipe = FindFirstRecipeCorrespondingToItems(item1.pickableObjectData, item2.pickableObjectData);
        bool foundARecipe = recipe != null;
        if (!foundARecipe || item1 == null || item2 == null)
            return;
        
        // Creates a GameObject from the Inventory Item Prefab to serve as preview and sets as child to the preview.
        GameObject previewObj = Instantiate(Inventory.Instance.inventoryItemPrefab);
        previewObj.transform.SetParent(resultPreview.transform);

        // Sets its logical information via the InventoryItemController script
        InventoryItemController previewScript = previewObj.GetComponent<InventoryItemController>();
        previewScript.pickableObjectData = recipe.result;
        
        // Sets its style according to the PickableObjectData provided
        TextMeshProUGUI itemNameText = previewObj.GetComponentInChildren<TextMeshProUGUI>();
        Image itemImage = previewObj.transform.Find("ItemImage").GetComponentInChildren<Image>();
        
        // Gives a little black-ish accent to the image of the preview
        itemImage.color = Color.gray;
        
        itemNameText.text = recipe.result.itemName;
        if (recipe.result.icon is not null) 
            itemImage.sprite = recipe.result.icon;
        
        // Sets it to be non interactable,
        // so it can't be attached to the inventory as a regular Inventory Item, or dragged around.
        previewObj.GetComponent<EventTrigger>().enabled = false;
        previewObj.GetComponent<BoxCollider2D>().enabled = false;
        previewObj.GetComponent<Button>().enabled = false;
        previewScript.enabled = false;
    }

    private void OnDisable()
    {
        // Sends both item back to inventory when the combining submenu is disabled.
        if (!isLeftItemHolderFree)  SendDockedItemsBackToInventory(itemHolderLeft.transform.GetChild(0).GetComponent<InventoryItemController>());
        if (!isRightItemHolderFree) SendDockedItemsBackToInventory(itemHolderRight.transform.GetChild(0).GetComponent<InventoryItemController>());
    }

    // Called by the button on the Combining submenu.
    public void TryCombine()
    {
        // Checks before if both holder have a Inventory Item as child.
        if (anyItemHolderAvaliable)
        {
            Debug.Log("Combining Failed: one or more item holders are empty");
            return;
        }

        // Each holder can only be assigned with 1 Inventory Item, so it safe to get them like this.
        InventoryItemController item1 = itemHolderLeft.transform.GetChild(0).GetComponent<InventoryItemController>();
        InventoryItemController item2 = itemHolderRight.transform.GetChild(0).GetComponent<InventoryItemController>();
        if (item1 == null || item2 == null)
        {
            Debug.Log("Combining Failed: One of the Items in the holder has no InventoryItemController script");
            return;
        }
        
        // Trys to find a valid recipe recipe to the Inventory Items in the holders.
        CombiningRecipeData recipe = FindFirstRecipeCorrespondingToItems(item1.pickableObjectData, item2.pickableObjectData);
        bool foundARecipe = recipe != null;
        if (!foundARecipe)
        {
            Debug.Log("Combining Failed: No Recipes found for the current combination");
            return;
        }
        
        // In case a valid recipe has been found:
        // - Adds the new PickableObjectData to the inventory list.
        // - Destroys the old ones.
        // - Rebuilds the inventory to show the new Inventory Item.
        Debug.Log("There is a valid recipe, Result: " + recipe.result.name);
        Destroy(item1.gameObject);
        Destroy(item2.gameObject);
        Inventory.Instance.Add(recipe.result);
        Inventory.Instance.BuildInventoryItemsBasedOnList();
    }

    private CombiningRecipeData FindFirstRecipeCorrespondingToItems(PickableObjectData data1, PickableObjectData data2)
    {
        foreach (CombiningRecipeData recipe in combiningRecipes)
            if ((data1 == recipe.obj1 && data2 == recipe.obj2) || (data1 == recipe.obj2 && data2 == recipe.obj1))
                return recipe;
        return null;
    }
    
    private void SendDockedItemsBackToInventory(InventoryItemController dockedItem)
    {
        Inventory.Instance.Add(dockedItem.pickableObjectData);
        dockedItem.transform.SetParent(Inventory.Instance.inventoryContent.transform);
        dockedItem.isChildOfInventoryParent = true;
    }

}
