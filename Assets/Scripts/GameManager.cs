using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField, Range(30, 165)] private int _targetFps = 60;

    private void Awake()
    {
        Application.targetFrameRate = _targetFps;
    }

    private void Update()
    {
        Application.targetFrameRate = _targetFps;
    }
}
