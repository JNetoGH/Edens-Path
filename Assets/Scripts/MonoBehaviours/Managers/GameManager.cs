using UnityEngine;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _inventoryContainer;
    private bool IsInSettingsMenu { get; set; } = false;
    private bool IsInInventory { get; set; } = false;
    
    private void Start()
    {
        IsInSettingsMenu = _settingsMenu.activeInHierarchy;
        if (!IsInSettingsMenu)
            LockTheCursor();
        else
            ReleaseTheCursor();   
    }

    private void Update()
    {
        // Reset Button Pressed
        if (Input.GetButtonDown("Reset"))
            ResetGame();
        
        // Menu Button Pressed
        if (Input.GetButtonDown("Settings Menu") && _settingsMenu != null && !IsInInventory)
            SwitchMenuState();
        
        // Inventory Menus
        if (Input.GetKeyDown(KeyCode.I) && !IsInSettingsMenu)
        {
            _inventoryContainer.SetActive(true);
            IsInInventory = true;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            _inventoryContainer.SetActive(false);
            IsInInventory = false;
        }
    }
    
    public void SwitchMenuState()
    {
        IsInSettingsMenu = !IsInSettingsMenu;
        _settingsMenu.SetActive(IsInSettingsMenu);
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
