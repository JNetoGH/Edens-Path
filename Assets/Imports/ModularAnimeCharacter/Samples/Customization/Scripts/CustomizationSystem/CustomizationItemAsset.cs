using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.ModularAnimeCharacter.Customization
{
    [CreateAssetMenu(fileName = "CustomizationItemAsset", menuName = "Rukha93/ModularAnimeCharacter/Create CustomizationAsset", order = 1)]
    public class CustomizationItemAsset : ScriptableObject
    {
        [System.Serializable]
        public struct Parentable
        {
            public GameObject prefab;
            public HumanBodyBones targetBone;
        }

        [Space]
        public BodyPartType[] bodyParts;

        [Space]
        public SkinnedMeshRenderer[] meshes;
        public Parentable[] objects;
    }
}