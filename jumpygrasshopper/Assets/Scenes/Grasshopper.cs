using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasshopper : MonoBehaviour
{
    public float launchForce = 10f; // The force to apply when launching the Grasshopper
    public float rotationSpeed = 30f; // The rotation speed in degrees per second
    private Rigidbody2D rb; // Rigidbody2D reference
    private bool hasBeenLaunched = false;
    private float currentRotation = 280f; // Track the current rotation angle
    private bool rotateClockwise = true; // Indicates whether to rotate clockwise or counterclockwise
    private float highAngle = 359f;
    private float lowAngle = 295f;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component on this GameObject
        rb.isKinematic = true; // Start with kinematic set to true
    }

    private void Update()
    {
        if (!hasBeenLaunched)
        {
            RotateGrasshopper();
        }

        if (!hasBeenLaunched && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchGrasshopper();
        }
    }

    void RotateGrasshopper()
    {
        // Rotate the object in place before launching
        float rotationDirection = rotateClockwise ? 1f : -1f;
        float newRotation = currentRotation + (rotationDirection * rotationSpeed * Time.deltaTime);

        // Check if we have reached the angle limits and change direction
        if (rotateClockwise && newRotation >= highAngle)
        {
            rotateClockwise = false;
            newRotation = highAngle;
        }
        else if (!rotateClockwise && newRotation <= lowAngle)
        {
            rotateClockwise = true;
            newRotation = lowAngle;
        }

        currentRotation = newRotation;
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    void LaunchGrasshopper()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * launchForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
    }
}
