using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rukha93.ModularAnimeCharacter.Customization.UI
{
    public class UIColorPicker : MonoBehaviour
    {
        [Space]
        [SerializeField] private UISlider m_RedSlider;
        [SerializeField] private UISlider m_GreenSlider;
        [SerializeField] private UISlider m_BlueSlider;

        [Space]
        [SerializeField] private Image m_LastColorImage;
        [SerializeField] private Button m_LastColorButton;

        [Space]
        [SerializeField] private Button m_CloseButton;

        public System.Action<Color> OnChangeColor;

        private Color m_Color;

        private void Awake()
        {
            //m_RedSlider.title = "red";
            //m_GreenSlider.title = "green";
            //m_BlueSlider.title = "blue";

            m_RedSlider.title = "H";
            m_GreenSlider.title = "S";
            m_BlueSlider.title = "V";

            m_CloseButton.onClick.AddListener(OnClickClose);
            m_LastColorButton.onClick.AddListener(OnClickLastColor);
        }

        public void Show(Color color, System.Action<Color> onChangeColor)
        {
            if (gameObject.activeSelf)
                Close();

            m_RedSlider.slider.onValueChanged.RemoveAllListeners();
            m_GreenSlider.slider.onValueChanged.RemoveAllListeners();
            m_BlueSlider.slider.onValueChanged.RemoveAllListeners();

            m_Color = color;
            SetSliders(color);
            OnChangeColor = onChangeColor;

            m_RedSlider.slider.onValueChanged.AddListener(OnSliderChange);
            m_GreenSlider.slider.onValueChanged.AddListener(OnSliderChange);
            m_BlueSlider.slider.onValueChanged.AddListener(OnSliderChange);

            gameObject.SetActive(true);
        }

        public void Close()
        {
            //m_LastColorImage.color = new Color(m_RedSlider.slider.value, m_GreenSlider.slider.value, m_BlueSlider.slider.value);
            m_LastColorImage.color = Color.HSVToRGB(m_RedSlider.slider.value, m_GreenSlider.slider.value, m_BlueSlider.slider.value);

            m_RedSlider.slider.onValueChanged.RemoveAllListeners();
            m_GreenSlider.slider.onValueChanged.RemoveAllListeners();
            m_BlueSlider.slider.onValueChanged.RemoveAllListeners();

            OnChangeColor = null;
            gameObject.SetActive(false);
        }

        private void OnSliderChange(float v)
        {
            //m_Color = new Color(m_RedSlider.slider.value, m_GreenSlider.slider.value, m_BlueSlider.slider.value);
            m_Color = Color.HSVToRGB(m_RedSlider.slider.value, m_GreenSlider.slider.value, m_BlueSlider.slider.value);
            OnChangeColor?.Invoke(m_Color);
        }

        private void SetSliders(Color c)
        {
            //m_RedSlider.slider.value = c.r;
            //m_GreenSlider.slider.value = c.g;
            //m_BlueSlider.slider.value = c.b;

            float h, s, v;
            Color.RGBToHSV(c, out h, out s, out v);

            m_RedSlider.slider.value = h;
            m_GreenSlider.slider.value = s;
            m_BlueSlider.slider.value = v;
        }

        private void OnClickClose()
        {
            this.Close();
        }

        private void OnClickLastColor()
        {
            SetSliders(m_LastColorImage.color);
        }
    }
}
