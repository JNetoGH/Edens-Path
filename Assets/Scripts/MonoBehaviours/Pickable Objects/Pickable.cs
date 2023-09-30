using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour
{

    public bool IsBeingCarried { get; set; } = false;
    public bool IsBeingHitByPickUpRay { get; set; } = false;
    public bool IsColliding { get; private set; } = false;
    
    public Outline OutlineScript => _outlineScript;
    [SerializeField] private Outline _outlineScript;
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
        // When being picked up:
        // - Disables collision with the player.
        // - Sets a limit to its angular velocity so it doesn't go crazy under player's control 
        // - Disables Gravity
        if (IsBeingCarried)
        {
            gameObject.layer = LayerMask.NameToLayer("DontCollideWithPlayer");
            if (IsColliding)
                _rigidbody.maxAngularVelocity = 3;
            else
                _rigidbody.maxAngularVelocity = 7;
            _rigidbody.useGravity = false;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            _rigidbody.maxAngularVelocity = 7;
            _rigidbody.useGravity = true;
        }

        // Syncs the outline for both either being carried or hit by the camera's ray
        _outlineScript.enabled = IsBeingHitByPickUpRay || IsBeingCarried;
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
