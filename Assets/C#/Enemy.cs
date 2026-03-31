using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Chỉ số Quái vật")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    [Header("Cài đặt AI")]
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public Transform[] patrolPoints;

    [Header("Cài đặt Tấn công")]
    public float attackCooldown = 2f;
    public float attackDelay = 0.5f;
    public float damageToPlayer = 15f;

    [Header("Vật phẩm rớt ra")]
    public GameObject itemDropPrefab; // Kéo thả Prefab vật phẩm vào đây

    [Header("Hiệu ứng")]
    public Damage damagePopupPrefab; // Kéo thả Prefab DamageText vào đây

    private float lastAttackTime = 0f;
    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex;
    private bool isDead = false; // Biến kiểm tra xem quái đã chết chưa

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth; // Hồi đầy máu khi bắt đầu

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        // Nếu quái đã chết thì dừng mọi hoạt động
        if (isDead) return;

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.destination = patrolPoints[currentPatrolIndex].position;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
            Invoke("DealDamage", attackDelay);
        }
    }

    void DealDamage()
    {
        // Chặn lỗi: Nếu quái đã chết trong lúc đang giơ tay lên đánh thì hủy đòn đánh luôn
        if (isDead) return;

        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange + 0.5f)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.TakeDamage(damageToPlayer);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        // 1. Chặn không cho máu bị âm
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if (damagePopupPrefab != null)
        {
            // Vị trí xuất hiện: Ngay trên đầu quái vật một chút
            Vector3 popupPos = transform.position + Vector3.up * 1.5f;

            // Sinh ra chữ và truyền số sát thương vào
            Damage popup = Instantiate(damagePopupPrefab, popupPos, Quaternion.identity);
            popup.Setup(damageAmount);
        }

        // Cập nhật UI
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            agent.isStopped = true;
            animator.SetTrigger("Hit");
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        agent.isStopped = true;

        GetComponent<CapsuleCollider>().enabled = false;

        // 2. Ẩn thanh máu đi khi quái ngã xuống
        if (healthBar != null)
        {
            // Tắt GameObject chứa cái Slider thanh máu
            healthBar.gameObject.SetActive(false);
           
        }
        if (itemDropPrefab != null)
        {
            // Tọa độ rớt: Ngay vị trí quái chết, nhưng nhích lên cao 0.5m để đồ không bị lún xuống đất
            Vector3 dropPosition = transform.position + Vector3.up * 0.5f;
            Instantiate(itemDropPrefab, dropPosition, Quaternion.identity);
        }

        Destroy(gameObject, 5f);
    }

    void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }