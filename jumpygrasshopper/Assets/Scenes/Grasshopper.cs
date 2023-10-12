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
    private float lowAngle = 275f;
    public float panSpeed = 5.0f; // Speed of camera panning
    public GameObject landingLeafPrefab; // Prefab of the LandingLeaf block
    private bool shouldPan = false; // Flag to control camera panning
    private Vector3 targetPosition; // Target position for camera panning
    private GameObject startingLeaf;
    private GameObject landingLeaf;
    private float distanceBetweenLeafs;
    private float randomFloat; 
    private float newLeafX = -7.33f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component on this GameObject
        rb.isKinematic = true; // Start with kinematic set to true
    }

    private void Start()
    {
        // Initialize game objects and calculate the initial distance between leaves
        startingLeaf = GameObject.FindWithTag("StartingLeaf");
        landingLeaf = GameObject.FindWithTag("LandingLeaf");

        if (startingLeaf != null && landingLeaf != null)
        {
            distanceBetweenLeafs = startingLeaf.transform.position.x - landingLeaf.transform.position.x;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LandingLeaf") && startingLeaf != null && landingLeaf != null)
        {

                ///////////// Get this part to work!!!!!!!!! /////////////

        //     GameObject oldStartingLeaf = GameObject.FindGameObjectWithTag("StartingLeaf");

        // // Delete the old StartingLeaf if found
        //     if (oldStartingLeaf != null)
        //     {
        //         Destroy(oldStartingLeaf);
        //     }
            randomFloat = Random.Range(3.0f, 14.5f);
            collision.gameObject.tag = "StartingLeaf";
            // Recalculate the distance between leaves whenever a collision occurs
            distanceBetweenLeafs = startingLeaf.transform.position.x - landingLeaf.transform.position.x;

            // Set the target position for camera panning
            targetPosition = Camera.main.transform.position + Vector3.right * -distanceBetweenLeafs;
            newLeafX += -distanceBetweenLeafs;
            shouldPan = true;

            // Instantiate a new LandingLeaf block in front of the old block
            Instantiate(landingLeafPrefab, new Vector3(landingLeaf.transform.position.x + randomFloat, landingLeaf.transform.position.y, landingLeaf.transform.position.z), Quaternion.identity);
            
            RespawnGrasshopper();
        }
    }
        private void Update()
    {
        
        if (!hasBeenLaunched)
        {
            RotateGrasshopper();
        }

        // Check if the grasshopper is out of the screen boundaries
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnGrasshopper();
        }

        if (!hasBeenLaunched && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchGrasshopper();
        }
        if (shouldPan)
        {
            // Smoothly move the camera to the target position
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, panSpeed * Time.deltaTime);

            // Check if the camera is close to the target position and stop panning
            if (Vector3.Distance(Camera.main.transform.position, targetPosition) < 0.1f)
            {
                shouldPan = false;
            }
        }
    }

    void RespawnGrasshopper()
    {
        // Reset the grasshopper's position and rotation
        transform.position = new Vector3(newLeafX, -2.13f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Reset rotation variables
        currentRotation = 280f;
        rotateClockwise = true;

        // Reset launch state
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        hasBeenLaunched = false;

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
        if (currentRotation <= 276f) {
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 35f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 277f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 33f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 278f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 31f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 280f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 28f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 283f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 25f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 288f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 22f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 293f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 20f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 300f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 18f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 306f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 16f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 324f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 13.5f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else if (currentRotation <= 342f){
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 12f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        } else {
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * 15f, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
    }
}