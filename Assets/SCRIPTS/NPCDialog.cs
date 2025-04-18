using UnityEngine;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject choicePanel;

    private void Start()
    {
        dialogPanel.SetActive(false);
        choicePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            dialogPanel.SetActive(true);

            if (inventory != null && inventory.hasGoldCoin)
            {
                dialogText.text = "Bạn đã có Đồng xu vàng trong tay, hãy chọn vũ khí của bạn.";
                choicePanel.SetActive(true); // hiện các nút chọn Kiếm hoặc Súng
            }
            else
            {
                dialogText.text = "Bạn cần có Đồng xu vàng để mua vũ khí.";
                choicePanel.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogPanel.SetActive(false);
            choicePanel.SetActive(false);
        }
    }
}
