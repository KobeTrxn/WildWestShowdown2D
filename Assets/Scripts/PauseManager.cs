using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // Set the name of the main menu scene here

    void Update()
    {
        // Check if the 'Esc' key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }

    private void GoToMainMenu()
    {
        // Set Time.timeScale back to 1 to ensure the game resumes normal speed in the main menu
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
