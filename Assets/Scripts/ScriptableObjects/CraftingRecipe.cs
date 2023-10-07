using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Combining Recipe", menuName = "Scriptable Objects/Combining Recipe", order = 0)]
    public class CraftingRecipe : ScriptableObject
    {
        public PickableObjectData obj1;
        public PickableObjectData obj2;
        public PickableObjectData result;
    }
}