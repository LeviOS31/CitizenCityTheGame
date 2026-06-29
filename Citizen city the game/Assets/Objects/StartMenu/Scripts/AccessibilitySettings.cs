using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class is responsible for managing accessibility settings in the game, including high contrast mode, font size adjustments, and font selection.
// It interacts with UI elements such as toggles, sliders, and dropdowns to allow users to customize their experience.
public class AccessibilitySettings : MonoBehaviour
{
    public Toggle toggleHighContrast;
    public Slider FontSizeOffset;
    public TMP_Dropdown Fontselector;

    private void Start()
    {
        if (PlayerPrefs.GetInt("HighContrast", 0) == 1)
        {
            toggleHighContrast.isOn = true;
        }
        else
        {
            toggleHighContrast.isOn = false;
        }

        FontSizeOffset.value = PlayerPrefs.GetInt("FontSizeOffset", 0);
        
    }
    public void toggle()
    {
        if (toggleHighContrast.isOn)
        {
            PlayerPrefs.SetInt("HighContrast", 1);
        }
        else
        {
            PlayerPrefs.SetInt("HighContrast", 0);
        }
    }

    public void changeFontSize()
    {
        PlayerPrefs.SetInt("FontSizeOffset", (int)FontSizeOffset.value);
    }

    public void ChangeFont()
    {
        PlayerPrefs.SetInt("Font", Fontselector.value);
    }
}