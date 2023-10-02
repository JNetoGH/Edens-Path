using ScriptableObjects;
using UnityEngine;


public class InventoryItemController: MonoBehaviour
{
    
    // All set by the inventory system at inventory build
    [HideInInspector] public PickableObjectData pickableObjectData;
    [HideInInspector] public int indexInInventory;
    
    // Called by the button component on the Inventory Item Prefab
    public void InstantiateCorrespondingPickableAndRemoveFromInventory()
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
        
        // Removes itself from the inventory list
        // Rebuilds the inventory
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.pickableObjectsList.RemoveAt(indexInInventory);
        inventory.BuildInventoryItemsBasedOnList();
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
