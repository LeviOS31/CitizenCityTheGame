using TMPro;
using UnityEngine;

public class Accessibility : MonoBehaviour
{
    public TMP_FontAsset NormalFont;
    public TMP_FontAsset HighContrastFont;

    private void Awake()
    {
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
            }
            else
            {
                memory.Reset();
            }
        }
    }
}
