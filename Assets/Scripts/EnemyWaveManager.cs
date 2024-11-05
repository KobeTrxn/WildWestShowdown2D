using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    void Start()
    {
        // Start the first wave  
        SpawnWave(0); // Spawns 3 enemies on each side
    }

    public void SpawnEnemy(bool spawnOnRight)
    {
        // Choose the spawn position based on spawnOnRight
        Vector3 spawnPosition = spawnOnRight ? rightSpawnPoint.position : leftSpawnPoint.position;

        // Instantiate the enemy at the chosen spawn position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Flip the enemy based on which side it spawns on
        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
        enemySprite.flipX = spawnOnRight; // Flip to face left if spawned on the right side
    }

    // Example function to spawn a wave of enemies
    public void SpawnWave(int enemyCountPerSide)
    {
        for (int i = 0; i < enemyCountPerSide; i++)
        {
            SpawnEnemy(false); // Spawn on the left side
            SpawnEnemy(true);  // Spawn on the right side
        }
    }
}