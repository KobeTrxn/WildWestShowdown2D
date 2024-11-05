using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Required to load scenes

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;            // Maximum health
    public int currentHealth = 100;        // Starting health
    public TextMeshProUGUI healthText;     // Reference to the UI Text for health display
    public string mainMenuSceneName = "MainMenu"; // Name of the main menu scene

    void Start()
    {
        UpdateHealthUI(); // Initialize health display
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and maxHealth
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn’t exceed maxHealth
        UpdateHealthUI();
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        // Load the main menu scene to restart the game
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }
}