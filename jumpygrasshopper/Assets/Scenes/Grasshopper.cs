using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasshopper : MonoBehaviour
{
    public GameObject gameOver;
    public float rotationSpeed = 30f; // The rotation speed in degrees per second
    private Rigidbody2D rb; // Rigidbody2D reference
    private bool hasBeenLaunched = false;
    private float currentRotation = 280f; // Track the current rotation angle
    private bool rotateClockwise = true; // Indicates whether to rotate clockwise or counterclockwise
    private float highAngle = 355f;
    private float lowAngle = 280f;
    public float panSpeed = 5.0f; // Speed of camera panning
    public GameObject landingLeafPrefab; // Prefab of the LandingLeaf block
    private bool shouldPan = false; // Flag to control camera panning
    private Vector3 targetPosition; // Target position for camera panning
    private GameObject startingLeaf;
    private GameObject landingLeaf;
    private float distanceBetweenLeafs;
    private float randomFloat; 
    private float newLeafX = -7.33f;
    private float newLandingLeafX;
    private bool offScreen = false;
    private GameObject oldStartingLeaf;
    public AudioSource src;
    public AudioClip jump, land;
    public Scoring score;
    private GameObject foreground;
    private GameObject foreground2;
    private GameObject middleground;
    private GameObject middleground2;
    private GameObject background;
    private GameObject background2;
    private GameObject clouds;
    private GameObject clouds2;
    Animator myAnimator;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component on this GameObject
        rb.isKinematic = true; // Start with kinematic set to true
    }

    private void Start()
    {
        startingLeaf = GameObject.FindWithTag("StartingLeaf");
        landingLeaf = GameObject.FindWithTag("LandingLeaf");
        oldStartingLeaf = GameObject.FindWithTag("StartingLeaf");
        newLandingLeafX = landingLeaf.transform.position.x;
        foreground = GameObject.FindWithTag("Foreground");
        foreground2 = GameObject.FindWithTag("Foreground2");
        middleground = GameObject.FindWithTag("Middleground");
        middleground2 = GameObject.FindWithTag("Middleground2");
        background = GameObject.FindWithTag("Background");
        background2 = GameObject.FindWithTag("Background2");
        clouds = GameObject.FindWithTag("Clouds");
        clouds2 = GameObject.FindWithTag("Clouds2");
        myAnimator = GetComponent<Animator>();

        if (startingLeaf != null && landingLeaf != null)
        {
            distanceBetweenLeafs = startingLeaf.transform.position.x - landingLeaf.transform.position.x;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LandingLeaf") && startingLeaf != null && landingLeaf != null)
        {
            MoveForeground();
            MoveForeground2();
            MoveMiddleground();
            MoveMiddleground2();
            MoveBackground();
            MoveBackground2();
            MoveClouds();
            MoveClouds2();
            
            src.clip = land;
            src.Play();
            myAnimator.SetBool("isJumping", false);
            randomFloat = Random.Range(3.0f, 14.5f);

            oldStartingLeaf = GameObject.FindWithTag("StartingLeaf");
            oldStartingLeaf.tag = "Untagged";
            collision.gameObject.tag = "StartingLeaf";
            startingLeaf = GameObject.FindWithTag("StartingLeaf");

            Collider2D startingLeafCollider = startingLeaf.GetComponent<Collider2D>();
            
            if (startingLeafCollider != null)
            {
                startingLeafCollider.enabled = false;
                Debug.Log(startingLeafCollider.enabled);
            }
            else
            {
                Debug.Log("Collider not found!");
            }
            
            // Recalculate the distance between leaves whenever a collision occurs
            distanceBetweenLeafs = newLeafX - landingLeaf.transform.position.x;

            // Set the target position for camera panning
            targetPosition = new Vector3(newLandingLeafX + 6.79f, Camera.main.transform.position.y, Camera.main.transform.position.z);
            newLeafX += -distanceBetweenLeafs;
            shouldPan = true;

            Instantiate(landingLeafPrefab, new Vector3(landingLeaf.transform.position.x + randomFloat, landingLeaf.transform.position.y, landingLeaf.transform.position.z), Quaternion.identity);
            landingLeaf = GameObject.FindWithTag("LandingLeaf");
            newLandingLeafX = landingLeaf.transform.position.x;
            
            RespawnGrasshopper();
            DeleteOldLeaf();
            score.AddScore();
        }
    }

    public void OnRestartButtonClick()
        {
            RespawnGrasshopper();
            score.ResetScore();
            gameOver.SetActive(false);
        }
    private void Update()
    {
        ScrollForeground();
        ScrollMiddleground();
        ScrollBackground();
        ScrollClouds();
        if (!hasBeenLaunched)
        {
            RotateGrasshopper();
        }

        // Check if the grasshopper is out of the screen boundaries
        


        if (Input.GetKeyDown(KeyCode.R))
        {
            
        }

        if  ((!hasBeenLaunched && Input.GetKeyDown(KeyCode.Space) && transform.position.y > -3f) || (!hasBeenLaunched && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && transform.position.y > -3f))
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
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        bool isOffScreen = screenPosition.y < -150;

        if (isOffScreen)
        {
            // Stop the grasshopper when it goes offscreen
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
            hasBeenLaunched = false;
            offScreen = true;
            gameOver.SetActive(true);
        }
        else
        {
            offScreen = false;
        }

    }

    void MoveForeground()
    {
        float foreground2Position = foreground2.transform.position.x;
        if (transform.position.x > foreground.transform.position.x + 36f)
        {
            foreground.transform.position = new Vector3(foreground2Position + 36f, foreground.transform.position.y, foreground.transform.position.z);
        }
    }

    void MoveForeground2()
    {
        float foregroundPosition = foreground.transform.position.x;
        if (transform.position.x > foreground2.transform.position.x + 36f)
        {
            foreground2.transform.position = new Vector3(foregroundPosition + 36f, foreground2.transform.position.y, foreground2.transform.position.z);
        }
    }

    void MoveMiddleground()
    {
        float middleground2Position = middleground2.transform.position.x;
        if (transform.position.x >middleground.transform.position.x + 36f)
        {
            middleground.transform.position = new Vector3(middleground2Position + 36f, middleground.transform.position.y, middleground.transform.position.z);
        }
    }

    void MoveMiddleground2()
    {
        float middlegroundPosition = middleground.transform.position.x;
        if (transform.position.x > middleground2.transform.position.x + 36f)
        {
            middleground2.transform.position = new Vector3(middlegroundPosition + 36f, middleground2.transform.position.y, middleground2.transform.position.z);
        }
    }
    void MoveBackground()
    {
        float background2Position = background2.transform.position.x;
        if (transform.position.x >background.transform.position.x + 36f)
        {
            background.transform.position = new Vector3(background2Position + 36f, background.transform.position.y, background.transform.position.z);
        }
    }

    void MoveBackground2()
    {
        float backgroundPosition = background.transform.position.x;
        if (transform.position.x > background2.transform.position.x + 36f)
        {
            background2.transform.position = new Vector3(backgroundPosition + 36f, background2.transform.position.y, background2.transform.position.z);
        }
    }
    void MoveClouds()
    {
        float clouds2Position = clouds2.transform.position.x;
        if (transform.position.x >clouds.transform.position.x + 36f)
        {
            clouds.transform.position = new Vector3(clouds2Position + 36f, clouds.transform.position.y, clouds.transform.position.z);
        }
    }

    void MoveClouds2()
    {
        float cloudsPosition = clouds.transform.position.x;
        if (transform.position.x > clouds2.transform.position.x + 36f)
        {
            clouds2.transform.position = new Vector3(cloudsPosition + 36f, clouds2.transform.position.y, clouds2.transform.position.z);
        }
    }
    void ScrollForeground()
    {
    // Check if the camera is panning
    if (shouldPan)
    {
        // Calculate the distance the camera has moved since the last frame
        float cameraDeltaX = (targetPosition.x - Camera.main.transform.position.x) * 0.009f;

        // Adjust the middleground position by the same amount as the camera movement
        foreground.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        foreground2.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        }
    }
    void ScrollMiddleground()
    {
    // Check if the camera is panning
    if (shouldPan)
    {
        // Calculate the distance the camera has moved since the last frame
        float cameraDeltaX = (targetPosition.x - Camera.main.transform.position.x) * 0.015f;

        // Adjust the middleground position by the same amount as the camera movement
        middleground.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        middleground2.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        }
    }
    void ScrollBackground()
    {
    // Check if the camera is panning
    if (shouldPan)
    {
        // Calculate the distance the camera has moved since the last frame
        float cameraDeltaX = (targetPosition.x - Camera.main.transform.position.x) * 0.021f;

        // Adjust the middleground position by the same amount as the camera movement
        background.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        background2.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        }
    }
    void ScrollClouds()
    {
    // Check if the camera is panning
    if (shouldPan)
    {
        // Calculate the distance the camera has moved since the last frame
        float cameraDeltaX = (targetPosition.x - Camera.main.transform.position.x) * 0.024f;

        // Adjust the middleground position by the same amount as the camera movement
        clouds.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        clouds2.transform.position += new Vector3(cameraDeltaX, 0f, 0f);
        }
    }


    void RespawnGrasshopper()
    {
        myAnimator.SetBool("isJumping", false);
        // Reset the grasshopper's position and rotation
        if (Camera.main.transform.position.x == 0 && transform.position.y < -6)
        {
           transform.position = new Vector3(newLeafX, -2.95f, 0f);
        } else {
            transform.position = new Vector3(newLeafX-.5f, -2.95f, 0f);
        }
        
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
        src.clip = jump;
        src.Play();
        myAnimator.SetBool("isJumping", true);
        float launchAngle = currentRotation % 360;

        if (currentRotation <= 285f) {
            
            float normalizedAngle = Mathf.InverseLerp(281f, 285f, launchAngle);
            float interpolatedForce = Mathf.Lerp(28f, 24.4f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 290f) {
            
            float normalizedAngle = Mathf.InverseLerp(286f, 290f, launchAngle);
            float interpolatedForce = Mathf.Lerp(23.5f, 20.9f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 295f) {
            
            float normalizedAngle = Mathf.InverseLerp(291f, 295f, launchAngle);
            float interpolatedForce = Mathf.Lerp(20.3f, 18.5f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 300f) {
            
            float normalizedAngle = Mathf.InverseLerp(296f, 300f, launchAngle);
            float interpolatedForce = Mathf.Lerp(18f, 16.6f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 305f) {
            
            float normalizedAngle = Mathf.InverseLerp(301f, 305f, launchAngle);
            float interpolatedForce = Mathf.Lerp(16.5f, 15.3f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 310f) {
            
            float normalizedAngle = Mathf.InverseLerp(306f, 310f, launchAngle);
            float interpolatedForce = Mathf.Lerp(15.3f, 14.5f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 315f) {
            
            float normalizedAngle = Mathf.InverseLerp(311f, 315f, launchAngle);
            float interpolatedForce = Mathf.Lerp(14.5f, 13.8f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 320f) {
            
            float normalizedAngle = Mathf.InverseLerp(316f, 320f, launchAngle);
            float interpolatedForce = Mathf.Lerp(13.8f, 13f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 325f) {
            
            float normalizedAngle = Mathf.InverseLerp(321f, 325f, launchAngle);
            float interpolatedForce = Mathf.Lerp(13f, 12.6f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 330f) {
            
            float normalizedAngle = Mathf.InverseLerp(326f, 330f, launchAngle);
            float interpolatedForce = Mathf.Lerp(12.6f, 12.5f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 335f) {
            
            float normalizedAngle = Mathf.InverseLerp(331f, 335f, launchAngle);
            float interpolatedForce = Mathf.Lerp(12.5f, 12f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 340f) {
            
            float normalizedAngle = Mathf.InverseLerp(336f, 340f, launchAngle);
            float interpolatedForce = Mathf.Lerp(12f, 12f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 345f) {
            
            float normalizedAngle = Mathf.InverseLerp(341f, 345f, launchAngle);
            float interpolatedForce = Mathf.Lerp(12f, 12.5f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 350f) {
            
            float normalizedAngle = Mathf.InverseLerp(346f, 350f, launchAngle);
            float interpolatedForce = Mathf.Lerp(12.5f, 13.5f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
        else if (currentRotation <= 355f) {
            
            float normalizedAngle = Mathf.InverseLerp(351f, 355f, launchAngle);
            float interpolatedForce = Mathf.Lerp(13f, 15f, normalizedAngle);
            rb.isKinematic = false; // Disable kinematic to allow physics to take over
            rb.AddForce(transform.up * interpolatedForce, ForceMode2D.Impulse); // Launch upwards in 2D space
            hasBeenLaunched = true; // Mark the object as launched
        }
    }
    void DeleteOldLeaf()
    {
        StartCoroutine(DeleteWithDelay());
    }

    IEnumerator DeleteWithDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(0.2f);
            Destroy(oldStartingLeaf);
    }
}
