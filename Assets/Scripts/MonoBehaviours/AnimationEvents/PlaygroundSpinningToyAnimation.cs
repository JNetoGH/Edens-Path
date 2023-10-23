using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundSpinningToyAnimation : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 1.2f;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y -= rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
