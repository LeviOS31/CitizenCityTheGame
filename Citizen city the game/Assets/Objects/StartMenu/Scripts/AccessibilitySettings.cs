using TMPro;
using UnityEngine;
using UnityEngine.UI;

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