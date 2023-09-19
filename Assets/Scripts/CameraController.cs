using UnityEngine;


public class CameraController : MonoBehaviour
{
    
    // Encapsulation
    public float MouseSensitivity
    {
        get => _mouseSensitivity; 
        set => _mouseSensitivity = value <= 10 ? value : 10;
    }
    
    [Header("Rotation")]
    [SerializeField, Range(1, 10)] private float _mouseSensitivity = 1f;
    [SerializeField, Range(1, 99)] private float _rotationXRange = 80f;
    private float _rotationX;
    private float _rotationY;
    
    [Header("Movement")]
    [SerializeField] private Transform _cameraPosition;
    private float _mouseX;
    private float _mouseY;
    
    private void Update()
    {
        if (!PlayerController.CanMove)
            return;
        UpdateInputs();
        Rotate();
    }

    private void LateUpdate()
    {
        MoveToTargetPosition();
    }

    private void UpdateInputs()
    {
        _mouseX = Input.GetAxisRaw("Mouse X");
        _mouseY = Input.GetAxisRaw("Mouse Y");
    }
    
    private void Rotate()
    {
        _rotationY += _mouseX * _mouseSensitivity;
        _rotationX -= _mouseY * _mouseSensitivity;

        // Keeps the X axis within range 
        _rotationX = Mathf.Clamp(_rotationX, -_rotationXRange, _rotationXRange);

        // Applies the rotations
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
    }

    private void MoveToTargetPosition()
    {
        transform.position = _cameraPosition.position;
    }
    
}