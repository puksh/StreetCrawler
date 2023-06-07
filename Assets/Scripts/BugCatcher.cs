using UnityEngine;

public class BugCatcher : MonoBehaviour
{
    public float maxCatchDistance = 3f;
    public LayerMask bugLayer;
    public Transform bugNet;

    private int score;
    private GameObject nearestBug;

    private void Start()
    {
        score = 0;
        nearestBug = null;
    }

    private void Update()
    {
        // Check if the player is holding the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("clicked mouse");
            // Check if the tool has index 1 (bug net)
            if (GetComponentInParent<ToolSwitcher>().currentToolIndex == 1)
            {
                //Debug.Log("correct tool");
                // Check if there is a nearest bug to catch
                Debug.Log(IsPlayerLookingAtBug(nearestBug));
                if (nearestBug != null && IsPlayerLookingAtBug(nearestBug))
                {

                    Debug.Log("near bug and looking at");
                    // Catch the bug
                    CatchBug(nearestBug);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Update the nearest bug when the player moves
        UpdateNearestBug();
        //Debug.Log(nearestBug);
    }

    private void UpdateNearestBug()
    {
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bugs");

        float nearestDistance = Mathf.Infinity;
        GameObject newNearestBug = null;

        foreach (GameObject bug in bugs)
        {
            float distance = Vector3.Distance(transform.position, bug.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                newNearestBug = bug;
            }
        }

        nearestBug = newNearestBug;
    }

    private bool IsPlayerLookingAtBug(GameObject bug)
    {
        Vector3 playerPosition = transform.position;
        Vector3 bugDirection = bug.transform.position - playerPosition;
        float bugDistance = bugDirection.magnitude;
        bugDirection /= bugDistance;

        if (bugDistance <= maxCatchDistance)
        {
            if (Vector3.Dot(bugDirection, transform.forward) > 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerPosition, bugDirection, out hit, bugDistance, bugLayer))
                {
                    if (hit.collider.gameObject == bug)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void CatchBug(GameObject bug)
    {
        // Hide the bug object
        bug.SetActive(false);

        // Increment the score counter
        score++;

        // Perform any additional actions or scoring logic here

        Debug.Log("Bug caught! Score: " + score);
    }
}
