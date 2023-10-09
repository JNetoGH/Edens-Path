using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningTreeColision : MonoBehaviour
{

    private bool _hasTriggeredAlready;

    private void Start()
    {
        _hasTriggeredAlready = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasTriggeredAlready)
            return;

        if (collision.gameObject.tag.Equals("Torch"))
        {
            Level1Manager.Instance.TriggerBurningTreeCutscene();
            _hasTriggeredAlready = true;
        }
    }
}
