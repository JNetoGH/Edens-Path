using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour
{

    public bool IsBeingCarried { get; set; } = false;
    public bool IsColliding { get; private set; } = false;
    [SerializeField] private Renderer _renderer;
    
    private Material _originalMaterial;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        IsBeingCarried = false;
        _originalMaterial = _renderer.material;
    }

    private void Update()
    {
        // Syncs the gravity use
        _rigidbody.useGravity = !IsBeingCarried;
        
        // Disables collision with the player, when being picked up
        if (IsBeingCarried) this.gameObject.layer = LayerMask.NameToLayer("DontCollideWithPlayer");
        else this.gameObject.layer = LayerMask.NameToLayer("Default");

        if (GetComponent<LitableStick>() is not null)
        {
            Debug.Log(_rigidbody.angularVelocity.magnitude);
        }
    }

    public void SetMaterial(Material material)
    {
        _renderer.material = material;
    }
    
    public void ResetToOriginalMaterial()
    {
        _renderer.material = _originalMaterial;
    }

    private void OnCollisionStay(Collision other)
    {
        IsColliding = true;
    }

    private void OnCollisionExit(Collision other)
    {
        IsColliding = false;
    }
    
}
