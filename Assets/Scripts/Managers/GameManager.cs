using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{

    [Header("FPS")] 
    [SerializeField] private bool _useVSync = true;
    [SerializeField] private bool _unlockFps = false;
    [SerializeField, Range(10, 165)] private int _targetFps = 60;

    [Header("Settings Menu Reference")] 
    [SerializeField] private GameObject _settingsMenu;
    private bool IsInSettingsMenu { get; set; } = false;
    
    [Header("Settings Menu Components")] 
    [SerializeField] private Toggle _toggleUnlockFps; 
    [SerializeField] private Toggle _toggleVSync; 
    [SerializeField] private TextMeshProUGUI _dropdownTargetFpsValueText;
    [SerializeField] private Slider _sliderMouseSensibility;
    
    private CameraController _cameraController;
    private PlayerController _playerController;
    
    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
        _playerController = FindObjectOfType<PlayerController>();
        IsInSettingsMenu = false;
        
        // Syncs the Menu Elements with the Editor/Game default states
        _toggleUnlockFps.isOn = _unlockFps;
        _toggleVSync.isOn = _useVSync;
        _dropdownTargetFpsValueText.text = _targetFps.ToString();
        _sliderMouseSensibility.value = _cameraController.MouseSensitivity * 10;
        SetFps();
    }

    private void Update()
    {
        // Reset Button Pressed
        if (Input.GetButtonDown("Reset"))
            ResetGame();
        
        // Menu Button Pressed
        if (Input.GetButtonDown("Settings Menu"))
            SwitchMenuState();
        
        // Makes sure the Cursor is locked
        if (!IsInSettingsMenu)
            LockTheCursor();
    }
    
    public void SwitchMenuState()
    {
        IsInSettingsMenu = !IsInSettingsMenu;
        UpdateMenuState();
        SetGameBasedOnMenuValues();
    }
    
    private void UpdateMenuState()
    {
        if (IsInSettingsMenu) ReleaseTheCursor();
        else LockTheCursor();
        _settingsMenu.SetActive(IsInSettingsMenu);
        PlayerController.CanMove = !IsInSettingsMenu;
    }

    private void SetGameBasedOnMenuValues()
    {
        // Sensibility sync (from CameraController)
        _cameraController.MouseSensitivity = _sliderMouseSensibility.value / 10;
        
        // FPS sync (from Editor)
        _unlockFps = _toggleUnlockFps.isOn;
        _useVSync = _toggleVSync.isOn;
        _targetFps = int.Parse(_dropdownTargetFpsValueText.text);
        SetFps();
    }
    
    private void SetFps()
    {
        QualitySettings.vSyncCount = _useVSync ? 1 : 0;
        Application.targetFrameRate = _unlockFps ? -1 : _targetFps;
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
