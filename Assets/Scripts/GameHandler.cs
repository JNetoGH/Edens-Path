using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{

    [Header("FPS Settings")]
    [SerializeField, Range(10, 165)] private int _targetFps = 60;
    [SerializeField] private bool _unlockFps = false;

    [Header("Mouse Settings")]
    [SerializeField, Range(1, 10)] private float _mouseSensibilityX = 2f; 
    [SerializeField, Range(1, 10)] private float _mouseSensibilityY = 2f;

    [Header("Menu Settings")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private Toggle _toggleUnlockFps;
    [SerializeField] private TextMeshProUGUI _dropdownTargetFps;
    [SerializeField] private Slider _sliderMouseSensibilityX;
    [SerializeField] private Slider _sliderMouseSensibilityY;

    public bool IsInMenu { get; set; } = false; // called by the menu return button as well

    // Important Components
    private FirstPersonController _player;
    
    private void Awake()
    {
        IsInMenu = false;
        LockTheCursor();
        SyncFps();
    }

    private void Start()
    {
        _player = FindObjectOfType<FirstPersonController>();
        SyncMouseSensibility();
        
        // Syncs the Menu with the Handler default state
        _toggleUnlockFps.isOn = _unlockFps;
        _dropdownTargetFps.text = _targetFps.ToString();
        _sliderMouseSensibilityX.value = _mouseSensibilityX;
        _sliderMouseSensibilityY.value = _mouseSensibilityY;
    }
    
    private void Update()
    {
        SyncFps();
        SyncMouseSensibility();
        
        if (Input.GetButtonDown("Reset"))
            ResetGame();
        
        // Enter and Exits the Menu
        if (Input.GetButtonDown("Menu"))
            SwitchMenuState();
        SyncMenuState();

        // Syncs the Handle with the Menu current state
        _unlockFps = _toggleUnlockFps.isOn;
        _mouseSensibilityX = _sliderMouseSensibilityX.value;
        _mouseSensibilityY= _sliderMouseSensibilityY.value;
        _targetFps = int.Parse(_dropdownTargetFps.text);
    }

    private void SyncMenuState()
    {
        if (IsInMenu) ReleaseTheCursor();
        else LockTheCursor();
        _menu.SetActive(IsInMenu);
        _player.CanMove = !IsInMenu;
    }

    private void SwitchMenuState()
    {
        Debug.Log($"Menu state switched to: active? {IsInMenu}");
        IsInMenu = !IsInMenu;
    }

    private void SyncMouseSensibility()
    {
        _player.LookSpeedX = _mouseSensibilityX;
        _player.LookSpeedY = _mouseSensibilityY;
    }
    
    private void SyncFps()
    {
        Application.targetFrameRate = _unlockFps ?  Application.targetFrameRate = -1 : _targetFps;
    }
    
    public static void ReleaseTheCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public static void LockTheCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
