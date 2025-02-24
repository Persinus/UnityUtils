using UnityEngine;

/// <summary>
/// Handles player movement and jumping using a joystick.
/// Requires a Rigidbody2D and collision detection for ground checking.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class U006_Joystick_movement_Windows_2D : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] private Joystick movementJoystick; // Reference to the movement joystick
    [SerializeField] private float speed = 5f; // Movement speed
    [SerializeField] private float jumpForce = 7f; // Force applied when jumping

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Handles player movement and jumping based on joystick input.
    /// </summary>
    private void HandleMovement()
    {
        // Horizontal movement
        float moveX = movementJoystick.Direction.x * speed;
        rb.velocity = new Vector2(moveX, rb.velocity.y);

        // Jumping
        if (movementJoystick.Direction.y > 0.5f && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false; // Prevent multiple jumps
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Player is grounded when colliding with ground
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Player leaves the ground
        }
    }
}
