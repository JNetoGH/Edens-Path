using UnityEngine;
using UnityEngine.UI;


public class SoundToggleButton : MonoBehaviour
{
    
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private SoundTrackController _soundTrackController;

    private bool _isOn = true;
    private Image _buttonImage; 

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
        if (_buttonImage == null)
            Debug.LogError("No Image component found on the button object.");
    }

    private void Start()
    {
        //_isOn = !_soundTrackController.IsMute; 
        UpdateButtonVisual();
    }

    public void ToggleState()
    {
        _isOn = !_isOn;
        UpdateButtonVisual();
        if (_soundTrackController != null)
            _soundTrackController.ToggleMute();
        else 
            Debug.LogWarning("Audio_Controller reference not set on ToggleButton.");
    }

    private void UpdateButtonVisual()
    {
        _buttonImage.sprite = _isOn ? _onSprite : _offSprite;
    }
    
}
