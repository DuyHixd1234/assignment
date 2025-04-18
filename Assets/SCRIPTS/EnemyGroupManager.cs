using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EnemyGroupManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text enemyCounterText;  // Text hien thi so ke dich
    public Slider enemySlider;         // Slider the hien ti le con lai
    public string winSceneName = "WinScene";

    private int totalEnemies;

    void Start()
    {
        totalEnemies = transform.childCount;
        UpdateEnemyUI();
    }

    void Update()
    {
        int remainingEnemies = CountAliveEnemies();
        UpdateEnemyUI(remainingEnemies);

        if (remainingEnemies == 0 && totalEnemies > 0)
        {
            SceneManager.LoadScene(winSceneName);
        }
    }

    int CountAliveEnemies()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }

    void UpdateEnemyUI(int current = -1)
    {
        if (current == -1)
            current = CountAliveEnemies();

        if (enemyCounterText != null)
        {
            enemyCounterText.text = current + "/" + totalEnemies;
        }

        if (enemySlider != null)
        {
            // Tang dan khi tieu diet dich
            float progress = totalEnemies > 0 ? 1f - ((float)current / totalEnemies) : 1f;
            enemySlider.value = progress;
        }
    }

}
