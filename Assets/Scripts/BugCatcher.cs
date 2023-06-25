using UnityEngine;

public class BugCatcher : MonoBehaviour
{
    public float maxCatchDistance = 3f;
    public LayerMask bugLayer;
    public Transform bugNet;
    public int resetCounter;

    public int score;
    private int scoreMax;
    private GameObject nearestBug;

    public AudioSource soundEffect;
    public MonologueManager monologueManager;

    public Color newFogColor = new Color(0.5f, 0f, 0f, 1f);  // Dark red color
    public float newFogDensity = 0.4f;
    public float transitionDuration = 2f;
    
    private Color initialFogColor;
    private float initialFogDensity;
    private float transitionTimer = 0f;

    private void Start()
    {
        score = 0;
        nearestBug = null;
        UpdateScoreMax();
        initialFogColor = RenderSettings.fogColor;
        initialFogDensity = RenderSettings.fogDensity;

        // Call the monologue manager to show the initial monologue text
        monologueManager.UpdateMonologue(score);
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
                //Debug.Log(IsPlayerLookingAtBug(nearestBug));
                if (nearestBug != null && IsPlayerLookingAtBug(nearestBug))
                {

                    //Debug.Log("near bug and looking at");
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

        // Check if the number of bugs collected is 5
        if (score == 5)
        {

            // Start the fog color transition
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);
            RenderSettings.fogColor = Color.Lerp(initialFogColor, newFogColor, t);

            // Start the fog density transition
            float currentFogDensity = Mathf.Lerp(initialFogDensity, newFogDensity, t);
            RenderSettings.fogDensity = currentFogDensity;

            // Check if the transition is complete
            if (t >= 1f)
            {
                // Reset the transition variables
                transitionTimer = 0f;
                initialFogColor = newFogColor;
                initialFogDensity = newFogDensity;
            }
        }
    }

    private void UpdateScoreMax()
    {
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bugs");

        foreach (GameObject bug in bugs)
        {
            Vector3 raycastOrigin = bug.transform.position - new Vector3(0, 10f, 0); // Modify the starting position

            RaycastHit hit;
            if (Physics.Raycast(raycastOrigin, Vector3.up, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("Generated"))
                {
                    scoreMax++;
                    Debug.Log("Score increased! New scoreMax: " + scoreMax);
                }
                else
                {
                    Debug.Log("Ray hit object with tag: " + hitObject.tag);
                }
            }
            else
            {
                //Debug.Log("Raycast didn't hit anything above the bug.");
            }
        }
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
        Camera playerCamera = Camera.main;

        if (playerCamera == null || !playerCamera.CompareTag("MainCamera") || playerCamera.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            // Player camera not found or does not meet the criteria
            Debug.LogError("Player camera not found or not correctly tagged/layered.");
            return false;
        }
        Vector3 playerPosition = transform.position;
        Vector3 bugDirection = bug.transform.position - playerPosition;
        float bugDistance = bugDirection.magnitude;
        bugDirection /= bugDistance;

        if (bugDistance <= maxCatchDistance)
        {
            Vector3 playerForward = transform.forward;
            float dotProduct = Vector3.Dot(bugDirection.normalized, playerForward.normalized);

            if (dotProduct > 0.5f)
            {
                // The bug is within catch distance and the player is looking at it
                // Add your desired behavior here
                //Debug.Log("Player is looking at the bug!");
                return true;
            }
        }
        //Debug.Log("Bug not hit by raycast.");
        return false;
        


    }

    private void CatchBug(GameObject bug)
    {
        // Play SFX
        soundEffect.Play();
        // Hide the bug object
        bug.SetActive(false);

        // Increment the score counter
        score++;

        // Perform any additional actions or scoring logic here

        //Debug.Log("Bug caught! Score: " + score);

        // Increase the intensity of jarLight by 0.3
        GetComponentInChildren<Light>().intensity += 0.03f;

        // Check if the number of bugs collected is 5
        if (score == 5)
        {
            // Call the monologue manager to update the monologue text
            monologueManager.UpdateMonologue(score);

        }
        else if (score == 7)
        {
            // Call the monologue manager to update the monologue text
            monologueManager.UpdateMonologue(score);
        }
    }

    public void ResetScore()
    {
        resetCounter++;
        score = 0;
        Debug.Log("Score Reset. Reset Counter: " + resetCounter);
    }
}
