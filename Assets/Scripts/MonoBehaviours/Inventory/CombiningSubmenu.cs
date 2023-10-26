using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Manages the combining submenu functionality.
/// </summary>
public class CombiningSubmenu : MonoBehaviour
{

    // Singleton Pattern
    public static CombiningSubmenu Instance { get; private set; }
    
    [HorizontalLine]
    [BoxGroup("COMBINING RECIPES DATA BASE")]
    [InfoBox("This list represents the data base of possible combining recipes (ScriptableObjects)")]
    [SerializeField] private List<CombiningRecipeData> _combiningRecipes = new List<CombiningRecipeData>(); 

    [Header("REQUIRED REFERENCES (may be passed as singleton)"), HorizontalLine] 
    [BoxGroup, Required] public GameObject combiningSubmenuHitbox;
    [BoxGroup, Required] public GameObject itemHolderLeft;
    [BoxGroup, Required] public GameObject itemHolderRight;
    [BoxGroup, Required] public GameObject resultPreview;
    
    // Auto-Implemented Get-Only Communication Properties.
    // Used to other classes to understand the current state od the Combining Submenu.
    public bool IsLeftItemHolderFree => itemHolderLeft.transform.childCount == 0;
    public bool IsRightItemHolderFree => itemHolderRight.transform.childCount == 0;
    public bool IsResultPreviewFree => resultPreview.transform.childCount == 0;
    public bool AnyItemHolderAvailable => (IsLeftItemHolderFree || IsRightItemHolderFree);
    
    // EDITOR-ONLY:
    // Debugging Information ReadOnly Fields Wrapper.
    // Properties are not very customizable and hard to update with Naughty Attributes, this workaround it's needed.
    private const string G2 = "DEBUG INFO WRAPPER";
    [HorizontalLine]
    [BoxGroup(G2), ReadOnly, SerializeField] private bool _isLeftItemHolderFree = true;
    [BoxGroup(G2), ReadOnly, SerializeField] private bool _isRightItemHolderFree = true;
    [BoxGroup(G2), ReadOnly, SerializeField] private bool _isResultPreviewFree = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateDebugEditorInfo();
        UpdateCombiningPreview();
    }

    /// <summary>
    /// Updates the Debugging Information ReadOnly Fields Wrapper.
    /// </summary>
    private void UpdateDebugEditorInfo()
    {
        _isLeftItemHolderFree = IsLeftItemHolderFree;
        _isRightItemHolderFree = IsRightItemHolderFree;
        _isResultPreviewFree = IsResultPreviewFree;
    }
    
    /// <summary>
    /// Updates the preview of the combining process based on the items in the item holders.
    /// </summary>
    private void UpdateCombiningPreview()
    {
        bool areBothHolderOccupied = !IsRightItemHolderFree && !IsLeftItemHolderFree;
        if (!areBothHolderOccupied)
        {
            // Clears previous preview item in case any of the holders has been changed.
            if (!IsResultPreviewFree)
            {
                GameObject objPrev = resultPreview.transform.GetChild(0).gameObject;
                Destroy(objPrev);
            }
            return;
        }

        // can't child another obj to preview if there is already one, they will stack.
        if (!IsResultPreviewFree)
            return;

        InventoryItemController item1 = itemHolderLeft.transform.GetChild(0).GetComponent<InventoryItemController>();
        InventoryItemController item2 = itemHolderRight.transform.GetChild(0).GetComponent<InventoryItemController>();
        CombiningRecipeData recipe =
            FindFirstRecipeCorrespondingToItems(item1.pickableObjectData, item2.pickableObjectData);
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
        if (!IsLeftItemHolderFree)  SendDockedItemsBackToInventory(itemHolderLeft.transform.GetChild(0).GetComponent<InventoryItemController>());
        if (!IsRightItemHolderFree) SendDockedItemsBackToInventory(itemHolderRight.transform.GetChild(0).GetComponent<InventoryItemController>());
    }

    /// <summary>
    /// Called by the button on the Combining submenu.
    /// Attempts to combine items in the item holders based on available recipes.
    /// </summary>
    public void TryCombine()
    {
        // Checks before if both holder have a Inventory Item as child.
        if (AnyItemHolderAvailable)
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

    /// <summary>
    /// Finds the first combining recipe corresponding to the provided pickable object data.
    /// </summary>
    /// <param name="data1">The first pickable object data.</param>
    /// <param name="data2">The second pickable object data.</param>
    /// <returns>The combining recipe data if found, null otherwise.</returns>
    private CombiningRecipeData FindFirstRecipeCorrespondingToItems(PickableObjectData data1, PickableObjectData data2)
    {
        foreach (CombiningRecipeData recipe in _combiningRecipes)
            if ((data1 == recipe.obj1 && data2 == recipe.obj2) || (data1 == recipe.obj2 && data2 == recipe.obj1))
                return recipe;
        return null;
    }
    
    /// <summary>
    /// Sends the docked item back to the inventory, detaching it from the current parent.
    /// </summary>
    /// <param name="dockedItem">The docked inventory item controller.</param>
    private void SendDockedItemsBackToInventory(InventoryItemController dockedItem)
    {
        Inventory.Instance.Add(dockedItem.pickableObjectData);
        dockedItem.transform.SetParent(Inventory.Instance.inventoryContent.transform);
        dockedItem.isChildOfInventoryParent = true;
    }

}