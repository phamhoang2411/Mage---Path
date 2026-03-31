using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Chỉ số & UI")]
    public float currentHealth;
    public float maxHealth = 100f;
    public Slider healthBar;

    [Header("Cài đặt di chuyển")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private CharacterController _controller;
    private Animator _anim;
    private Camera _mainCamera;
    private Vector3 _playerVelocity;
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _mainCamera = Camera.main;

        // Khởi tạo máu
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        // Kiểm tra xem nhân vật có chạm đất không
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        HandleMovement();
        HandleActions();

        // Cập nhật trọng lực
        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        // Lấy input trực tiếp từ bàn phím
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Tính hướng di chuyển theo Camera
        Vector3 camForward = _mainCamera.transform.forward;
        Vector3 camRight = _mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 move = (camForward.normalized * moveZ) + (camRight.normalized * moveX);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Di chuyển nhân vật
        if (move.magnitude > 0.1f)
        {
            _controller.Move(move.normalized * currentSpeed * Time.deltaTime);

            // Xoay nhân vật về hướng di chuyển
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // Cập nhật Animator
        float animValue = move.magnitude > 0.1f ? (isRunning ? 1f : 0.5f) : 0f;
        _anim.SetFloat("Speed", animValue);
    }

    void HandleActions()
    {
        // Nhảy
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            _anim.SetTrigger("Jump");
        }

        // Đánh chém (Chuột trái)
        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");
            DealDamage();
        }
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthBar != null) healthBar.value = currentHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            _anim.SetTrigger("Hit");
        }
    }

    public void Die()
    {
        _anim.SetBool("IsDead", true);
        Debug.Log("Player đã chết!");
        this.enabled = false;
    }

    void DealDamage()
    {
        // Logic gây sát thương cho quái ở đây
        Debug.Log("Đang tấn công!");
    }
}