using UnityEngine;

namespace ScriptableObjects
{

    [CreateAssetMenu(fileName = "CombiningRecipeData", menuName = "Scriptable Objects/Combining Recipe Data", order = 0)]
    public class CombiningRecipeData : ScriptableObject
    {
        public PickableObjectData obj1;
        public PickableObjectData obj2;
        public PickableObjectData result;
    }
}