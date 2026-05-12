using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AccessibilitySettings : MonoBehaviour
{
    public Toggle toggleHighContrast;

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
}