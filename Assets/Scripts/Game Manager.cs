using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField, Range(30, 165)] private int _targetFps = 60;
    
    void Start()
    {
        Application.targetFrameRate = _targetFps;
    }
    
}
