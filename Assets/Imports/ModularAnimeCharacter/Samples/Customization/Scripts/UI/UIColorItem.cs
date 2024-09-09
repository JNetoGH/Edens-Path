using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rukha93.ModularAnimeCharacter.Customization.UI
{
    public class UIColorItem : MonoBehaviour
    {
        [SerializeField] private Button m_LeftButton;
        [SerializeField] private Image m_LeftImage;
        [SerializeField] private Text m_LeftText;

        [Space]
        [SerializeField] private Image m_RightImage;
        [SerializeField] private Button m_RightButton;
        [SerializeField] private Text m_RightText;

        public System.Action OnClickShade;
        public System.Action OnClickLight;

        public Color ShadeColor
        {
            get => m_LeftImage.color;
            set => m_LeftImage.color = value;
        }

        public Color LightColor
        {
            get => m_RightImage.color;
            set => m_RightImage.color = value;
        }

        private void Awake()
        {
            m_LeftButton.onClick.AddListener(_OnClickShade);
            m_RightButton.onClick.AddListener(_OnClickLight);
        }

        private void _OnClickShade()
        {
            OnClickShade?.Invoke();
        }
        private void _OnClickLight()
        {
            OnClickLight?.Invoke();
        }

        public void SetSingleColor(bool value)
        {
            m_LeftImage.gameObject.SetActive(!value);
            m_LeftButton.gameObject.SetActive(!value);

            m_LeftText.gameObject.SetActive(!value);
            m_RightText.gameObject.SetActive(!value);
        }
    }
}
