using TMPro;
using UnityEngine;

public class TextMemory : MonoBehaviour
{
    [HideInInspector] public Color NormalColor;
    [HideInInspector] public TMP_FontAsset NormalFont;

    private void Awake()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        NormalColor = text.color;
        NormalFont = text.font;
    }

    public void Reset()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.color = NormalColor;
        text.font = NormalFont;
    }
}
