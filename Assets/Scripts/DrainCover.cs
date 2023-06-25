using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DrainCover : MonoBehaviour
{
    public MonologueManager monologueManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("DrainCover: OnTriggerEnter");
        BugCatcher bugCatcher = FindObjectOfType<BugCatcher>();
        GameController gameController = FindObjectOfType<GameController>();

        if (bugCatcher != null)
        {

            if (bugCatcher.score >= 7)
            {
                if (gameController != null)
                {
                    gameController.StartNewGame();
                    bugCatcher.ResetScore();
                    monologueManager.UpdateMonologue(0);
                }
            }
        }
    }
}

