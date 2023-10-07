using UnityEngine;


public class CameraController : MonoBehaviour
{
    
    [Header("Data")]
    [SerializeField] private GameSettingsData _gameSettingsData;
    
    [Header("Rotation")]
    [SerializeField, Range(1, 90)] private float _rotationXRangeUpperLimit = 80f;
    [SerializeField, Range(1, 90)] private float _rotationXRangeLowerLimit = 80f;
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
        float mouseSensitivity = _gameSettingsData.MouseSensitivity; 
        _rotationY += _mouseX * mouseSensitivity;
        _rotationX -= _mouseY * mouseSensitivity;

        // Keeps the X axis within range 
        _rotationX = Mathf.Clamp(_rotationX, -_rotationXRangeUpperLimit, _rotationXRangeLowerLimit);

        // Applies the rotations
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
    }

    private void MoveToTargetPosition()
    {
        transform.position = _cameraPosition.position;
    }
    
}