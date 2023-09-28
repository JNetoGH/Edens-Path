using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickable : MonoBehaviour
{

    public bool IsBeingCarried { get; set; } = false;
    private Material _originalMaterial;
    [SerializeField] private Renderer _renderer;
    
    private void Start()
    {
        IsBeingCarried = false;
        _originalMaterial = _renderer.material;
    }

    private void Update()
    {
        if (IsBeingCarried)
            this.gameObject.layer = LayerMask.NameToLayer("DontCollideWithPlayer");
        else
            this.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void SetMaterial(Material material)
    {
        _renderer.material = material;
    }
    
    public void ResetToOriginalMaterial()
    {
        _renderer.material = _originalMaterial;
    }
    
}
