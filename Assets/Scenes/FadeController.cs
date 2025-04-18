using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Try-Catch to ensure the CanvasGroup is found
        try
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                throw new System.Exception("CanvasGroup component is missing!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error in FadeController: " + e.Message);
        }
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("FadeIn failed: CanvasGroup is missing.");
            yield break;
        }

        canvasGroup.alpha = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false); // Hide panel after fade

        Debug.Log("FadeIn completed successfully.");
    }

    public IEnumerator FadeOut(System.Action onComplete = null)
    {
        if (canvasGroup == null)
        {
            Debug.LogError("FadeOut failed: CanvasGroup is missing.");
            yield break;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        Debug.Log("FadeOut completed successfully.");
        onComplete?.Invoke();
    }
}
