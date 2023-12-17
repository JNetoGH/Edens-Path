using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{
    
    [Header("Required References")]
    [SerializeField, Required] private GameObject _settingsMenu;
    [SerializeField, Required] private GameObject _inventory;
    [SerializeField, Required] private GameObject _crossHair;
    [SerializeField, Required] private GameObject _playerHead;
    [SerializeField, Required] private GameObject _playerHair;
    
    [Header("Cached for Other Scripts")]
    [SerializeField, Required] private TextMeshProUGUI _npcInteractableAnimationMsg; 
    [SerializeField, Required] private PhysicMaterial _slipperyMaterial; 
    [SerializeField, Required] private GameObject _skipCutsceneMsg;
    public static PhysicMaterial SlipperyMaterial { get; private set; } // passed to pickable objects.
    public static TextMeshProUGUI NpcInteractableAnimationMsg { get; private set; } // passed to NpcInteractableAnimations.
    public static GameObject SkipCutsceneMsg { get; private set; } // passed to cutscenes
    
    // Public control properties (can be called by other scripts to control the game state).
    public static bool CanMovePlayer { get; set; } = true;
    public static bool CanRotateCamera { get; set; } = true;
    public static bool CanOpenOrCloseInventory { get; set; } = true;
    
    // Private control fields, are displayed on the editor in read-only mode, meant for debugging only.
    private static bool _isInCutscene = false;
    private static bool _isInSettingsMenu = false;
    private static bool _isInInventory = false;
    
    // Used by the pausing method,
    // in order to not play what is not supposed to be replayed when the game is unpaused.
    private static List<AudioSource> _audioSourcesPausedOnPausedEvent;

    private void Awake()
    {
        SlipperyMaterial = _slipperyMaterial;
        NpcInteractableAnimationMsg = _npcInteractableAnimationMsg;
        SkipCutsceneMsg = _skipCutsceneMsg;
        
        _audioSourcesPausedOnPausedEvent = new List<AudioSource>();
        CanMovePlayer = true;
        CanRotateCamera = true;
        CanOpenOrCloseInventory = true;
    }

    private void Start()
    {
        _isInSettingsMenu = _settingsMenu.activeInHierarchy;
        _isInInventory = _inventory.activeInHierarchy;
        if (!_isInSettingsMenu || !_isInInventory) LockTheCursor();
        else ReleaseTheCursor();
    }

    private void Update()
    {
        if (_isInSettingsMenu)
            PauseGame();
        else
            UnpauseGame();
        
        // Set's the player's head activity.
        _playerHead.SetActive(_isInCutscene);
        _playerHair.SetActive(_isInCutscene);
        
        // Menu Button Pressed
        if (Input.GetButtonDown("Settings Menu") && _settingsMenu != null)
            SwitchSettingsMenuState();
        
        // Makes sure the inventory is close during cutscenes.
        if (_isInCutscene)
            CloseInventory();
        
        // Inventory Button Pressed, can't open the menu while in cutscenes.
        if (Input.GetKeyDown(KeyCode.Tab) && !_isInSettingsMenu && CanOpenOrCloseInventory && !_isInCutscene)
            SwitchInventoryState();
        
        // only shows the cross hair while in gameplay.
        _crossHair.SetActive(!_isInCutscene && !_isInInventory && !_isInSettingsMenu);
    }
    
    
    #region Called By Button Eventes
    
        // Called by the Inventory's Resume Button
        public void SwitchSettingsMenuState()
        {
            _isInSettingsMenu = !_isInSettingsMenu;
            _settingsMenu.SetActive(_isInSettingsMenu);
            if (_isInSettingsMenu)
            {
                EnterSettingsMenuMode();
                // after switching the state of the menu, if the player was in the inventory, it will be closed.
                if (_isInInventory) CloseInventory();
            }
            else
            {
                if (_isInCutscene) EnterCutsceneMode();
                else EnterGameplayMode();
            }
        }

        // Called by the Settings Menu Resume Button
        // Called by the Inventory's Return Button
        public void SwitchInventoryState()
        {
            _isInInventory = !_isInInventory;
            _inventory.SetActive(_isInInventory);
            if (_isInInventory) EnterInventoryMenuMode();
            else EnterGameplayMode();
        }
        
        private void CloseInventory()
        {
            _inventory.SetActive(false);
            _isInInventory = false;
        }
        
    #endregion
    
    
    #region Game Modes
    
        public static void EnterGameplayMode()
        {
            // Tells the game manager a cutscene is not happening.
            GameManager._isInCutscene = false;
        
            // Makes the player movable.
            GameManager.CanMovePlayer = true;
        
            // Activates the Camera Controller.
            GameManager.ActivateCameraSystems();
            
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
            GameManager._isInCutscene = true;
        
            // Makes the player Immovable.
            GameManager.CanMovePlayer = false;
            
            // Deactivate the camera controller.
            GameManager.DeactivateCameraSystems();
            
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
            
            // Unpause te audio source
            foreach (AudioSource audioSource in _audioSourcesPausedOnPausedEvent)
                audioSource.UnPause();
            // Clears the list.
            _audioSourcesPausedOnPausedEvent.Clear();
        }

        public static void PauseGame()
        {
            Time.timeScale = 0;
            
            // Pauses every audio source as well, otherwise they keep playing.
            AudioSource[] activeAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in activeAudioSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                    _audioSourcesPausedOnPausedEvent.Add(audioSource);
                }
            }
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

        public static void ActivateCameraSystems()
        {
            FindObjectOfType<CameraController>().enabled = true;
           
            // Enables the PickupSystem.
            FindObjectOfType<PickupSystem>().enabled = true;
        }
        
        public static void DeactivateCameraSystems()
        {
            FindObjectOfType<CameraController>().enabled = false;
            
            // Release the current object held by the PickupSystem and disables the PickupSystem.
            PickupSystem pickupSystem = FindObjectOfType<PickupSystem>();
            pickupSystem.ReleaseCurrentObject(false);
            pickupSystem.enabled = false;
        } 
        
    #endregion
    
}
