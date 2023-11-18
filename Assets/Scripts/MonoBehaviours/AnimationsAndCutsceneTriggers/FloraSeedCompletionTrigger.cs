using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;


public class FloraSeedCompletionTrigger : MonoBehaviour
{
    
    [SerializeField, Required] private GameObject _seed;
    [SerializeField] private UnityEvent _onTrigger;
    
    private FloraInteraction1CutsceneController _floraInteraction1CutsceneController;
    
    private void OnTriggerEnter(Collider other)
    {
  
        if (other.gameObject != _seed)
            return;
        
        _onTrigger.Invoke();
    }
}
