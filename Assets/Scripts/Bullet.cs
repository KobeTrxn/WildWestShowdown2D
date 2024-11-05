using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveDirection;

    // Enum to differentiate bullet types
    public enum BulletType { PlayerBullet, EnemyBullet }
    public BulletType bulletType;

    public void SetDirection(Vector2 direction, float customSpeed = -1f)
    {
        moveDirection = direction;
        // If a custom speed is provided, override the default speed
        if (customSpeed > 0)
        {
            speed = customSpeed;
        }
    }

    private void Start()
    {
        // Set bullet velocity based on direction
        GetComponent<Rigidbody2D>().velocity = moveDirection * speed;

        // Set the layer based on the bullet type
        if (bulletType == BulletType.PlayerBullet)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
        }
        else if (bulletType == BulletType.EnemyBullet)
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
        }
    }

    private void OnBecameInvisible()
    {
        // Destroy the bullet when it goes off-screen
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bulletType == BulletType.EnemyBullet && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player hit by enemy bullet");

            // Access the PlayerHealth component and deal 10 damage
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Deal 10 damage to the player
            }

            Destroy(gameObject); // Destroy the bullet on impact
        }
        else if (bulletType == BulletType.PlayerBullet && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy hit by player bullet");
            // Damage the enemy (you can add code here to decrease enemy health)

            Destroy(gameObject); // Destroy the bullet on impact
        }
    }
}