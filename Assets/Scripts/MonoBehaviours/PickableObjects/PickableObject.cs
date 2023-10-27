using System.Collections.Generic;
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
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
    
    private List<Material> _originalMaterials = new List<Material>();
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        IsBeingCarried = false;
        if (_renderers.Count == 0)
            Debug.LogWarning($"tried to cache the default materials in PickableObject ({gameObject.name}), but there are no Renderes set");
        else 
            _renderers.ForEach(r => _originalMaterials.Add(r.material));
    }

    private void Update()
    {
        // When being picked up:
        // - Disables collision with the player.
        // - Sets a higher limit to its angular velocity if colliding so it doesn't get wierd under player's control
        //   dragging it around over surfaces. 
        // - Disables Gravity
        // - Applies a slippery PhysicMaterial
        if (IsBeingCarried)
        {
            gameObject.layer = LayerMask.NameToLayer("DontCollideWithPlayer");
            if (IsColliding)
                _rigidbody.maxAngularVelocity = 3;
            else
                _rigidbody.maxAngularVelocity = 7;
            _rigidbody.useGravity = false;
            
            foreach (Collider component in _rigidbody.GetComponents<Collider>())
                component.material = GameManager.SlipperyMaterial;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            _rigidbody.maxAngularVelocity = 7;
            _rigidbody.useGravity = true;
            
            foreach (Collider component in _rigidbody.GetComponents<Collider>())
                component.material = null;
        }
        // Syncs the outline for both either being carried or hit by the camera's ray
        _outlineScript.enabled = IsBeingHitByPickUpRay || IsBeingCarried;
    }
    
    private void OnCollisionStay(Collision other)
    {
        IsColliding = true;
    }

    private void OnCollisionExit(Collision other)
    {
        IsColliding = false;
    }
    
    public void SetMaterial(Material material)
    {
        if (_renderers.Count == 0)
            Debug.LogWarning($"tried to set materials in PickableObject {gameObject.name}, but there are no renderes set");
        else
            _renderers.ForEach(r => r.material = material);
    }
    
    public void ResetToOriginalMaterial()
    {
        if (_renderers.Count == 0)
            Debug.LogWarning($"tried to set material in PickableObject {gameObject.name}, but there are no renderes set");
        else
            for (int i = 0; i < _renderers.Count; i++)
                _renderers[i].material = _originalMaterials[i];
    }
    
}