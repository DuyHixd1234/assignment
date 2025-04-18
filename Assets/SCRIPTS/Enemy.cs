using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Di chuyen")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDuration = 8f;
    [SerializeField] private float idleDuration = 4f;
    [SerializeField] private float randomMoveChangeDuration = 2f; // Thời gian thay đổi hướng di chuyển ngẫu nhiên
    [SerializeField] private Vector2 moveAreaSize = new Vector2(10f, 10f); // Kích thước khu vực di chuyển
    [SerializeField] private Vector2 spawnPosition = Vector2.zero; // Vị trí spawn của Enemy

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletDestroyTime = 5f;
    [SerializeField] private float fireCooldown = 3f; // Mỗi 3 giây được bắn 1 lần
    [SerializeField][Range(0f, 1f)] private float fireChance = 0.2f; // Tỉ lệ bắn ngẫu nhiên

    [Header("Animation & Audio")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip deathSFX;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isDead = false;
    private bool isIdle = false;
    private bool movingRight = true;
    private Vector2 currentDirection;
    private float moveTimer;
    private float idleTimer;
    private float fireTimer = 0f;
    private float randomMoveTimer;

    // Các hướng di chuyển có thể của enemy (8 hướng)
    private Vector2[] moveDirections = new Vector2[] {
        new Vector2(1f, 0f), // Phải
        new Vector2(-1f, 0f), // Trái
        new Vector2(0f, 1f), // Lên
        new Vector2(0f, -1f), // Xuống
        new Vector2(1f, 1f).normalized, // Diagonal phải lên
        new Vector2(-1f, 1f).normalized, // Diagonal trái lên
        new Vector2(1f, -1f).normalized, // Diagonal phải xuống
        new Vector2(-1f, -1f).normalized // Diagonal trái xuống
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        moveTimer = moveDuration;
        randomMoveTimer = randomMoveChangeDuration;
        fireTimer = fireCooldown;
        currentDirection = moveDirections[Random.Range(0, moveDirections.Length)]; // Chọn ngẫu nhiên hướng di chuyển ban đầu

        // Gán vị trí spawn nếu không được chỉ định
        if (spawnPosition == Vector2.zero)
        {
            spawnPosition = transform.position;
        }
    }

    void Update()
    {
        if (isDead) return;

        fireTimer -= Time.deltaTime;

        if (isIdle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                isIdle = false;
                moveTimer = moveDuration;
                animator.SetBool("isMoving", true);
            }
            rb.linearVelocity = Vector2.zero; // Dừng di chuyển khi idle
        }
        else
        {
            moveTimer -= Time.deltaTime;
            randomMoveTimer -= Time.deltaTime;

            // Di chuyển enemy theo hướng hiện tại
            rb.linearVelocity = currentDirection * moveSpeed;

            // Nếu đến thời gian thay đổi hướng di chuyển ngẫu nhiên
            if (randomMoveTimer <= 0f)
            {
                currentDirection = moveDirections[Random.Range(0, moveDirections.Length)]; // Chọn ngẫu nhiên hướng mới
                randomMoveTimer = randomMoveChangeDuration; // Reset timer
            }

            // Đổi hướng sau mỗi khoảng thời gian moveDuration
            if (moveTimer <= 0f)
            {
                isIdle = true;
                idleTimer = idleDuration;
                animator.SetBool("isMoving", false);
            }

            // Flip theo hướng di chuyển
            if (currentDirection.x > 0f && !movingRight)
            {
                movingRight = true;
                Flip();
            }
            else if (currentDirection.x < 0f && movingRight)
            {
                movingRight = false;
                Flip();
            }

            // Kiểm tra và giới hạn phạm vi di chuyển
            Vector2 newPosition = rb.position + currentDirection * moveSpeed * Time.deltaTime;
            float halfWidth = moveAreaSize.x / 2;
            float halfHeight = moveAreaSize.y / 2;

            newPosition.x = Mathf.Clamp(newPosition.x, spawnPosition.x - halfWidth, spawnPosition.x + halfWidth);
            newPosition.y = Mathf.Clamp(newPosition.y, spawnPosition.y - halfHeight, spawnPosition.y + halfHeight);

            rb.position = newPosition;
        }

        // Bắn ngẫu nhiên với cooldown và xác suất
        if (fireTimer <= 0f && Random.Range(0f, 1f) < fireChance)
        {
            Attack();
            fireTimer = fireCooldown;
        }
    }

    void Attack()
    {
        if (animator) animator.SetTrigger("attack");

        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }

            bullet.tag = "Enemy";
            Destroy(bullet, bulletDestroyTime);

            if (audioSource && shootSFX)
                audioSource.PlayOneShot(shootSFX);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Dan"))
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;

            if (animator)
                animator.SetTrigger("death");

            if (audioSource && deathSFX)
                audioSource.PlayOneShot(deathSFX);

            Destroy(gameObject, 1.5f);
        }
    }

    // Hàm Flip để thay đổi hướng theo X mà không thay đổi kích thước
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x; // Lật hướng X
        transform.localScale = scale;
    }
}
