using ScriptableObjects;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    
    public bool IsBeingCarried { get; set; } = false;
    public bool IsBeingHitByPickUpRay { get; set; } = false;
    public bool IsColliding { get; private set; } = false;
 
    [Header("Data")]
    public PickableObjectData pickableObjectData;
    
    [Header("Rendering")]
    [SerializeField] private Outline _outlineScript;
    public Outline OutlineScript => _outlineScript;
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
        if (_renderer != null) _originalMaterial = _renderer.material;
        else Debug.LogWarning($"tried to cache the default material in PickableObject {gameObject.name}, but there is no renderer set");
    }

    private void Update()
    {
        // When being picked up:
        // - Disables collision with the player.
        // - Sets a higher limit to its angular velocity if colliding so it doesn't get wierd under player's control
        //   dragging it around over surfaces. 
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
        if (_renderer != null) _renderer.material = material;
        else Debug.LogWarning($"tried to set material in PickableObject {gameObject.name}, but there is no renderer set");
    }
    
    public void ResetToOriginalMaterial()
    {
        if (_renderer != null) _renderer.material = _originalMaterial;
        else Debug.LogWarning($"tried to set material in PickableObject {gameObject.name}, but there is no renderer set");
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
