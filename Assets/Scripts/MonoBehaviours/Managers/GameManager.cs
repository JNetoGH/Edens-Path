using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _inventory;
    public bool IsInSettingsMenu { get; private set; } = false;
    public bool IsInInventory { get; private set; } = false;
    
    private void Start()
    {
        IsInSettingsMenu = _settingsMenu.activeInHierarchy;
        IsInInventory = _inventory.activeInHierarchy;
        
        if (!IsInSettingsMenu || !IsInInventory) LockTheCursor();
        else ReleaseTheCursor();   
    }

    private void Update()
    {
        // Reset Button Pressed
        if (Input.GetButtonDown("Reset"))
            ResetGame();
        
        // Menu Button Pressed
        if (Input.GetButtonDown("Settings Menu") && _settingsMenu != null && !IsInInventory)
            SwitchMenuState();
        
        // Inventory Button Pressed
        if (Input.GetKeyDown(KeyCode.I) && !IsInSettingsMenu)
        {
            _inventory.SetActive(true);
            IsInInventory = true;
        }
        else if (Input.GetKeyDown(KeyCode.O))
            CloseInventory();
    }
    
    // Also called by the Inventory's Resume Button
    public void SwitchMenuState()
    {
        IsInSettingsMenu = !IsInSettingsMenu;
        _settingsMenu.SetActive(IsInSettingsMenu);
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
    
    public static void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
    
}
