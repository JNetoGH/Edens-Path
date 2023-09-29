using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    
    [Header("Data")]
    [SerializeField] private GameSettingsData _gameSettingsData;
    
    [Header("Settings Menu Components")] 
    [SerializeField] private Toggle _toggleUnlockFps; 
    [SerializeField] private Toggle _toggleVSync;
    [SerializeField] private TMP_Dropdown _dropdownTargetFpsButton;
    [SerializeField] private TextMeshProUGUI _dropdownTargetFpsValueText;
    [SerializeField] private Slider _sliderMouseSensibility;
    
    private CameraController _cameraController;
    
    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }
    
    private void OnEnable()
    {
        GameManager.ReleaseTheCursor();
        PlayerController.CanMove = false;
        SyncMenuWithGameData();
    }

    private void OnDisable()
    {
        GameManager.LockTheCursor();
        PlayerController.CanMove = true;
        SyncGameDataWithMenu();
    }
    
    private void SyncMenuWithGameData()
    {
        // FPS sync (From Data)
        _toggleUnlockFps.isOn = _gameSettingsData.unlockFps;
        _toggleVSync.isOn = _gameSettingsData.useVSync;
        for (int index = 0; index < _dropdownTargetFpsButton.options.Count; index++)
        {
            TMP_Dropdown.OptionData optionData = _dropdownTargetFpsButton.options[index];
            if (int.Parse(optionData.text) == _gameSettingsData.targetFps)
                _dropdownTargetFpsButton.value = index;
        }
        
        // Sensibility sync (from CameraController)
        // It's times 10 in order to be easier to be set by the user
        _sliderMouseSensibility.value = _gameSettingsData.MouseSensitivity * 10;
    }
    
    private void SyncGameDataWithMenu()
    {
        // Sensibility sync (from CameraController)
        // Needs to be divide by 10 inorder to be applied
        _gameSettingsData.MouseSensitivity = _sliderMouseSensibility.value / 10;

        // FPS sync (Into Data)
        _gameSettingsData.unlockFps = _toggleUnlockFps.isOn;
        _gameSettingsData.useVSync = _toggleVSync.isOn;
        _gameSettingsData.targetFps = int.Parse(_dropdownTargetFpsValueText.text);
       
        // Updates the game to the Fps Settings
        _gameSettingsData.SyncGameFpsWithGameData();
    }

} 
