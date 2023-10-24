using UnityEngine;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _inventory;
    
    public static bool CanMovePlayer { get; set; } = true;
    public static bool CanRotateCamera { get; set; } = true;
    public static bool CanOpenOrCloseInventory { get; set; } = true;
    public static bool IsInCutscene { get; set; } = false;
    
    public bool IsInSettingsMenu { get; private set; } = false;
    public bool IsInInventory { get; set; } = false;

    private void Awake()
    {
        CanMovePlayer = true;
        CanRotateCamera = true;
        CanOpenOrCloseInventory = true;
    }

    private void Start()
    {
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
            SwitchSettingsMenuState();
        
        // Makes sure the inventory is close during cutscenes.
        if (IsInCutscene)
            CloseInventory();
        
        // Inventory Button Pressed, can't open the menu while in cutscenes.
        if (Input.GetKeyDown(KeyCode.I) && !IsInSettingsMenu && CanOpenOrCloseInventory && !IsInCutscene)
            SwitchInventoryState();
        
    }
    
    
    #region Called By Button Eventes
    
        // Called by the Inventory's Resume Button
        public void SwitchSettingsMenuState()
        {
            IsInSettingsMenu = !IsInSettingsMenu;
            _settingsMenu.SetActive(IsInSettingsMenu);
            if (IsInSettingsMenu)
            {
                EnterSettingsMenuMode();
                // after switching the state of the menu, if the player was in the inventory, it will be closed.
                if (IsInInventory) CloseInventory();
            }
            else
            {
                if (IsInCutscene) EnterCutsceneMode();
                else EnterGameplayMode();
            }
        }

        // Called by the Settings Menu Resume Button
        public void SwitchInventoryState()
        {
            IsInInventory = !IsInInventory;
            _inventory.SetActive(IsInInventory);
            if (IsInInventory) EnterInventoryMenuMode();
            else EnterGameplayMode();
        }
        
        // Called by the Inventory's Return Button
        public void CloseInventory()
        {
            _inventory.SetActive(false);
            IsInInventory = false;
        }
        
    #endregion
    
    
    #region Game Modes
    
        public static void EnterGameplayMode()
        {
            // Tells the game manager a cutscene is not happening.
            GameManager.IsInCutscene = false;
        
            // Makes the player movable.
            GameManager.CanMovePlayer = true;
        
            // Makes Camera movable.
            GameManager.CanRotateCamera = true;
        
            // Enables the player to open/close the inventory.
            GameManager.CanOpenOrCloseInventory = true;
        
            // Activate screen cross.
            GameManager.ActivateScreenCross();
            
            // Locks the cursor.
            GameManager.LockTheCursor();
        }
        
        public static void EnterCutsceneMode()
        {
            // Tells the game manager a cutscene is happening.
            GameManager.IsInCutscene = true;
        
            // Makes the player Immovable.
            GameManager.CanMovePlayer = false;
        
            // Makes Camera Immovable.
            GameManager.CanRotateCamera = false;
            
            // Disables the player unable to open the inventory during cutscenes.
            GameManager.CanOpenOrCloseInventory = false;
            
            // Deactivate screen cross.
            GameManager.DeactivateScreenCross();
            
            // Locks the cursor.
            GameManager.LockTheCursor();
        }
        
        public static void EnterSettingsMenuMode()
        {
            // May or may not be in a cutscene, so it's not set.
            
            // Makes the player movable
            GameManager.CanMovePlayer = false;
        
            // Makes Camera movable
            GameManager.CanRotateCamera = false;
        
            // Disables the player to open/close the inventory while in Setting menu.
            GameManager.CanOpenOrCloseInventory = false;
        
            // Deactivate screen cross.
            GameManager.DeactivateScreenCross();
            
            // Unlocks the cursor
            GameManager.ReleaseTheCursor();
        }
        
        public static void EnterInventoryMenuMode()
        {
            // May or may not be in a cutscene, so it's not set.
            
            // Makes the player movable
            GameManager.CanMovePlayer = false;
        
            // Makes Camera movable
            GameManager.CanRotateCamera = false;
        
            // Enables the player to open/close the inventory during cutscenes.
            GameManager.CanOpenOrCloseInventory = true;
        
            // Deactivate screen cross.
            GameManager.DeactivateScreenCross();
            
            // Unlocks the cursor
            GameManager.ReleaseTheCursor();
        }
        
    #endregion

    
    #region General Utilities
    
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
        
    #endregion
    
}
