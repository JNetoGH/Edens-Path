using UnityEngine;

/// <summary>
/// This script rotates the GameObject around its y-axis at a specified speed.
/// </summary>
public class PlaygroundSpinningToyAnimation : MonoBehaviour
{

    [SerializeField] private float _rotationSpeed = 10f;
    
    void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y -= _rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
