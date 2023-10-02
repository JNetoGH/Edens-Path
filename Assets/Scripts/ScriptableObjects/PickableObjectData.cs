using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Pickable Object Data", menuName = "Scriptable Objects/Pickable Object Data", order = 1)]
    public class PickableObjectData : ScriptableObject
    {
        public int id;
        public string itemName;
        public Sprite icon;
        public GameObject correspondingPickablePrefab;
    }
    
}