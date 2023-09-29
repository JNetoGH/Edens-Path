using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Pickable))]
public class LitableStick : MonoBehaviour
{

    [SerializeField] public bool isLit = false;
    [SerializeField] private GameObject _flame;
    private Pickable _pickable;
    
    private void Start() 
    {
        _pickable = GetComponent<Pickable>();
    }

    private void Update()
    {
        _flame.SetActive(isLit);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_pickable.IsBeingCarried && other.collider.GetComponent<LitableStick>() is not null)
            isLit = true;
    }
}
