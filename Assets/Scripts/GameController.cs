using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]
public class GameController : MonoBehaviour
{
    public Image transitionImage;
    public float transitionDuration = 1f;

    private MazeConstructor generator;
    private bool transitioning = false;
    

    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        StartNewGame();
    }
    
    public void StartNewGame()
    {
        generator.GenerateNewMaze(27, 29);
        StartNewMaze();
    }
    
    private void StartNewMaze()
    {

        if (transitioning)
        {
            return;
        }

        transitioning = true;
        StartCoroutine(TransitionToMaze());
    }

    private System.Collections.IEnumerator TransitionToMaze()
    {
        // Set the transition image to black and make it fully opaque
        transitionImage.color = Color.black;
        transitionImage.gameObject.SetActive(true);

        // Smoothly fade in the transition effect
        float timer = 0f;
        while (timer < transitionDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / transitionDuration);
            transitionImage.color = new Color(0f, 0f, 0f, alpha);
            timer += Time.deltaTime;
            yield return null;
        }


        float x = generator.startCol * generator.hallWidth;
        float z = generator.startRow * generator.hallWidth;
        
        transitioning = false;
    }


    
}
