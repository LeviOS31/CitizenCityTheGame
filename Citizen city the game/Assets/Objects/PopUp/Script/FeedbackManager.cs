using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    private CanvasGroup panelCanvasGroup;

    [Header("Timing Settings")]
    [SerializeField] private float displayDuration = 1.2f;
    [SerializeField] private float fadeOutSpeed = 0.2f;

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

        panelCanvasGroup = GetComponent<CanvasGroup>();

        if (feedbackText != null) feedbackText.text = string.Empty;
        panelCanvasGroup.alpha = 0f;
    }

    public void ShowFeedback(string message, FeedbackType type)
    {
        if (feedbackText == null) return;

        if (activeLifecycleCoroutine != null)
        {
            StopCoroutine(activeLifecycleCoroutine);
        }

        Image panel = GetComponent<Image>();

        switch (type)
        {
            case FeedbackType.Success: panel.color = successColor; break;
            case FeedbackType.Error: panel.color = errorColor; break;
            case FeedbackType.Info: panel.color = infoColor; break;
        }

        feedbackText.text = message;
        panelCanvasGroup.alpha = 1f;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

        AudioSignalHandler.PlaySound?.Invoke("PopUp");

        activeLifecycleCoroutine = StartCoroutine(FeedbackLifecycle());
    }

    private IEnumerator FeedbackLifecycle()
    {
        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        while (elapsedTime < fadeOutSpeed)
        {
            elapsedTime += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutSpeed);
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