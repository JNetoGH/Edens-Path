using UnityEngine;
using UnityEngine.Serialization;

public class CameraTranslate : MonoBehaviour
{
    [SerializeField] private Transform _target;         // The point to translate around
    [SerializeField] private float _radius = 5f;        // The radius of the translation circle
    [SerializeField] private float _speed = 2f;         // The translation speed
    [SerializeField] private float _heightOffset = 1f;  // The offset in height from the target
    
    private void Update()
    {
        // Calculate the desired position on the translation circle
        float angle = Time.time * _speed;
        Vector3 desiredPosition = _target.position + new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * _radius;
        desiredPosition.y = _target.position.y + _heightOffset;

        // Update the camera's position to the desired position
        transform.position = desiredPosition;

        // Make the camera always look at the target
        transform.LookAt(_target);
    }
}