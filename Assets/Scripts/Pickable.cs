using UnityEngine;

public class Pickable : MonoBehaviour
{

    private Material _originalMaterial;
    private Renderer _renderer;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;
    }
    
    public void ResetToOriginalMaterial()
    {
        _renderer.material = _originalMaterial;
    }
}
