using UnityEngine;

public class PersistenceTesting : MonoBehaviour
{

    private bool _runTest = false;
    private int _value = 3;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            _value = 5;
        
        if (_runTest)
            Debug.Log("Test Value: " + _value);
    }
}
