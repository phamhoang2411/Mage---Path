using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float gravity = 20f; 
    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity; 
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Nhận tín hiệu phím
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 2. Xử lý di chuyển
        if (direction.magnitude >= 0.1f)
        {
            // Xoay nhân vật
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // Cộng thêm góc xoay của Camera nếu cần (để đơn giản hiện tại xoay theo hướng bấm)
            Quaternion toRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }

        // 3. Xử lý trọng lực (Kéo nhân vật xuống đất)
        if (characterController.isGrounded)
        {
            velocity.y = -2f; // Ép nhẹ xuống đất
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
        animator.SetFloat("Speed", direction.magnitude);
    }
}