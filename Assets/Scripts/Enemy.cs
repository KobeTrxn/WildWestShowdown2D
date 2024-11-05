using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 1;
    public GameObject bulletPrefab;
    public float minShootCooldown = 1f; // Minimum time between shots
    public float maxShootCooldown = 3f; // Maximum time between shots
    public float moveSpeed = 3.5f;
    public float minMoveDistance = 0.5f;
    public float maxMoveDistance = 1.5f;

    private int currentHealth;
    private Animator animator;
    private bool isDead = false;
    private float shootTimer;
    private Transform playerTransform;

    public delegate void EnemyDeathHandler();
    public event EnemyDeathHandler OnEnemyDeath;

    public AudioClip shootSound; // Assign the shooting sound in the Inspector
    public float volume = .5f;
    private AudioSource audioSource;

    private void Start()
    {

        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        ResetShootTimer();
        StartCoroutine(RandomMovement());
    }

    private void Update()
    {
        // Handle shooting cooldown
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0 && !isDead)
        {
            Shoot();
            ResetShootTimer();
        }
    }

    private void ResetShootTimer()
    {
        shootTimer = UnityEngine.Random.Range(minShootCooldown, maxShootCooldown);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Prevent taking damage if already dead

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Prevent multiple calls to Die
        isDead = true;

        // Trigger the death animation
        if (animator != null)
        {
            animator.SetTrigger("DieTrigger"); // Ensure the trigger name matches in Animator
        }

        // Invoke the death event for the WaveManager
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath.Invoke();
        }

        // Delay destruction to allow death animation to play
        Destroy(gameObject, 1f); // Adjust delay to match the length of your death animation
    }

    private void Shoot()
    {
        if (playerTransform == null || isDead) return;

        // Trigger the shooting animation
        animator.ResetTrigger("ShootTrigger");
        animator.SetTrigger("ShootTrigger");

        // Play shooting sound immediately
        audioSource.PlayOneShot(shootSound, volume);

        // Calculate bullet spawn position and direction
        Vector3 spawnPosition = transform.position + new Vector3(0.5f, 0, 0);
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        bulletScript.bulletType = Bullet.BulletType.EnemyBullet;
        bulletScript.SetDirection(direction, 6.5f); // Slower bullets for enemies
    }

    private IEnumerator RandomMovement()
    {
        while (!isDead)
        {
            // Randomly select a movement direction
            Vector2 randomDirection = new Vector2(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
            ).normalized;

            // Randomize the distance
            float moveDistance = UnityEngine.Random.Range(minMoveDistance, maxMoveDistance);
            Vector2 targetPosition = (Vector2)transform.position + randomDirection * moveDistance;

            // Move towards the target position
            float timeToMove = moveDistance / moveSpeed;
            float elapsedTime = 0;

            while (elapsedTime < timeToMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Short pause before choosing a new movement direction
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }
}