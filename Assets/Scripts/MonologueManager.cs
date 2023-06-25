using UnityEngine;
using TMPro;

public class MonologueManager : MonoBehaviour
{
    private BugCatcher bugCatcher;
    public TextMeshProUGUI monologueText;
    public float baseDisplayDurationPerCharacter = 0.1f;

    private float displayDuration;

    private void Start()
    {
        bugCatcher = FindObjectOfType<BugCatcher>();
    }

    public void UpdateMonologue(int bugsCollected)
    {
        if (bugCatcher.resetCounter == 2)
        {
            if (bugsCollected == 0)
            {
                ShowMonologue("I...");
            }
            else if (bugsCollected == 5)
            {
                ShowMonologue("....");
            }
            else if (bugsCollected == 7)
            {
                // Show the monologue text for collecting all 7 bugs
                ShowMonologue("why...");
            }
        }
        else if (bugCatcher.resetCounter == 1)
        {
            if (bugsCollected == 0)
            {
                ShowMonologue("I should collect at least 7... wait...");
            }
            else if (bugsCollected == 5)
            {
                ShowMonologue("...only 2 more...");
            }
            else if (bugsCollected == 7)
            {
                // Show the monologue text for collecting all 7 bugs
                ShowMonologue("Where is the drain now...");
            }
        }
        else
        {
            if (bugsCollected == 0)
            {
                ShowMonologue("I should collect at least 7 of the bugs here...");
            }
            else if (bugsCollected == 5)
            {
                ShowMonologue("I should hurry up... 2 more to go");
            }
            else if (bugsCollected == 7)
            {
                // Show the monologue text for collecting all 7 bugs
                ShowMonologue("Done! I should be able to leave through the drain system");
            }
        }

        
    }
    public void ShowMonologue(string text)
    {
        monologueText.text = text;

        // Calculate the display duration based on the number of characters
        displayDuration = baseDisplayDurationPerCharacter * text.Length;

        // Start the coroutine to hide the monologue text after the display duration
        StartCoroutine(HideMonologueAfterDuration(displayDuration));
    }

    private System.Collections.IEnumerator HideMonologueAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Hide the monologue text
        monologueText.text = "";
    }
}
