using Cinemachine;
using UnityEngine;

/// <summary>
/// Teleports Pickable Objects back to a point if the get out of the trigger.
/// </summary>
public class PickableObjectPresenceChecker : MonoBehaviour
{

    [Header("Required Components")]
    [SerializeField] private Transform _transportationPoint;
    [SerializeField, Range(0f, 5f)] private float _exitTime = 2f;
    private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnTriggerExit(Collider other)
    {
        PickableObject pickableObject = other.GetComponent<PickableObject>();
        if (pickableObject == null)
            return;

        Debug.Log($"{pickableObject.name} has exited the checker");
        StartCinemachine();
        TransportPickableObject(pickableObject);
    }
    
    /// <summary>
    /// Transports a pickable object to the _transportationPoint.
    /// </summary>
    /// <param name="obj">The pickable object to be transported</param>
    private void TransportPickableObject(PickableObject obj)
    {
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.transform.position = _transportationPoint.position;
    }
    
    private void StartCinemachine()
    {
        GameManager.EnterCutsceneMode();
        _virtualCamera.enabled = true;
        FindObjectOfType<CinemachineBrain>(includeInactive: true).gameObject.SetActive(true);
        Invoke(nameof(EndCinemachine), _exitTime);
    }
    
    private void EndCinemachine()
    {
        GameManager.EnterGameplayMode();
        _virtualCamera.enabled = false;
        FindObjectOfType<CinemachineBrain>(includeInactive: true).gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Draw a wire sphere representing the trigger in the Scene view
    /// </summary>
    private void OnDrawGizmos()
    {
        // Draws a wire sphere representing the interaction range in the Scene view
        float radius = GetComponent<SphereCollider>().radius;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
        
        // Draws a solid cyan dot on top of the wire sphere to represent the transportation point.
        Vector3 dotPosition = _transportationPoint.position;
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(dotPosition, 0.25f); // 0.25f is the radius of the solid sphere
    }

}
