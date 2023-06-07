using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    

    private MazeConstructor generator;

    private int score;
    private bool goalReached;

    //3
    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        StartNewGame();
    }

    //4
    private void StartNewGame()
    {

        score = 0;

        StartNewMaze();
    }

    //5
    private void StartNewMaze()
    {
        generator.GenerateNewMaze(27, 29, OnStartTrigger);

        float x = generator.startCol * generator.hallWidth; float z = generator.startRow * generator.hallWidth;

        goalReached = false;

    }


    //7
    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Goal!");
        goalReached = true;

        score += 1;

        Destroy(trigger);
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        if (goalReached)
        {
            Debug.Log("Finish!");

            Invoke("StartNewMaze", 4);
        }
    }
}
