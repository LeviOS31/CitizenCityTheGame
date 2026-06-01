using TMPro;
using UnityEngine;

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
