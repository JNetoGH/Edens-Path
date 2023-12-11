using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rukha93.ModularAnimeCharacter.Customization.UI
{
    public class UISlider : MonoBehaviour
    {
        [SerializeField] private Text m_Title;
        [SerializeField] private Slider m_Slider;

        public string title
        {
            get => m_Title.text;
            set => m_Title.text = value;
        }

        public Slider slider { get => m_Slider; }
    }
}