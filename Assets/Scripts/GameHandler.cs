using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    
    [SerializeField, Range(10, 165)] private int _targetFps = 60;
    [SerializeField] private bool _releaseFps = false;
    private FirstPersonController _player;
    
    
    private void Awake()
    {
        Application.targetFrameRate = _targetFps;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Application.targetFrameRate = _targetFps;
        if (_releaseFps) 
            Application.targetFrameRate = -1;
    }
    
}
