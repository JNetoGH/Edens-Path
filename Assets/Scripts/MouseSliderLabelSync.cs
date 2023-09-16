using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MouseSliderLabelSync : MonoBehaviour
{

    [SerializeField] private string _axisName = "?";
    private TextMeshProUGUI _label;
    private Slider _sliderMouseSensibility;

    private void Start()
    {
        _sliderMouseSensibility = GetComponentInParent<Slider>();
        _label = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _label.text = $"Mouse Sensitivity {_axisName}: {_sliderMouseSensibility.value}";
    }
}
