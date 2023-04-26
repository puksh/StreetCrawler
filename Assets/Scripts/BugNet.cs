using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugNet : MonoBehaviour
{
    public float range = 5f;
    public LayerMask bugLayer;
    public GameObject caughtBugPrefab;

    void Update()
    {
        // Check if the player presses the left mouse button to catch a bug
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray in front of the player to detect bugs within range
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, bugLayer))
            {
                // Create a new instance of the caught bug prefab and position it at the hit point
                GameObject caughtBug = Instantiate(caughtBugPrefab, hit.point, Quaternion.identity);

                // Destroy the bug that was hit by the raycast
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
