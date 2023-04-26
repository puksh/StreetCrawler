using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public float rotationSpeed = 5.0f; // Speed at which the planes rotate towards the player

    void Update()
    {
        // Loop through all the child objects of this GameObject
        foreach (Transform child in transform)
        {
            // Check if the child object has the "Bugs" tag
            if (child.gameObject.CompareTag("Bugs"))
            {
                // Calculate the direction vector from the child plane to the player, ignoring the Y-axis
                Vector3 direction = player.transform.position - child.transform.position;
                direction.y = 0;

                // Calculate the current angles of the child plane in euler angles
                Vector3 currentRotation = child.transform.rotation.eulerAngles;

                // Keep the current Z-axis rotation of the child plane
                float currentZRotation = currentRotation.z;

                // Use the "look at" function to rotate the child plane towards the player
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Remove the Z-axis rotation from the target rotation
                Vector3 targetEulerAngles = targetRotation.eulerAngles;
                targetEulerAngles.z = 0;
                targetRotation = Quaternion.Euler(targetEulerAngles);

                // Use spherical interpolation to smoothly rotate the child plane towards the target rotation
                child.transform.rotation = Quaternion.Slerp(child.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Set the Z-axis rotation of the child plane back to its original value
                currentRotation.z = currentZRotation;
                child.transform.rotation = Quaternion.Euler(currentRotation);
            }
        }
    }
}
