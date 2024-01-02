using UnityEngine;
using UnityEngine.Events;


public class FloraSeedCompletionTrigger : MonoBehaviour
{
    
    [SerializeField] private UnityEvent _onTrigger;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Seed"))
            return;
        
        _onTrigger.Invoke();
    }
}
