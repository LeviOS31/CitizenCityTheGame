using TMPro;
using UnityEngine;

// this class is responsible for storing the original properties of a TextMeshProUGUI component, such as its color, font, and font size.
// It allows for resetting the text properties to their original state when needed.
public class TextMemory : MonoBehaviour
{
    public Color NormalColor;
    public TMP_FontAsset NormalFont;
    public float NormalFontSize;

    private void Awake()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        NormalColor = text.color;
        NormalFont = text.font;
        NormalFontSize = text.fontSize;
    }

    public void Reset()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.color = NormalColor;
        text.font = NormalFont;
    }
}
