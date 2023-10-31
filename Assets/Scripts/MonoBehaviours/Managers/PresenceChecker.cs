using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresenceChecker : MonoBehaviour
{
    
    [SerializeField] private List<Transform> _tracked;
    private Transform _toBeTeleported;

    private void OnTriggerExit(Collider other)
    {
        foreach (Transform t in _tracked)
        {
            if (t == other.gameObject.transform)
            {
                other.gameObject.transform.position = this.transform.position;
                if (t.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody.velocity = Vector3.zero;
                }
            }   
        }
    }
}
