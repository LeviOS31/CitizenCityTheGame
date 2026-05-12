using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    public TMP_Dropdown displaychoice;
    public TMP_Dropdown resolutionchoice;
    public TMP_Dropdown windowchoice;

    private DisplayInfo currentdisplay;
    private Vector2Int currentresolution;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetCurrentDisplay();
        GetResolution();
    }

    void GetCurrentDisplay()
    {
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);

        List<string> displayNames = new List<string>();

        int i = 1;
        foreach (DisplayInfo display in displays)
        {
            displayNames.Add("Display" + i + ": " + display.name);
            i++;
        }

        displaychoice.AddOptions(displayNames);

        currentdisplay = Screen.mainWindowDisplayInfo;
        List<TMP_Dropdown.OptionData> options = displaychoice.options;

        int selectedindex = 0;

        for (int j = 0; j < options.Count; j++)
        {
            if (options[j].text.Contains(currentdisplay.name))
            {
                selectedindex = j;
                break;
            }
        }

        displaychoice.value = selectedindex;
        displaychoice.RefreshShownValue();
    }

    void SetDisplay()
    {
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);

        int selectedIndex = displaychoice.value;

        if (selectedIndex >= 0)
        {
            DisplayInfo targetDisplay = displays[selectedIndex];

            Screen.MoveMainWindowTo(targetDisplay, Vector2Int.zero);

            Screen.SetResolution(targetDisplay.width, targetDisplay.height, FullScreenMode.FullScreenWindow);

            Debug.Log($"Applied change: Moving to {targetDisplay.name}");

            currentdisplay = targetDisplay;
        }
    }

    void GetResolution()
    {
        List<string> resolutionOptions = new List<string>();

        foreach (Resolution resolution in Screen.resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            
            if (!resolutionOptions.Contains(option))
            {
                resolutionOptions.Add(option);
            }
        }

        resolutionchoice.AddOptions(resolutionOptions);

        currentresolution = new Vector2Int(Screen.width, Screen.height);

        List<TMP_Dropdown.OptionData> options = resolutionchoice.options;
        int selectedindex = 0;
        for (int j = 0; j < options.Count; j++)
        {
            if (options[j].text.Contains(currentresolution.x + " x " + currentresolution.y))
            {
                selectedindex = j;
                break;
            }
        }

        resolutionchoice.value = selectedindex;
        resolutionchoice.RefreshShownValue();
    }

    void SetResolution()
    {
        string value = resolutionchoice.options[resolutionchoice.value].text;

        if (value != null) 
        { 
            string[] dimensions = value.Split('x');
            int width = int.Parse(dimensions[0].Trim());
            int height = int.Parse(dimensions[1].Trim());
            Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
            Debug.Log($"Applied change: Setting resolution to {width} x {height}");
            currentresolution = new Vector2Int(width, height);
        }
    }

    public void applysettings()
    {
        SetDisplay();
        SetResolution();
    }

    public void resetsettigns()
    {
        GetCurrentDisplay();
        GetResolution();
    }
}
