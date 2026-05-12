using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    public TMP_Dropdown displaychoice;

    private DisplayInfo currentdisplay; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
        GetCurrentDisplay();

    }

    void GetCurrentDisplay()
    {
        currentdisplay = Screen.mainWindowDisplayInfo;
        List<TMP_Dropdown.OptionData> options = displaychoice.options;

        int selectedindex = 0;

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].text.Contains(currentdisplay.name))
            {
                selectedindex = i;
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

        // 1. Get the current index from the dropdown
        int selectedIndex = displaychoice.value;

        // 2. Safety check
        if (selectedIndex >= 0)
        {
            DisplayInfo targetDisplay = displays[selectedIndex];

            // 3. Perform the move
            Screen.MoveMainWindowTo(targetDisplay, Vector2Int.zero);

            // 4. Match resolution (Borderless Fullscreen is best for this)
            Screen.SetResolution(targetDisplay.width, targetDisplay.height, FullScreenMode.FullScreenWindow);

            Debug.Log($"Applied change: Moving to {targetDisplay.name}");
        }
    }

    public void applysettings()
    {
        SetDisplay();
    }

    public void resetsettigns()
    {
        GetCurrentDisplay();
    }
}
