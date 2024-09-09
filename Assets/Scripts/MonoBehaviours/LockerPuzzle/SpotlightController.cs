using System;
using System.Linq;
using TMPro;
using UnityEngine;
using VLB;


public class SpotlightController : MonoBehaviour
{

    [SerializeField] private float _digitIndex = 99;
    [SerializeField] private float _totalBlinks = 3;
    [SerializeField] private Color _baseColor = Color.yellow;
    [SerializeField] private Color _endCycleColor = Color.red;
    [SerializeField] private float _litDuration = 2;
    [SerializeField] private float _unlitDuration = 1;
    [SerializeField] private VolumetricLightBeam _volumetricLight;
    private float _timerUnlit = 0;
    private float _timerLit = 0;
    
    private LightMode _lightMode;
    private int _blinkCounter = 0;

    private TextMeshProUGUI _textMesh;
    
    private void Awake()
    {
        _lightMode = LightMode.Lit;
        _blinkCounter = 0;
    }

    private void Start()
    {
        GetComponentsInChildren<TextMeshProUGUI>().ToList().ForEach(t => t.text = _digitIndex.ToString());
    }

    private void Update()
    {
        switch (_lightMode)
        {
            case LightMode.Lit: UpdateLit(); break;
            case LightMode.Unlit: UpdateUnlit(); break;
        }
    }

    private void UpdateLit()
    {
        _timerLit += Time.deltaTime;
        if (_timerLit >= _litDuration)
            Unlit();
    }
    
    private void UpdateUnlit()
    {
        _timerUnlit += Time.deltaTime;
        if (_timerUnlit >= _unlitDuration)
            Lit();
    }

    private void Unlit()
    {
        _lightMode = LightMode.Unlit;
        _timerLit = 0;
        _volumetricLight.gameObject.SetActive(false);
        _blinkCounter++;
    }

    private void Lit()
    {
        _lightMode = LightMode.Lit;
        _timerUnlit = 0;
        
        if (_blinkCounter > _totalBlinks)
        {
            _blinkCounter = 0;
            _volumetricLight.color = _endCycleColor;
        }
        else
        {
            _volumetricLight.color = _baseColor;
        }
        
        _volumetricLight.gameObject.SetActive(true);
    }
    
}
