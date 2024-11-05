using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public int initialEnemiesPerPoint = 0;
    public float spawnDelay = 1f;
    public float timeBetweenWaves = 5f;
    public int waveHealAmount = 20;            // Amount to heal player each wave

    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI waveCountText;

    private int currentWave = 0;
    private int enemiesToSpawnLeft;
    private int enemiesToSpawnRight;
    private int enemiesRemaining;
    private PlayerHealth playerHealth;        // Reference to the player’s health script

    void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        StartNextWave(); // Start the first wave
    }

    void StartNextWave()
    {
        currentWave++;

        // Start with 2 enemies on each side for the first wave, and increase by 1 per wave
        enemiesToSpawnLeft = 2 + (currentWave - 1);
        enemiesToSpawnRight = 2 + (currentWave - 1);

        // Total enemies in this wave
        enemiesRemaining = enemiesToSpawnLeft + enemiesToSpawnRight;

        UpdateWaveUI();
        UpdateEnemyCountUI();

        // Heal the player after each wave, but don’t exceed max health
        if (playerHealth != null)
        {
            playerHealth.Heal(waveHealAmount);
        }

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // Spawn enemies on the left
        for (int i = 0; i < enemiesToSpawnLeft; i++)
        {
            SpawnEnemy(leftSpawnPoint, false); // Spawn on the left without flipping
            yield return new WaitForSeconds(spawnDelay);
        }

        // Spawn enemies on the right
        for (int i = 0; i < enemiesToSpawnRight; i++)
        {
            SpawnEnemy(rightSpawnPoint, true); // Spawn on the right with flip
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy(Transform spawnPoint, bool flipSprite)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Flip the enemy sprite if necessary
        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
        if (enemySprite != null)
        {
            enemySprite.flipX = flipSprite;
        }

        // Subscribe to the enemy's death event to track remaining enemies
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDeath += EnemyDied;
        }
    }

    void EnemyDied()
    {
        enemiesRemaining--;
        UpdateEnemyCountUI();

        if (enemiesRemaining <= 0)
        {
            Invoke("StartNextWave", timeBetweenWaves);
        }
    }

    void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies Left: " + enemiesRemaining;
        }
    }

    void UpdateWaveUI()
    {
        if (waveCountText != null)
        {
            waveCountText.text = "Wave: " + currentWave;
        }
    }
}