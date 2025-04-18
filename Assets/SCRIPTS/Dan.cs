using UnityEngine;

public class Dan : MonoBehaviour
{
    private Vector2 huong;
    private float tocDo;
    private float thoiGianHuy;
    private AudioClip tiengBan;
    private AudioClip tiengTrungDich;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    public void SetUp(Vector2 dir, float speed, float destroyTime, AudioClip shootSFX, AudioClip hitSFX)
    {
        huong = dir.normalized;
        tocDo = speed;
        thoiGianHuy = destroyTime;
        tiengBan = shootSFX;
        tiengTrungDich = hitSFX;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (tiengBan != null && audioSource != null)
        {
            audioSource.PlayOneShot(tiengBan);
        }

        Destroy(gameObject, thoiGianHuy);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = huong * tocDo;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (tiengTrungDich != null && audioSource != null)
            {
                AudioSource.PlayClipAtPoint(tiengTrungDich, transform.position);
            }

            Destroy(other.gameObject);
            Destroy(gameObject); // Huy dan khi trung
        }
    }
}
