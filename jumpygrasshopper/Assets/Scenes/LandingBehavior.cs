// using UnityEngine;

// public class LandingBehavior : MonoBehaviour
// {
//     public float panSpeed = 5.0f; // Speed of camera panning
//     public GameObject landingLeafPrefab; // Prefab of the LandingLeaf block
//     private bool shouldPan = false; // Flag to control camera panning
//     private Vector3 targetPosition; // Target position for camera panning
//     private GameObject startingLeaf;
//     private GameObject landingLeaf;
//     private float distanceBetweenLeafs;
//     private float randomFloat; // Declare randomFloat at the class level


//     private void Start()
//     {
//         // Initialize game objects and calculate the initial distance between leaves
//         startingLeaf = GameObject.FindWithTag("StartingLeaf");
//         landingLeaf = GameObject.FindWithTag("LandingLeaf");

//         if (startingLeaf != null && landingLeaf != null)
//         {
//             distanceBetweenLeafs = startingLeaf.transform.position.x - landingLeaf.transform.position.x;
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("LandingLeaf") && startingLeaf != null && landingLeaf != null)
//         {
//             randomFloat = Random.Range(3.0f, 14.5f);
//             collision.gameObject.tag = "StartingLeaf";
//             // Recalculate the distance between leaves whenever a collision occurs
//             distanceBetweenLeafs = startingLeaf.transform.position.x - landingLeaf.transform.position.x;

//             // Set the target position for camera panning
//             targetPosition = Camera.main.transform.position + Vector3.right * -distanceBetweenLeafs;
//             shouldPan = true;

//             // Instantiate a new LandingLeaf block in front of the old block
//             Instantiate(landingLeafPrefab, new Vector3(landingLeaf.transform.position.x + randomFloat, landingLeaf.transform.position.y, landingLeaf.transform.position.z), Quaternion.identity);
//         }
//     }

//     private void Update()
//     {
//         if (shouldPan)
//         {
//             // Smoothly move the camera to the target position
//             Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, panSpeed * Time.deltaTime);

//             // Check if the camera is close to the target position and stop panning
//             if (Vector3.Distance(Camera.main.transform.position, targetPosition) < 0.1f)
//             {
//                 shouldPan = false;
//             }
//         }
//     }
// }
