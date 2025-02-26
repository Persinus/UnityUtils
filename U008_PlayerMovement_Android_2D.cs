using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_Android_2D : MonoBehaviour, Input_player.IPlayerActions
{
    private Input_player controls; // Lớp auto-generated
    private Vector2 moveInput; // Input di chuyển
    public Joystick joystick; // Joystick từ Joystick Pack
    public float movementSpeed = 5f; // Tốc độ di chuyển trái/phải
    public float jumpForce = 7f; // Lực nhảy
    private Rigidbody2D rb; // Rigidbody của nhân vật
    private bool isGrounded = true; // Kiểm tra nhân vật có trên mặt đất không

    private void Awake()
    {
        // Khởi tạo Input System
        controls = new Input_player();
        controls.Player.SetCallbacks(this); // Đăng ký callback
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody từ GameObject
    }

    private void OnEnable()
    {
        controls.Enable(); // Bật Input System
    }

    private void OnDisable()
    {
        controls.Disable(); // Tắt Input System
    }

    private void Update()
    {
        // Sử dụng Joystick nếu có
        if (joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            moveInput = new Vector2(joystick.Horizontal, 0);
            if (isGrounded && joystick.Vertical > 0.5f)
            {
                Jump(); // Gọi hàm nhảy khi Joystick dọc > 0.5
            }
        }
    }

    private void FixedUpdate()
    {
        // Di chuyển trái/phải
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput.x * movementSpeed;
        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // Không thể nhảy tiếp khi đang ở trên không
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Sử dụng Input System nếu Joystick không hoạt động
        if (joystick == null || (joystick.Horizontal == 0 && joystick.Vertical == 0))
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Xác định khi nhân vật chạm đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
