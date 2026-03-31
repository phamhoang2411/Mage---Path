using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    public Transform target; // Kéo nhân vật vào đây
    public Vector3 offset;   // Khoảng cách giữa camera và nhân vật
    public float smoothSpeed = 0.125f; // Độ mượt khi di chuyển

    void LateUpdate() 
    {
        if (target == null) return;

        // Tính toán vị trí mong muốn
        Vector3 desiredPosition = target.position + offset;

        // Dùng Lerp để làm mượt chuyển động
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Gán vị trí cho camera
        transform.position = smoothedPosition;

        // (Tùy chọn) Luôn nhìn vào nhân vật
        transform.LookAt(target);
    }
}