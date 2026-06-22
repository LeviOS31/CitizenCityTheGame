using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAISelection : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI handletext;
    public void SliderChange()
    {
        if (slider == null || handletext == null)
        {
            return;
        }

        switch (slider.value)
        {
            case 0:
                handletext.text = "4/0";
                PlayerPrefs.SetInt("AICount", 0);
                break;
            case 1:
                handletext.text = "3/1";
                PlayerPrefs.SetInt("AICount", 1);
                break;
            case 2:
                handletext.text = "2/2";
                PlayerPrefs.SetInt("AICount", 2);
                break;
            case 3:
                handletext.text = "1/3";
                PlayerPrefs.SetInt("AICount", 3);
                break;

        }
    }
}
