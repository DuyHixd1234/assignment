using UnityEngine;
using System.Collections;

public class TomatoPlant : MonoBehaviour
{
    public Sprite[] growthStages; // 0: Lv0, 1: Lv1, 2: Lv2, 3: Lv3, 4: Héo
    private int currentLevel = 0;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
        StartCoroutine(GrowOverTime());
    }

    void UpdateSprite()
    {
        if (currentLevel >= 0 && currentLevel < growthStages.Length)
        {
            spriteRenderer.sprite = growthStages[currentLevel];
        }
    }

    public void WaterPlant()
    {
        if (currentLevel < 3)
        {
            currentLevel++;
            UpdateSprite();
        }
    }

    public void Wither()
    {
        currentLevel = 4; // Chuyển sang sprite héo
        UpdateSprite();
        StartCoroutine(ResetPlant());
    }

    private IEnumerator GrowOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f); // Chờ 4 giây

            if (currentLevel < 3)
            {
                currentLevel++;
                UpdateSprite();
            }
            else
            {
                Wither(); // Nếu đã Lv3, cây sẽ héo
            }
        }
    }

    private IEnumerator ResetPlant()
    {
        yield return new WaitForSeconds(4f); // Đợi 4 giây sau khi héo
        currentLevel = 0; // Quay về Lv0
        UpdateSprite();
    }
}
