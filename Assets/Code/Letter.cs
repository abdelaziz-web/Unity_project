using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }


    void Update()
    {
           /* if (mainCamera != null)
    {
        // Calculate the direction from the letter to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Set the y component of the direction to zero to keep the letter upright
        directionToCamera.y = 0;

        // Rotate the letter to face the direction towards the camera
        transform.rotation = Quaternion.LookRotation(directionToCamera.normalized, Vector3.up);
    } */
    }
}
