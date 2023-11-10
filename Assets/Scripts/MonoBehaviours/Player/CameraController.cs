using UnityEngine;


public class CameraController : MonoBehaviour
{
    
    [Header("Data")]
    [SerializeField] private GameSettingsData _gameSettingsData;
    
    [Header("Rotation")]
    [SerializeField, Range(1, 90)] private float _rotationXRangeUpperLimit = 80f;
    [SerializeField, Range(1, 90)] private float _rotationXRangeLowerLimit = 80f;

    [Header("Movement")]
    [SerializeField] private Transform _cameraPosition;

    public float RotationX { get; set; }
    public float RotationY { get; set; }
    private float _mouseX;
    private float _mouseY;
    
    private void Awake()
    {
        float initialRotationX = transform.rotation.eulerAngles.x;
        float initialRotationY = transform.rotation.eulerAngles.y;
        RotationX = initialRotationX;
        RotationY = initialRotationY;
    }

    private void Update()
    {
        if (!GameManager.CanRotateCamera)
            return;
      
        UpdateInputs();
        RotateFreely();
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
    
    private void RotateFreely()
    {
        float mouseSensitivity = _gameSettingsData.MouseSensitivity; 
        RotationY += _mouseX * mouseSensitivity;
        RotationX -= _mouseY * mouseSensitivity;

        // Keeps the X axis within range 
        RotationX = Mathf.Clamp(RotationX, -_rotationXRangeUpperLimit, _rotationXRangeLowerLimit);

        // Applies the rotations
        transform.rotation = Quaternion.Euler(RotationX, RotationY, 0);
    }
    
    private void MoveToTargetPosition()
    {
        transform.position = _cameraPosition.position;
    }

    public void OverrideRotationCache(float rotationX, float rotationY)
    {
        RotationX = rotationX;
        RotationY = rotationY;
    }
    
}