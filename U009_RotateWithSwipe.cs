using UnityEngine;

public class RotateWithSwipe : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // Tốc độ xoay

    private Vector2 lastTouchPosition; // Vị trí chạm cuối cùng
    private bool isTouching = false; // Kiểm tra xem người dùng có đang vuốt không

    void Update()
    {
        if (Input.touchCount > 0) // Nếu có ít nhất 1 ngón tay chạm
        {
            Touch touch = Input.GetTouch(0); // Lấy dữ liệu từ lần chạm đầu tiên

            if (touch.phase == TouchPhase.Began)
            {
                // Lưu vị trí khi bắt đầu chạm
                lastTouchPosition = touch.position;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                // Tính toán khoảng cách vuốt ngang
                Vector2 deltaPosition = touch.position - lastTouchPosition;

                // Chỉ xoay theo trục Y (trái/phải)
                transform.Rotate(Vector3.up, -deltaPosition.x * rotationSpeed, Space.World);

                // Cập nhật vị trí cuối cùng
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false; // Kết thúc thao tác vuốt
            }
        }
    }
}
