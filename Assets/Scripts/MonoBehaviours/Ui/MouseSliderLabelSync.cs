using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MouseSliderLabelSync : MonoBehaviour
{
    
    private TextMeshProUGUI _label;
    private Slider _sliderMouseSensibility;

    private void Start()
    {
        _sliderMouseSensibility = GetComponentInParent<Slider>();
        _label = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _label.text = $"Mouse Sensitivity {_sliderMouseSensibility.value}";
    }
}
