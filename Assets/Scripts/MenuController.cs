using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image transitionImage;
    public float transitionDuration = 1f; 
    private bool transitioning = false;
    public string creditsURL;
    public GameObject[] menuItems;

    private void Start()
    {

        // Subscribe to the button events
        Button newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        newGameButton.onClick.AddListener(NewGame);

        Button exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(Exit);

        Button creditsButton = GameObject.Find("CreditsButton").GetComponent<Button>();
        creditsButton.onClick.AddListener(OpenCredits);
    }

    private void NewGame()
    {
        if (transitioning)
        {
            return;
        }

        transitioning = true;
        StartCoroutine(TransitionToScene("MazeScene"));
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

    private System.Collections.IEnumerator TransitionToScene(string sceneName)
    {
        // Set the transition image and menu items to fully opaque
        transitionImage.color = new Color(0f, 0f, 0f, 1f);
        SetMenuItemsAlpha(1f);

        // Smoothly fade out the transition image and menu items
        float timer = 0f;
        while (timer < transitionDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / transitionDuration);
            transitionImage.color = new Color(0f, 0f, 0f, alpha);
            SetMenuItemsAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // Load the next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the new scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
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
    private void SetMenuItemsAlpha(float alpha)
    {
        foreach (GameObject item in menuItems)
        {
            Graphic[] graphics = item.GetComponentsInChildren<Graphic>(true);
            foreach (Graphic graphic in graphics)
            {
                graphic.canvasRenderer.SetAlpha(alpha);
            }
        }
    }
}
