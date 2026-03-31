using TMPro;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Cài đặt bay")]
    public float moveSpeed = 2f;     // Tốc độ chữ bay lên trời
    public float destroyTime = 1f;   // Chữ sẽ biến mất sau 1 giây

    private TextMeshPro textMesh;
    private Transform camTransform;

    void Awake()
    {
       
        textMesh = GetComponent<TextMeshPro>();
        camTransform = Camera.main.transform;
    }

    
    public void Setup(float damageAmount)
    {
        textMesh.text = "-" + damageAmount.ToString();

        // Hẹn giờ tự hủy cái chữ này
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // 1. Chữ bay từ từ lên trên
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 2. Ép chữ luôn xoay mặt về phía người chơi
        transform.LookAt(transform.position + camTransform.forward);
    }
}
