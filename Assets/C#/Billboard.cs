using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Tự động tìm Main Camera trong Scene
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Lệnh này ép thanh máu luôn nhìn thẳng vào mặt Camera
        transform.LookAt(transform.position + mainCameraTransform.forward);
    }
}

