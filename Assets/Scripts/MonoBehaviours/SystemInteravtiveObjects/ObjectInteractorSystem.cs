using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// Must be attached to the player's first person camera
/// </summary>
public class ObjectInteractorSystem : MonoBehaviour
{
    
    /// <summary>
    /// Length of the ray that will be casted from the player's camera.
    /// </summary>
    [SerializeField] private float _rayLength = 3;

    /// <summary>
    /// Displayed on editor as Read-Only for debugging;
    /// </summary>
    [SerializeField, ReadOnly] private InteractiveObject _targetedInteractiveObject; 
    
    private void Start()
    {
        _targetedInteractiveObject = null;
    }

    private void Update()
    {
        // Creates a ray from the camera into the scene trying to find a Interactive Object
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayLength))
        {
            InteractiveObject newTarget = hit.collider.GetComponent<InteractiveObject>();
            
            if (newTarget != _targetedInteractiveObject)
            {
                TrySetOutLineOfCurrentTarget(false);
                _targetedInteractiveObject = newTarget;
                TrySetOutLineOfCurrentTarget(true);
            }
            
        }
        else
        {
            TrySetOutLineOfCurrentTarget(false);
            _targetedInteractiveObject = null;
            return;
        }

        // Checks for input
        if (Input.GetButtonDown("Fire1") && _targetedInteractiveObject is not null)
            _targetedInteractiveObject.events.Invoke();
    }

    private void TrySetOutLineOfCurrentTarget(bool onOff)
    {
        if (_targetedInteractiveObject is not null)
            _targetedInteractiveObject.OutlineScript.enabled = onOff;
    }
    
    
}
