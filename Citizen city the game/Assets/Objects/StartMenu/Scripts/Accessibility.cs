using TMPro;
using UnityEngine;

public class Accessibility : MonoBehaviour
{
    public TMP_FontAsset NormalFont;
    public TMP_FontAsset HighContrastFont;

    public TMP_FontAsset FontTNRoman;
    public TMP_FontAsset FontComicSans;
    public TMP_FontAsset FontOpenSans;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        TextMeshProUGUI[] textcomponents = GameObject.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (TextMeshProUGUI text in textcomponents)
        {
            TextMemory memory = text.GetComponent<TextMemory>();
            if (memory == null) 
            { 
                memory = text.gameObject.AddComponent<TextMemory>();
            }

            if (PlayerPrefs.GetInt("HighContrast", 0) == 1)
            {
                text.color = new Color(0.85f, 1f, 0f);
                text.font = HighContrastFont;
            }else{
                if (PlayerPrefs.GetInt("Font",0) == 0)
                {
                    memory.Reset();
                }
                else
                {
                    text.color = memory.NormalColor;
                    switch (PlayerPrefs.GetInt("Font",0))
                    {
                        case 1:
                            text.font = FontComicSans;
                            break;
                        case 2:
                            text.font = FontOpenSans;
                            break;
                        case 3:
                            text.font = FontTNRoman;
                            break;
                    }
                }
            }

            float fontsize = PlayerPrefs.GetInt("FontSizeOffset", 0);

            text.fontSize = memory.NormalFontSize * (1f + fontsize / 100f);

            if (fontsize != 0)
            {
                Debug.Log(memory.NormalFontSize + " * " + "(1 + " + fontsize + " / 100)");
                Debug.Log("=");
                Debug.Log(memory.NormalFontSize * (1f + fontsize / 100f));
                Debug.Log(" ");
            }
        }
    }
}
