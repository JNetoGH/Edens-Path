using System;
using UnityEngine;

public class Pickable : MonoBehaviour
{

    public bool IsBeingCarried { get; set; } = false;
    
    [SerializeField] private Renderer _renderer;
    private Material _originalMaterial;
    private float _maxAngularSpeed = 3;
    
    private void Start()
    {
        IsBeingCarried = false;
        _originalMaterial = _renderer.material;
    }

    private void Update()
    {
        
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
