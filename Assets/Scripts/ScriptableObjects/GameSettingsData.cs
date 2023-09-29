using UnityEngine;


[CreateAssetMenu(fileName = "GameSettingsData", menuName = "ScriptableObjects/GameSettingsData", order = 0)]
public class GameSettingsData : ScriptableObject
{
    
    // Encapsulation
    public float MouseSensitivity
    {
        get => _mouseSensitivity; 
        set => _mouseSensitivity = value < 0.1f ? 0.1f : value;
    }
    
    [Header("Mouse")]
    [SerializeField, Range(1, 10)] private float _mouseSensitivity = 1.5f;
    
    [Header("FPS")]
    [Range(30, 165)] public int targetFps = 60;
    public bool useVSync = true;
    public bool unlockFps = false;
    
    private void Awake()
    {
        SyncGameFpsWithGameData();
    }

    public void SyncGameFpsWithGameData()
    {
        QualitySettings.vSyncCount = useVSync ? 1 : 0;
        Application.targetFrameRate = unlockFps ? -1 : targetFps;
    }
}
