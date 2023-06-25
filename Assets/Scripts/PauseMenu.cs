using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public string creditsURL;
    public bool showCursorOnPause = true;
    private bool isPaused = false; 
    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisibility;
    private void Start()
    {
        previousCursorLockMode = Cursor.lockState;
        previousCursorVisibility = Cursor.visible;
        // Subscribe to the button events when the pause menu is activated
        pauseMenuUI.SetActive(true);

        // Subscribe to the button events
        Button resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(Resume);

        Button exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(Exit);

        Button creditsButton = GameObject.Find("CreditsButton").GetComponent<Button>();
        creditsButton.onClick.AddListener(OpenCredits);
        // Deactivate the pause menu after subscribing to button events
        pauseMenuUI.SetActive(false);
    }
    

    private void Exit()
    {
        Debug.Log("Exiting the game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game by setting the time scale to 0
        if (showCursorOnPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game by setting the time scale to 1

        if (showCursorOnPause)
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the window
            Cursor.visible = false; // Hide the cursor
        }
        pauseMenuUI.SetActive(false);
    }
    private void OpenCredits()
    {
        Debug.Log("Opening credits website: " + creditsURL);
        #if UNITY_EDITOR
            Application.OpenURL(creditsURL);
        #else
                OpenURLInBrowser(creditsURL);
        #endif
    }

    private void OpenURLInBrowser(string url)
    {
        // Check if the platform supports opening URLs
        if (Application.platform == RuntimePlatform.WebGLPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("Opening URLs is not supported on this platform.");
        }
    }
}