using UnityEngine;

public class ChestRong : MonoBehaviour
{
    [SerializeField] private Sprite openChestSprite;
    [SerializeField] private GameObject emptyChestNotice; // panel hoặc ảnh "không có gì"
    [SerializeField] private AudioClip openChestSFX;      // Âm thanh khi mở rương
    [SerializeField] private AudioClip emptyChestSFX;     // Âm thanh khi rương trống

    private bool opened = false;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource gắn trên đối tượng
        if (emptyChestNotice != null) emptyChestNotice.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;

        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = openChestSprite;
            opened = true;

            // Phát âm thanh khi mở rương
            if (audioSource && openChestSFX)
                audioSource.PlayOneShot(openChestSFX);

            if (emptyChestNotice != null)
            {
                emptyChestNotice.SetActive(true);
                // Phát âm thanh khi thông báo rỗng
                if (audioSource && emptyChestSFX)
                    audioSource.PlayOneShot(emptyChestSFX);

                Invoke("HideNotice", 2f); // Hiển thị thông báo 2 giây
            }
        }
    }

    void HideNotice()
    {
        if (emptyChestNotice != null)
            emptyChestNotice.SetActive(false);
    }
}
