using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Rukha93.ModularAnimeCharacter.Customization
{
    /// <summary>
    /// Fake asset loader that simulates an addressable loader
    /// </summary>
    public class FakeLoader : MonoBehaviour, IAssetLoader
    {
        [System.Serializable]
        public class ItemGroup
        {
            public GameObject m_BaseBody;
            public CustomizationItemAsset[] m_Heads;
            public CustomizationItemAsset[] m_Hairstyles;
            public CustomizationItemAsset[] m_Tops;
            public CustomizationItemAsset[] m_Bottoms;
            public CustomizationItemAsset[] m_Shoes;
            public CustomizationItemAsset[] m_Outfits;
        }

        [SerializeField] private float m_FakeLoadTime = 1f;

        [Space]
        [SerializeField] private ItemGroup m_MaleItems;
        [SerializeField] private ItemGroup m_FemaleItems;

        private Dictionary<string, object> m_AssetMap;

        private void Awake()
        {
            m_AssetMap = new Dictionary<string, object>();

            AddAssets("m", m_MaleItems);
            AddAssets("f", m_FemaleItems);

            string debug = "fake asset loader initialized";
            foreach(var pair in m_AssetMap)
                debug += "\n" + pair.Key + "\t" + pair.Value;
            Debug.Log(debug);
        }

        private void AddAssets(string prefix, ItemGroup group)
        {
            //create ids for each asset, simulating an addressable path
            m_AssetMap.Add($"{prefix}/body", group.m_BaseBody);

            for (int i = 0; i < group.m_Heads.Length; i++)
                m_AssetMap.Add($"{prefix}/head/{group.m_Heads[i].name}", group.m_Heads[i]);

            for (int i = 0; i < group.m_Hairstyles.Length; i++)
                m_AssetMap.Add($"{prefix}/hairstyle/{group.m_Hairstyles[i].name}", group.m_Hairstyles[i]);

            for (int i = 0; i < group.m_Tops.Length; i++)
                m_AssetMap.Add($"{prefix}/top/{group.m_Tops[i].name}", group.m_Tops[i]);

            for (int i = 0; i < group.m_Bottoms.Length; i++)
                m_AssetMap.Add($"{prefix}/bottom/{group.m_Bottoms[i].name}", group.m_Bottoms[i]);

            for (int i = 0; i < group.m_Shoes.Length; i++)
                m_AssetMap.Add($"{prefix}/shoes/{group.m_Shoes[i].name}", group.m_Shoes[i]);

            for (int i = 0; i < group.m_Outfits.Length; i++)
                m_AssetMap.Add($"{prefix}/outfit/{group.m_Outfits[i].name}", group.m_Outfits[i]);
        }

        public IEnumerator LoadAssetList(string path, System.Action<string[]> onLoaded)
        {
            //create ids for each asset, simulating an addressable path
            List<string> result = new List<string>();

            if (path.Equals("body"))
            {
                if (m_MaleItems.m_BaseBody)
                    result.Add("m/body");
                if (m_FemaleItems.m_BaseBody)
                    result.Add("f/body");
                onLoaded?.Invoke(result.ToArray());
                yield break;
            }

            ItemGroup group = null;
            string prefix = null;

            if (path.StartsWith("m/"))
            {
                group = m_MaleItems;
                prefix = "m";
            }
            else if (path.StartsWith("f/"))
            {
                group = m_FemaleItems;
                prefix = "f";
            }

            if (group != null)
            {
                if (path.Contains("/head"))
                    for (int i = 0; i < group.m_Heads.Length; i++)
                        result.Add($"{prefix}/head/{group.m_Heads[i].name}");

                if (path.Contains("/hairstyle"))
                    for (int i = 0; i < group.m_Hairstyles.Length; i++)
                        result.Add($"{prefix}/hairstyle/{group.m_Hairstyles[i].name}");

                if (path.Contains("/top"))
                    for (int i = 0; i < group.m_Tops.Length; i++)
                        result.Add($"{prefix}/top/{group.m_Tops[i].name}");

                if (path.Contains("/bottom"))
                    for (int i = 0; i < group.m_Bottoms.Length; i++)
                        result.Add($"{prefix}/bottom/{group.m_Bottoms[i].name}");

                if (path.Contains("/shoes"))
                    for (int i = 0; i < group.m_Shoes.Length; i++)
                        result.Add($"{prefix}/shoes/{group.m_Shoes[i].name}");

                if (path.Contains("/outfit"))
                    for (int i = 0; i < group.m_Outfits.Length; i++)
                        result.Add($"{prefix}/outfit/{group.m_Outfits[i].name}");
            }

            //fake load time to simulate a real asset loader
            if (m_FakeLoadTime > 0)
                yield return new WaitForSeconds(Random.Range(0f, m_FakeLoadTime));
            onLoaded?.Invoke(result.ToArray());
        }

        public IEnumerator LoadAsset<T>(string path, System.Action<T> onLoaded)
        {
            //Debug.Log("load assset " + path + " as " + typeof(T));

            T result = m_AssetMap.ContainsKey(path) && m_AssetMap[path] is T ? (T)m_AssetMap[path] : default(T);

            //fake load time
            if (m_FakeLoadTime > 0)
                yield return new WaitForSeconds(Random.Range(0f, m_FakeLoadTime));
            onLoaded?.Invoke(result);
        }
    }
}