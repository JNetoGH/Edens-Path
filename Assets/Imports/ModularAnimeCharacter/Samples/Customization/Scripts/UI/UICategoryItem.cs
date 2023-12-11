using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rukha93.ModularAnimeCharacter.Customization.UI
{
    public class UICategoryItem : MonoBehaviour
    {
        [SerializeField] private Text m_CategoryTitle;
        [SerializeField] private Text m_EquipedItemTitle;
        [SerializeField] private Button m_Button;

        public System.Action OnClick;

        public string Title
        {
            get => m_CategoryTitle.text;
            set => m_CategoryTitle.text = value;
        }

        public string Value
        {
            get => m_EquipedItemTitle.text;
            set => m_EquipedItemTitle.text = value;
        }

        private void Awake()
        {
            m_Button.onClick.AddListener(Callback_OnClick);
        }

        private void Callback_OnClick()
        {
            OnClick?.Invoke();
        }
    }
}