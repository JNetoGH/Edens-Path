using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rukha93.ModularAnimeCharacter.Customization.UI
{
    public class UICustomizationDemo : MonoBehaviour
    {
        [Header("category")]
        [SerializeField] private LayoutGroup m_CategoryPanel;
        [SerializeField] private UICategoryItem m_CategoryItemPrefab;

        [Header("customization")]
        [SerializeField] private Text m_CustomizationTitle;
        [SerializeField] private LayoutGroup m_CustomizationPanel;
        [SerializeField] private UISwapperItem m_SwapperItem;
        [SerializeField] private UIMaterialItem m_MaterialItemPrefab;

        [Space]
        [SerializeField] private UIColorPicker m_ColorPicker;

        public string CurrentCategory { get; private set; }

        private List<UICategoryItem> m_CategoryItems = new List<UICategoryItem>();
        private List<UIMaterialItem> m_MaterialItems = new List<UIMaterialItem>();

        private List<string> m_ItemOptions;
        private int m_CurrentItemIndex;

        public System.Action<string> OnClickCategory;
        public System.Action<string, string> OnChangeItem;
        public System.Action<Renderer, int, string, Color> OnChangeColor;

        public bool IsCustomizationOpen => m_CustomizationPanel.gameObject.activeSelf;


        private void Awake()
        {
            m_CategoryItemPrefab.gameObject.SetActive(false);
            m_MaterialItemPrefab.gameObject.SetActive(false);

            m_SwapperItem.OnClickLeft = _OnSwapPrevious;
            m_SwapperItem.OnClickRight = _OnSwapNext;
        }

        private void Start()
        {
            ShowCustomization(false);
            m_ColorPicker.Close();
        }

        #region CATEGORY PANEL

        public void SetCategories(string[] categories)
        {
            for(int i = 0; i < categories.Length; i++)
            {
                UICategoryItem item;
                if (i < m_CategoryItems.Count)
                {
                    item = m_CategoryItems[i];
                }
                else
                {
                    item = Instantiate(m_CategoryItemPrefab, m_CategoryPanel.transform);
                    m_CategoryItems.Add(item);
                }

                item.gameObject.SetActive(true);
                item.Title = categories[i];

                string aux = categories[i];
                item.OnClick = () => OnClickCategory?.Invoke(aux);
            }

            for (int i = categories.Length; i < m_CategoryItems.Count; i++)
                m_CategoryItems[i].gameObject.SetActive(false);
        }

        public void SetCategoryValue(int index, string value)
        {
            UICategoryItem item = m_CategoryItems[index];
            item.Value = value;
        }

        #endregion

        #region CUSTOMIZATION PANEL

        public void ShowCustomization(bool value)
        {
            //todo: add basic animation

            if (value == false)
            {
                CurrentCategory = "";
                m_CustomizationPanel.gameObject.SetActive(false);
                m_ColorPicker.Close();
                return;
            }

            m_CustomizationPanel.gameObject.SetActive(true);
        }

        public void SetCustomizationOptions(string category, string[] items, string currentItem)
        {
            CurrentCategory = category;
            m_ItemOptions = new List<string>(items);
            m_CurrentItemIndex = Mathf.Max(m_ItemOptions.IndexOf(currentItem), 0);

            m_CustomizationTitle.text = category;

            //item swapping
            if( m_CurrentItemIndex >= m_ItemOptions.Count)
            {
                m_SwapperItem.gameObject.SetActive(false);
                return;
            }

            m_SwapperItem.Text = m_ItemOptions[m_CurrentItemIndex];
            m_SwapperItem.gameObject.SetActive(true);
        }

        public void SetCustomizationMaterials(Renderer[] renderers)
        {
            if (renderers == null)
                renderers = new Renderer[0];

            //get all available materials
            List<Material> materials = new List<Material>();
            List<Renderer> renderersPerMaterial = new List<Renderer>();
            List<int> rendererMaterialIndex = new List<int>();
            List<MaterialPropertyBlock> propertyBlock = new List<MaterialPropertyBlock>();

            for(int i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                var sharedMaterials = renderer.sharedMaterials;

                for (int j = 0; j < sharedMaterials.Length; j++)
                {
                    if (materials.Contains(sharedMaterials[j]))
                        continue;
                    materials.Add(sharedMaterials[j]);
                    renderersPerMaterial.Add(renderer);
                    rendererMaterialIndex.Add(j);

                    var block = new MaterialPropertyBlock();
                    renderer.GetPropertyBlock(block, j);
                    propertyBlock.Add(block);
                }
            }

            for (int i = 0; i < materials.Count; i++)
            {
                //item from pool or instantiate
                UIMaterialItem item;
                if (i < m_MaterialItems.Count)
                {
                    item = m_MaterialItems[i];
                }
                else
                {
                    item = Instantiate(m_MaterialItemPrefab, m_CustomizationPanel.transform);
                    item.ColorPicker = m_ColorPicker;
                    m_MaterialItems.Add(item);
                }
                item.gameObject.SetActive(true);

                //setup item
                item.Title = materials[i].name;

                var auxRenderer = renderersPerMaterial[i];
                var auxMatIndex = rendererMaterialIndex[i];
                var auxPropertyBlock = propertyBlock[i];

                if (materials[i].HasProperty("_MaskRemap"))
                {
                    item.ResetColors();

                    //customization channel 1
                    item.AddDoubleColor(
                        auxPropertyBlock.HasColor("_Color_A_2") ? auxPropertyBlock.GetColor("_Color_A_2") : materials[i].GetColor("_Color_A_2"),
                        auxPropertyBlock.HasColor("_Color_A_1") ? auxPropertyBlock.GetColor("_Color_A_1") : materials[i].GetColor("_Color_A_1"),
                        (c) => OnChangeColor?.Invoke(auxRenderer, auxMatIndex, "_Color_A_2", c),
                        (c) => OnChangeColor?.Invoke(auxRenderer, auxMatIndex, "_Color_A_1", c)
                    );
                    //customization channel 2
                    item.AddDoubleColor(
                        auxPropertyBlock.HasColor("_Color_B_2") ? auxPropertyBlock.GetColor("_Color_B_2") : materials[i].GetColor("_Color_B_2"),
                        auxPropertyBlock.HasColor("_Color_B_1") ? auxPropertyBlock.GetColor("_Color_B_1") : materials[i].GetColor("_Color_B_1"),
                        (c) => OnChangeColor?.Invoke(auxRenderer, auxMatIndex, "_Color_B_2", c),
                        (c) => OnChangeColor?.Invoke(auxRenderer, auxMatIndex, "_Color_B_1", c)
                    );
                    //customization channel 3
                    item.AddDoubleColor(
                        auxPropertyBlock.HasColor("_Color_C_2") ? auxPropertyBlock.GetColor("_Color_C_2") : materials[i].GetColor("_Color_C_2"),
                        auxPropertyBlock.HasColor("_Color_C_1") ? auxPropertyBlock.GetColor("_Color_C_1") : materials[i].GetColor("_Color_C_1"),
                        (c) => OnChangeColor?.Invoke(auxRenderer, auxMatIndex, "_Color_C_2", c),
                        (c) => OnChangeColor?.Invoke(auxRenderer, auxMatIndex, "_Color_C_1", c)
                    );
                }
                else if (materials[i].HasProperty("_Color"))
                {
                    item.SetupSingleColor(
                        auxPropertyBlock.HasColor("_Color") ? auxPropertyBlock.GetColor("_Color") : materials[i].GetColor("_Color"),
                        (c) => OnChangeColor?.Invoke(auxRenderer, auxMatIndex, "_Color", c));
                }
            }

            for (int i = materials.Count; i < m_MaterialItems.Count; i++)
                m_MaterialItems[i].gameObject.SetActive(false);
            m_ColorPicker.Close();
        }

        private void _OnSwapPrevious()
        {
            m_CurrentItemIndex = m_CurrentItemIndex - 1;
            if (m_CurrentItemIndex < 0)
                m_CurrentItemIndex = m_ItemOptions.Count - 1;
            m_SwapperItem.Text = m_ItemOptions[m_CurrentItemIndex];

            OnChangeItem?.Invoke(CurrentCategory, m_ItemOptions[m_CurrentItemIndex]);
        }

        private void _OnSwapNext()
        {
            m_CurrentItemIndex = (m_CurrentItemIndex + 1) % m_ItemOptions.Count;
            m_SwapperItem.Text = m_ItemOptions[m_CurrentItemIndex];

            OnChangeItem?.Invoke(CurrentCategory, m_ItemOptions[m_CurrentItemIndex]);
        }

        #endregion
    }
}