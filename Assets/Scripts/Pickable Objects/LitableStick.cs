using UnityEngine;


[RequireComponent(typeof(Pickable))]
public class LitableStick : MonoBehaviour
{

    [SerializeField] private bool _isLit = false;
    [SerializeField] private GameObject _flame;
    private Pickable _pickable;
    
    private void Start() 
    {
        _pickable = GetComponent<Pickable>();
    }

    private void Update()
    {
        _flame.SetActive(_isLit);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_pickable.IsBeingCarried && other.collider.GetComponent<LitableStick>() is not null)
            _isLit = true;
    }
}
