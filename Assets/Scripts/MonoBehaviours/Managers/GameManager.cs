using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _inventory;
    
    public static bool CanOpenInventory { get; set; } = true;
    public bool IsInSettingsMenu { get; private set; } = false;
    public bool IsInInventory { get; set; } = false;
    public static bool IsInCutscene { get; set; } = false;
    
    private void Start()
    {
        CanOpenInventory = true;
        IsInSettingsMenu = _settingsMenu.activeInHierarchy;
        IsInInventory = _inventory.activeInHierarchy;
        
        if (!IsInSettingsMenu || !IsInInventory) LockTheCursor();
        else ReleaseTheCursor();   
    }

    private void Update()
    {

        if (IsInSettingsMenu)
            PauseGame();
        else
            UnpauseGame();
        
        // Reset Button Pressed
        if (Input.GetButtonDown("Reset"))
            ResetGame();
        
        // Menu Button Pressed
        if (Input.GetButtonDown("Settings Menu") && _settingsMenu != null)
        {
            SwitchMenuState();
            
            // after switching the state of the menu e.g. the player was in the inventory,
            // the inventory it will be closed and the menu open.
            if (IsInSettingsMenu && IsInInventory && !IsInCutscene)
            {
                CloseInventory();
                ReleaseTheCursor();
                PlayerController.CanMove = false;
            }
        }

        // Inventory Button Pressed
        if (Input.GetKeyDown(KeyCode.I) && !IsInSettingsMenu && CanOpenInventory)
            SwitchInventoryState();
    }
    
    // Also called by the Inventory's Resume Button
    public void SwitchMenuState()
    {
        IsInSettingsMenu = !IsInSettingsMenu;
        _settingsMenu.SetActive(IsInSettingsMenu);
    }

    public void SwitchInventoryState()
    {
        IsInInventory = !IsInInventory;
        _inventory.SetActive(IsInInventory);
    }
    
    // Also called by the Inventory's Return Button
    public void CloseInventory()
    {
        _inventory.SetActive(false);
        IsInInventory = false;
    }
    
    public static void LockTheCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public static void ReleaseTheCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public static void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    
    public static void ResetGame()
    {
        SceneManager.LoadScene(0);
        
        // Needs to be reset if the game is reloaded,
        // Other wise it will keep track of old MonoBehaviour tha may have been destroyed.
        SuperTag.ResetControlListReference();
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void ActivateScreenCross()
    {
        SuperTag.GetFirstObjectWithSuperTag("cross")?.SetActive(true);
    }
    
    public static void DeactivateScreenCross()
    {
        SuperTag.GetFirstObjectWithSuperTag("cross")?.SetActive(false);
    }
    
}
