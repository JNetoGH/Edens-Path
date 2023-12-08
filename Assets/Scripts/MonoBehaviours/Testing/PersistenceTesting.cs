using UnityEngine;

public class PersistenceTesting : MonoBehaviour
{
    
    private int _value = 3;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            _value = 5;
        
        // Debug.Log("Test Value: " + _value);
    }
}
