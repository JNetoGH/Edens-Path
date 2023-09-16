using UnityEngine;

public class GameHandler : MonoBehaviour
{

    [Header("FPS Settings")]
    [SerializeField, Range(10, 165)] private int _targetFps = 60;
    [SerializeField] private bool _releaseFps = false;

    [Header("Mouse Settings")]
    [SerializeField, Range(1, 10)] private float _mouseSensibilityX = 2f; 
    [SerializeField, Range(1, 10)] private float _mouseSensibilityY = 2f;

    // Important Components
    private FirstPersonController _player;
    
    private void Awake()
    {
        SyncFps();
    }

    private void Start()
    {
        _player = FindObjectOfType<FirstPersonController>();
        SyncMouseSensibility();
    }
    
    private void Update()
    {
        SyncFps();
        SyncMouseSensibility();
    }
    
    private void SyncMouseSensibility()
    {
        _player.LookSpeedX = _mouseSensibilityX;
        _player.LookSpeedY = _mouseSensibilityY;
    }
    
    private void SyncFps()
    {
        Application.targetFrameRate = _releaseFps ?  Application.targetFrameRate = -1 : _targetFps;
    }
}
