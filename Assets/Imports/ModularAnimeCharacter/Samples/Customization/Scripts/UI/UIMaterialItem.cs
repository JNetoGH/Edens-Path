using Codice.CM.Common;
using Rukha93.ModularAnimeCharacter.Customization.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rukha93.ModularAnimeCharacter.Customization.UI
{
    public class UIMaterialItem : MonoBehaviour
    {
        [SerializeField] private Text m_Title;
        [SerializeField] private LayoutGroup m_ColorPanel;
        [SerializeField] private UIColorItem m_ColorItemPrefab;

        private List<UIColorItem> m_ColorItems = new List<UIColorItem>();

        public UIColorPicker ColorPicker { get; set; }

        public string Title
        {
            get => m_Title.text;
            set => m_Title.text = value;
        }

        private void Awake()
        {
            m_ColorItemPrefab.gameObject.SetActive(false);
        }

        public void ResetColors()
        {
            for (int i = 0; i < m_ColorItems.Count; i++)
                m_ColorItems[i].gameObject.SetActive(false);
        }

        public void SetupSingleColor(Color currentColor, System.Action<Color> onChangeColor)
        {
            currentColor.a = 1;

            UIColorItem item;
            if (m_ColorItems.Count > 0)
            {
                item = m_ColorItems[0];
            }
            else
            {
                item = Instantiate(m_ColorItemPrefab, m_ColorPanel.transform);
                m_ColorItems.Add(item);
            }
            item.gameObject.SetActive(true);

            for (int i = 1; i < m_ColorItems.Count; i++)
                m_ColorItems[i].gameObject.SetActive(false);

            item.SetSingleColor(true);
            item.LightColor = currentColor;
            item.OnClickLight = () =>
            {
                var pos = ColorPicker.transform.position;
                pos.y = item.transform.position.y;
                ColorPicker.transform.position = pos;

                ColorPicker.Show(
                item.LightColor,
                (c) =>
                {
                    item.LightColor = c;
                    onChangeColor?.Invoke(c);
                });
            };
        }

        public void AddDoubleColor(Color color1, Color color2, System.Action<Color> onChangeColor1, System.Action<Color> onChangeColor2)
        {
            color1.a = color2.a = 1;

            UIColorItem item = null;
            for (int i = 0; i < m_ColorItems.Count; i++)
                if (m_ColorItems[i].gameObject.activeSelf == false)
                    item = m_ColorItems[i];
            if (item == null)
            {
                item = Instantiate(m_ColorItemPrefab, m_ColorPanel.transform);
                m_ColorItems.Add(item);
            }
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            item.SetSingleColor(false);

            item.LightColor = color1;
            item.OnClickLight = () =>
            {
                var pos = ColorPicker.transform.position;
                pos.y = item.transform.position.y;
                ColorPicker.transform.position = pos;

                ColorPicker.Show(
                item.LightColor,
                (c) =>
                {
                    item.LightColor = c;
                    onChangeColor1?.Invoke(c);
                });
            };

            item.ShadeColor = color2;
            item.OnClickShade = () =>
            {
                var pos = ColorPicker.transform.position;
                pos.y = item.transform.position.y;
                ColorPicker.transform.position = pos;

                ColorPicker.Show(
                item.ShadeColor,
                (c) =>
                {
                    item.ShadeColor = c;
                    onChangeColor2?.Invoke(c);
                });
            };
        }
    }
}