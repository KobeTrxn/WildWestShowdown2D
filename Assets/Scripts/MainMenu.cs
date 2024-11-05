using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "GameplayScene";
    public Image fadeOverlay; // Reference to a UI Image for fade effect
    public float fadeDuration = 1f;

    private void Start()
    {
        Cursor.visible = true;  // Show the cursor in the main menu
        Cursor.lockState = CursorLockMode.None;  // Ensure the cursor is not locked

        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0, 0, 0, 1);
            StartCoroutine(FadeOut());
        }
    }

    public void PlayGame()
    {
        Debug.Log("Play button clicked");

        // Check if we're in the main menu and need to load the gameplay scene
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            StartCoroutine(FadeInAndLoadScene());
        }
        else
        {
            // If already in gameplay, just resume
            Time.timeScale = 1f;
            gameObject.SetActive(false); // Hide the pause menu if one exists
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked");

#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // Quit application in the build
    Application.Quit();
#endif
    }


    private IEnumerator FadeInAndLoadScene()
    {
        if (fadeOverlay != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeOverlay.color = new Color(0, 0, 0, t / fadeDuration);
                yield return null;
            }
        }

        // Reset time scale and load the gameplay scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator FadeOut()
    {
        if (fadeOverlay != null)
        {
            // Fade from black to transparent
            float t = fadeDuration;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                fadeOverlay.color = new Color(0, 0, 0, t / fadeDuration);
                yield return null;
            }

            // Set overlay to fully transparent after fade
            fadeOverlay.color = new Color(0, 0, 0, 0);
        }
    }
}