using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float destroyAfterSeconds = 3f;
    [SerializeField] private AudioClip shootSFX;

    private Vector2 direction = Vector2.left;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (shootSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSFX);
        }

        Destroy(gameObject, destroyAfterSeconds);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed; // Dung linearVelocity de tranh loi ve velocity obsoleted
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // KHONG goi TakeDamage vi da xu ly o PlayerController
            Destroy(gameObject);
        }
    }
}
