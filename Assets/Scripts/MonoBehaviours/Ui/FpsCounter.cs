using System.Globalization;
using TMPro;
using UnityEngine;


// Author => https://sharpcoderblog.com/blog/unity-fps-counter
public class FpsCounter : MonoBehaviour
{ 
    [SerializeField, Tooltip("How often should the number update")]
    private float _updateInterval = 0.5f;
    private TextMeshProUGUI _fpsText;
    private float _accum = 0.0f;
    private int _frames = 0;
    private float _timeLeft;
    private float _fps;
    
    void Start()
    {
        _fpsText = GetComponent<TextMeshProUGUI>();
        _timeLeft = _updateInterval;
    }
    
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;

        // Interval ended - update GUI text and start new interval
        if (_timeLeft <= 0.0)
        {
            // display two fractional digits (f2 format)
            _fps = (_accum / _frames);
            _timeLeft = _updateInterval;
            _accum = 0.0f;
            _frames = 0;
            _fpsText.text = "FPS: " + Mathf.CeilToInt(_fps).ToString(CultureInfo.InvariantCulture);
        }
    }
    
}