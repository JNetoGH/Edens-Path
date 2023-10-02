using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(PickableObject))]
public class LitableStick : MonoBehaviour
{

    [SerializeField] public bool isLit = false;
    [SerializeField] private GameObject _flame;
    private PickableObject _pickableObject;
    
    private void Start() 
    {
        _pickableObject = GetComponent<PickableObject>();
    }

    private void Update()
    {
        _flame.SetActive(isLit);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_pickableObject.IsBeingCarried && other.collider.GetComponent<LitableStick>() is not null)
            isLit = true;
    }
}
