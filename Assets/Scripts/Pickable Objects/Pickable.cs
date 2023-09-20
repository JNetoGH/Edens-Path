using UnityEngine;

public class Pickable : MonoBehaviour
{

    public bool IsBeingCarried { get; set; } = false;
    
    [SerializeField] private Renderer _renderer;
    private Material _originalMaterial;
    
    private void Start()
    {
        IsBeingCarried = false;
        _originalMaterial = _renderer.material;
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
