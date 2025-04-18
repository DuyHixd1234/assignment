using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Di chuyen")]
    [SerializeField] private float speed = 5f;

    [Header("Mau & UI")]
    [SerializeField] private int maxHP = 10;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Dan & Attack1")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletDestroyTime = 5f;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip bulletHitSFX;

    [Header("Audio - Damage")]
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private AudioClip deathSFX;

    [Header("Audio - Attack2")]
    [SerializeField] private AudioClip swordSFX;
    [SerializeField] private AudioClip swordHitSFX;

    private int currentHP;
    private bool isDead = false;
    private Vector2 movement;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    // Vu khi duoc chon
    private bool canShoot = false;
    private bool canSlash = false;

    // Luu scale goc
    private Vector3 originalScale;

    void Start()
    {
        currentHP = maxHP;

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
            animator = GetComponent<Animator>();

        originalScale = transform.localScale; // Luu scale goc luc bat dau

        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        animator.SetBool("isMoving", movement.sqrMagnitude > 0);

        // Flip dung theo scale goc
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack1();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack2();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && rb != null)
        {
            rb.linearVelocity = speed * movement;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHealthUI();

        if (currentHP > 0)
        {
            if (audioSource && hurtSFX)
                audioSource.PlayOneShot(hurtSFX);

            animator.SetTrigger("hurt");
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;

        movement = Vector2.zero;
        animator.SetBool("isMoving", false);
        rb.linearVelocity = Vector2.zero;

        if (audioSource && deathSFX)
            audioSource.PlayOneShot(deathSFX);

        animator.SetTrigger("death");

        yield return new WaitForSeconds(1.2f);
        Cursor.visible = true;
        SceneManager.LoadScene("LoseScene");
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHP / maxHP;

        if (healthText != null)
            healthText.text = currentHP + "/" + maxHP;
    }

    public void Attack1()
    {
        if (isDead || !canShoot || bulletPrefab == null || bulletSpawnPoint == null) return;

        animator.SetTrigger("attack1");

        if (audioSource && shootSFX)
            audioSource.PlayOneShot(shootSFX);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Dan danScript = bullet.GetComponent<Dan>();

        if (danScript != null)
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            danScript.SetUp(direction, bulletSpeed, bulletDestroyTime, null, bulletHitSFX);
        }
    }

    public void Attack2()
    {
        if (isDead || !canSlash) return;

        animator.SetTrigger("attack2");

        if (audioSource && swordSFX)
            audioSource.PlayOneShot(swordSFX);

        // Xu ly va cham bang Attack2Hitbox.cs
    }

    public AudioClip GetSwordHitSFX()
    {
        return swordHitSFX;
    }

    public void EnableGun(bool value)
    {
        canShoot = value;
    }

    public void EnableSword(bool value)
    {
        canSlash = value;
    }
}
