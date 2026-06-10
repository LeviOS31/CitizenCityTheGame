using System.Collections;
using TMPro;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Timing Settings")]
    [Tooltip("How long the text stays perfectly visible before starting to disappear.")]
    [SerializeField] private float displayDuration = 1.2f; // Short duration for fast gameplay
    [Tooltip("How fast the text completely vanishes.")]
    [SerializeField] private float fadeOutSpeed = 0.2f;    // Snappy fade out

    [Header("Feedback Palette")]
    [SerializeField] private Color successColor = new Color(0.12f, 0.73f, 0.40f);
    [SerializeField] private Color errorColor = new Color(0.92f, 0.25f, 0.25f);
    [SerializeField] private Color infoColor = new Color(0.22f, 0.58f, 0.93f);

    private Coroutine activeLifecycleCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (feedbackText != null)
        {
            feedbackText.text = string.Empty;
        }
    }
    public void ShowFeedback(string message, FeedbackType type)
    {
        if (feedbackText == null) return;

        if (activeLifecycleCoroutine != null)
        {
            StopCoroutine(activeLifecycleCoroutine);
        }

        Color targetColor = infoColor;
        switch (type)
        {
            case FeedbackType.Success: targetColor = successColor; break;
            case FeedbackType.Error: targetColor = errorColor; break;
        }

        feedbackText.color = new Color(targetColor.r, targetColor.g, targetColor.b, 1f);
        feedbackText.text = message;

        activeLifecycleCoroutine = StartCoroutine(FeedbackLifecycle());
    }

    private IEnumerator FeedbackLifecycle()
    {
        yield return new WaitForSeconds(displayDuration);

        Color originalColor = feedbackText.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutSpeed);
            feedbackText.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        feedbackText.text = string.Empty;
        activeLifecycleCoroutine = null;
    }
}

public enum FeedbackType
{
    Success,
    Error,
    Info
}