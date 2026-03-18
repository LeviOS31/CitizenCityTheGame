using UnityEngine;
using TMPro;

public class NewsTicker : MonoBehaviour
{
    public float scrollSpeed = 100f;
    private TextMeshProUGUI textComponent;
    private RectTransform rectTransform;
    private float textWidth;
    private float parentWidth;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        // Calculate widths
        textWidth = textComponent.preferredWidth;
        parentWidth = rectTransform.parent.GetComponent<RectTransform>().rect.width;

        // Start position: just off-screen to the right
        Vector3 startPos = rectTransform.localPosition;
        startPos.x = parentWidth / 2 + (textWidth / 2);
        rectTransform.localPosition = startPos;
    }

    void Update()
    {
        // Move left
        rectTransform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // If the text has moved completely off-screen to the left, reset it
        if (rectTransform.localPosition.x < -(parentWidth / 2 + textWidth / 2))
        {
            Vector3 resetPos = rectTransform.localPosition;
            resetPos.x = parentWidth / 2 + (textWidth / 2);
            rectTransform.localPosition = resetPos;
        }
    }
}