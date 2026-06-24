using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

// This class is responsible for managing accessibility and other settings in the game, including font styles, high contrast mode, and audio mixer settings. It ensures that user preferences are applied consistently across all UI elements.
public class Accessibility : MonoBehaviour
{
    public TMP_FontAsset NormalFont;
    public TMP_FontAsset HighContrastFont;
    public TMP_FontAsset FontTNRoman;
    public TMP_FontAsset FontComicSans;
    public TMP_FontAsset FontOpenSans;

    public AudioMixer Mixer;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // This method initializes the audio mixer settings based on user preferences stored in PlayerPrefs. It converts volume levels from linear scale to decibels and applies them to the corresponding mixer parameters.
    private void Start()
    {
        float masterdB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("MasterVolume", 0.5f), 0.0001f)) * 20;
        float musicdB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("MusicVolume", 1f), 0.0001f)) * 20;
        float backgrounddB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("BackgroundVolume", 1f), 0.0001f)) * 20;
        float effectdB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("EffectsVolume", 1f), 0.0001f)) * 20;

        Mixer.SetFloat("MasterVolume", masterdB);
        Mixer.SetFloat("MusicVolume", musicdB);
        Mixer.SetFloat("BackgroundVolume", backgrounddB);
        Mixer.SetFloat("EffectsVolume", effectdB);
    }


    // This method updates the font and color settings for all TextMeshProUGUI components in the scene based on user preferences stored in PlayerPrefs. It handles high contrast mode, font selection, and font size adjustments.
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
