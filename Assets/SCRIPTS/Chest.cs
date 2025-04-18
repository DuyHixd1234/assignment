using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Sprite openChestSprite;
    [SerializeField] private GameObject coinImageUI;     // anh dong xu UI
    [SerializeField] private GameObject receivedCoinImage; // anh thong bao nhan duoc xu
    [SerializeField] private AudioClip openChestSFX; // Âm thanh khi mở rương

    private bool opened = false;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;  // Thêm AudioSource để phát âm thanh

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource từ đối tượng

        if (coinImageUI != null) coinImageUI.SetActive(false);
        if (receivedCoinImage != null) receivedCoinImage.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;

        if (other.CompareTag("Player"))
        {
            // Thay đổi sprite và đánh dấu rương đã mở
            spriteRenderer.sprite = openChestSprite;
            opened = true;

            // Kiểm tra và cập nhật thông tin trong Inventory của người chơi
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.hasGoldCoin = true;

                // Hiển thị hình ảnh thông báo nhận được đồng xu
                if (coinImageUI != null) coinImageUI.SetActive(true);
                if (receivedCoinImage != null)
                {
                    receivedCoinImage.SetActive(true);
                    Invoke("HideCoinImage", 5f); // Tắt ảnh thông báo sau 5 giây
                }
            }

            // Phát âm thanh khi va chạm với rương
            if (audioSource != null && openChestSFX != null)
            {
                audioSource.PlayOneShot(openChestSFX);
            }
        }
    }

    void HideCoinImage()
    {
        if (receivedCoinImage != null)
            receivedCoinImage.SetActive(false);
    }
}
