using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Cài đặt theo dõi")]
    public Transform target;          // Kéo Player vào đây
    public float distance = 4.0f;     // Khoảng cách từ cam đến lưng nhân vật
    public float heightOffset = 1.2f; 

    [Header("Độ nhạy chuột")]
    public float mouseSensitivity = 200f;

    private float currentX = 0f;
    private float currentY = 0f;

    void Start()
    {
        // Khóa con trỏ chuột ở giữa màn hình
        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        if (target == null) return;

        // 1. Nhận dữ liệu chuột
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. Giới hạn góc ngước lên/cúi xuống (tránh lộn gầm)
        currentY = Mathf.Clamp(currentY, -15f, 60f);

        // 3. Tính toán vòng xoay xung quanh nhân vật
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 4. Xác định điểm tâm để Camera nhìn vào
        Vector3 lookAtPoint = target.position + Vector3.up * heightOffset;

        // 5. Cập nhật vị trí và góc nhìn của Camera
        Vector3 position = lookAtPoint - (rotation * Vector3.forward * distance);
        transform.position = position;
        transform.LookAt(lookAtPoint);
    }
}
