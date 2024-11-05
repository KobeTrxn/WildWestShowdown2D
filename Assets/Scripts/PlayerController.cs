using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    Vector2 moveInput;
    public AudioClip shootSound; // Assign the shooting sound in the Inspector
    public float volume = .5f;
    private AudioSource audioSource;
     

    private Boolean _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    public bool _isFacingRight = true;
    public bool IsFacingRight { get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                // Flip scale to make player face opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Set both horizontal and vertical velocity for up/down movement
        rb.velocity = new Vector2(moveInput.x * walkSpeed, moveInput.y * walkSpeed);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // Facing right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Facing left
            IsFacingRight = false;

        }

    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Trigger the shooting animation
            animator.SetTrigger(AnimationStrings.isShooting);

            // Call the OnFire method to instantiate the bullet
            OnFire();

            // Trigger the shooting animation
            animator.ResetTrigger("isShooting");
            animator.SetTrigger("isShooting");

            // Play shooting sound immediately
            audioSource.PlayOneShot(shootSound, volume);
        }
    }

    public GameObject bulletPrefab;

    public void OnFire()
    {
        Vector3 spawnPosition = transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // Access the Bullet script and set its type to PlayerBullet
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.bulletType = Bullet.BulletType.PlayerBullet;

        // Calculate direction toward the cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Set bullet direction
        bulletScript.SetDirection(direction);
    }
}
