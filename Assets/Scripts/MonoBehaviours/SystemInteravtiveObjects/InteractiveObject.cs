using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Represent an object with events attached, these will be invoked once the ObjectInteractorSystem.cs finds them.
/// </summary>
[RequireComponent(typeof(Outline))]
public class InteractiveObject : MonoBehaviour
{
    
    public UnityEvent events = new UnityEvent();
    public Outline OutlineScript { get; private set; }
    
    private void Awake()
    {
        OutlineScript = GetComponent<Outline>();
        OutlineScript.enabled = false;
    }
}

