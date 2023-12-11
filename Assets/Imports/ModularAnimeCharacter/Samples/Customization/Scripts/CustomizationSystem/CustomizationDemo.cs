using Rukha93.ModularAnimeCharacter.Customization.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Rukha93.ModularAnimeCharacter.Customization
{
    public class CustomizationDemo : MonoBehaviour
    {
        public class EquipedItem
        {
            public string path;
            public List<GameObject> instantiatedObjects;
            public CustomizationItemAsset assetReference;

            //for material customization
            public Renderer[] renderers;
        }

        [SerializeField] private UICustomizationDemo m_UI;

        private IAssetLoader m_AssetLoader;
        private List<string> m_Categories = new List<string>
        {
            "body",
            "head",
            "hairstyle",
            "top",
            "bottom",
            "shoes",
            "outfit"
        };

        private Animator m_Character;
        private SkinnedMeshRenderer m_ReferenceMesh;

        private Dictionary<string, List<string>> m_CustomizationOptions; //<categoryId, assetPath[]>

        private Dictionary<BodyPartType, BodyPartTag> m_BodyParts;
        private Dictionary<string, EquipedItem> m_Equiped;
        private Dictionary<Material, MaterialPropertyBlock> m_MaterialProperties;

        private Coroutine m_LoadingCoroutine;

        private void Awake()
        {
            //init variables
            m_AssetLoader = GetComponentInChildren<IAssetLoader>();
            m_CustomizationOptions = new Dictionary<string, List<string>>();
            m_Equiped = new Dictionary<string, EquipedItem>();
            m_BodyParts = new Dictionary<BodyPartType, BodyPartTag>();
            m_MaterialProperties = new Dictionary<Material, MaterialPropertyBlock>();

            //ui callbacks
            m_UI.OnClickCategory += OnSelectCategory;
            m_UI.OnChangeItem += OnSwapItem;
            m_UI.OnChangeColor += OnChangeColor;
        }

        private void Start()
        {
            //init categories UI
            m_UI.SetCategories(m_Categories.ToArray());
            for (int i = 0; i < m_Categories.Count; i++)
                m_UI.SetCategoryValue(i, "");

            m_LoadingCoroutine = StartCoroutine(Co_LoadAndInitBody("f"));
        }

        private void InitBody(string path, GameObject prefab)
        {
            //instantiate the body prefab and store the animator
            m_Character = Instantiate(prefab, this.transform).GetComponent<Animator>();

            //get a random body mesh to be used as reference
            var meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
            m_ReferenceMesh = meshes[meshes.Length / 2];

            //initialize all tagged body parts
            //they be used to disable meshes that are hidden by clothes
            var bodyparts = m_Character.GetComponentsInChildren<BodyPartTag>();
            foreach (var part in bodyparts)
                m_BodyParts[part.type] = part;

            var equip = new EquipedItem()
            {
                path = path,
                assetReference = null,
                instantiatedObjects = new List<GameObject>() { m_Character.gameObject }
            };
            InitRenderersForItem(equip);
            m_Equiped["body"] = equip;

            //update ui
            m_UI.SetCategoryValue(m_Categories.IndexOf("body"), path);
            if (m_UI.IsCustomizationOpen && m_UI.CurrentCategory == "body")
                m_UI.SetCustomizationMaterials(equip.renderers);
        }

        private IEnumerator Co_LoadAndInitBody(string bodyType)
        {
            //destroy old character
            if (m_Character != null)
            {
                Destroy(m_Character.gameObject);

                //clear old items
                m_Equiped.Clear();
                m_CustomizationOptions.Clear();
                m_BodyParts.Clear();
            }

            //init the customization options for the selected body type
            List<Coroutine> coroutines = new List<Coroutine>();
            for (int i = 0; i < m_Categories.Count; i++)
            {
                int index = i;
                string path = m_Categories[i].Equals("body") ? "body" : GetAssetPath(bodyType, m_Categories[i]);
                coroutines.Add(StartCoroutine(m_AssetLoader.LoadAssetList(path, res => m_CustomizationOptions[m_Categories[index]] = new List<string>(res))));
            }
            for (int i = 0; i < m_Categories.Count; i++)
            {
                yield return coroutines[i];

                //add an empty item for all categories that can be empty
                if (m_Categories[i].Equals("body"))
                    continue;
                if (m_Categories[i].Equals("head"))
                    continue;
                m_CustomizationOptions[m_Categories[i]].Insert(0, "");
            }

            //initialize the body
            var bodyPath = GetAssetPath(bodyType, "body");
            yield return m_AssetLoader.LoadAsset<GameObject>(bodyPath, res => InitBody(bodyPath, res));

            //initialize the head with the first available
            string assetPath = m_CustomizationOptions["head"][0];
            yield return m_AssetLoader.LoadAsset<CustomizationItemAsset>(assetPath, res => Equip("head", assetPath, res));

            m_LoadingCoroutine = null;
        }

        #region EQUIPMENT

        public void Equip(string cat, string path, CustomizationItemAsset item)
        {
            //if outfit, remove all othet pieces
            if(cat.Equals("outfit"))
            {
                Unequip("top", false);
                Unequip("bottom", false);
            }
            else if (cat.Equals("top") || cat.Equals("bottom"))
            {
                Unequip("outfit", false);
            }

            //unequip previous item
            Unequip(cat, false);

            EquipedItem equip = new EquipedItem()
            {
                path = path,
                assetReference = item,
                instantiatedObjects = new List<GameObject>()
            };
            m_Equiped[cat] = equip;

            //instantiate new meshes, init properties, parent to character
            GameObject go = null;
            SkinnedMeshRenderer skinnedMesh = null;
            foreach(var mesh in item.meshes)
            {
                //instantiate new gameobject
                go = new GameObject(mesh.name);
                go.transform.SetParent(m_Character.transform, false);
                m_Equiped[cat].instantiatedObjects.Add(go);

                //add the renderer
                skinnedMesh = go.AddComponent<SkinnedMeshRenderer>();
                skinnedMesh.rootBone = m_ReferenceMesh.rootBone;
                skinnedMesh.bones = m_ReferenceMesh.bones;
                skinnedMesh.bounds = m_ReferenceMesh.bounds;
                skinnedMesh.sharedMesh = mesh.sharedMesh;
                skinnedMesh.sharedMaterials = mesh.sharedMaterials;
            }

            //instantiate objects, parent to target bones
            foreach(var obj in item.objects)
            {
                go = Instantiate(obj.prefab, m_Character.GetBoneTransform(obj.targetBone));
                equip.instantiatedObjects.Add(go);
            }

            //update bodyparts
            UpdateBodyRenderers();

            //map renderers
            InitRenderersForItem(equip);

            //update ui
            m_UI.SetCategoryValue(m_Categories.IndexOf(cat), path);
            if (m_UI.IsCustomizationOpen && m_UI.CurrentCategory == cat)
                m_UI.SetCustomizationMaterials(equip.renderers);

            //send message to the character
            //used to update the facial blendshape controller and colliders for hair
            m_Character.SendMessage("OnChangeEquip", new object[] { cat, equip.instantiatedObjects }, SendMessageOptions.DontRequireReceiver);
        }

        private IEnumerator Co_LoadAndEquip(string cat, string path)
        {
            yield return m_AssetLoader.LoadAsset<CustomizationItemAsset>(path, res => Equip(cat, path, res));
            m_LoadingCoroutine = null;
        }

        public void Unequip(string category, bool updateRenderers = true)
        {
            if (m_Equiped.ContainsKey(category) == false)
                return;

            var item = m_Equiped[category];
            m_Equiped.Remove(category);

            //destroy instances
            foreach (var go in item.instantiatedObjects)
                Destroy(go);

            //update body parts
            if (updateRenderers)
                UpdateBodyRenderers();

            //update UI
            m_UI.SetCategoryValue(m_Categories.IndexOf(category), "");
            if (m_UI.IsCustomizationOpen)
                m_UI.SetCustomizationMaterials(null);
        }

        public void UpdateBodyRenderers()
        {
            List<BodyPartType> disabled = new List<BodyPartType>();

            //get all parts that are hidden by equips
            foreach (var equip in m_Equiped.Values)
            {
                if (equip.assetReference == null)
                    continue;

                foreach (var part in equip.assetReference.bodyParts)
                    if (!disabled.Contains(part))
                        disabled.Add(part);
            }

            //set active value of each part
            foreach (var part in m_BodyParts)
                part.Value.gameObject.SetActive(!disabled.Contains(part.Key));

            //todo: maybe move this offset to the CharacterController's center offset or skin width
            var localPos = m_Character.transform.localPosition;
            localPos.y = m_Equiped.ContainsKey("shoes") ? 0.02f : 0;
            m_Character.transform.localPosition = localPos;
        }

        private void SyncMaterialChange(Material sharedMaterial, MaterialPropertyBlock newProperties)
        {
            //apply the new properties to all renderers sharing the same material
            foreach (var equip in m_Equiped.Values)
            {
                foreach (var renderer in equip.renderers)
                {
                    for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                    {
                        if (renderer.sharedMaterials[i] != sharedMaterial)
                            continue;
                        renderer.SetPropertyBlock(newProperties, i);
                    }
                }
            }
        }

        private void SyncNewItemMaterials(Renderer renderer)
        {
            //update the new renderers material with the stored properties
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                if (m_MaterialProperties.ContainsKey(renderer.sharedMaterials[i]) == false)
                    continue;
                renderer.SetPropertyBlock(m_MaterialProperties[renderer.sharedMaterials[i]], i);
            }
        }

        #endregion
        private void InitRenderersForItem(EquipedItem item)
        {
            List<Renderer> renderers = new List<Renderer>();
            List<MaterialPropertyBlock> props = new List<MaterialPropertyBlock>();

            //get all materials in the instantiated items
            foreach (var obj in item.instantiatedObjects)
                renderers.AddRange(obj.GetComponentsInChildren<Renderer>());

            item.renderers = renderers.ToArray();

            //update the material properties for the new item
            foreach (var renderer in item.renderers)
                SyncNewItemMaterials(renderer);
        }

        #region HELPERS

        public string GetAssetPath(string bodyType, string asset)
        {
            return $"{bodyType}/{asset}".ToLower();
        }

        public string GetAssetPath(string bodyType, string category, string asset)
        {
            return $"{bodyType}/{category}/{asset}".ToLower();
        }

        #endregion

        #region UI CALLBACKS
        
        private void OnSelectCategory(string cat)
        {
            if (string.Equals(m_UI.CurrentCategory, cat))
            {
                m_UI.ShowCustomization(false);
                return;
            }

            //init items
            m_UI.SetCustomizationOptions(cat, m_CustomizationOptions[cat].ToArray(), m_Equiped.ContainsKey(cat) ? m_Equiped[cat].path : "");

            //set material options
            if (m_Equiped.ContainsKey(cat))
                m_UI.SetCustomizationMaterials(m_Equiped[cat].renderers);
            else
                m_UI.SetCustomizationMaterials(null);

            //show UI
            m_UI.ShowCustomization(true);
        }

        private void OnSwapItem(string cat, string asset)
        {
            //if empty, just unequip the current one if any
            if (string.IsNullOrEmpty(asset))
            {
                Unequip(cat);
                return;
            }

            //stop loading previous item
            if (m_LoadingCoroutine != null)
                StopCoroutine(m_LoadingCoroutine);

            //load new item
            if (cat.Equals("body"))
                m_LoadingCoroutine = StartCoroutine(Co_LoadAndInitBody(asset.StartsWith("m") ? "m" : "f"));
            else
                m_LoadingCoroutine = StartCoroutine(Co_LoadAndEquip(cat, asset));
        }

        private void OnChangeColor(Renderer renderer, int materialIndex, string property, Color color)
        {
            //update the renderer material property
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialIndex);

            block.SetColor(property, color);
            renderer.SetPropertyBlock(block, materialIndex);

            //store the properties for this material in case a new equip needs it
            var sharedMaterial = renderer.sharedMaterials[materialIndex];
            m_MaterialProperties[sharedMaterial] = block;

            //update the property block of each renderer sharing the same material
            SyncMaterialChange(sharedMaterial, block);
        }

        #endregion
    }
}