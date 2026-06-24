using System;
using UnityEngine;
using UnityEngine.UI;

// This class manages the settings menu in the game, allowing users to switch between video, audio, and accessibility settings.
// video settings have been removed, but the code is still present in case it is added back in the future.
public class settings : MonoBehaviour
{
    public GameObject VideoButton;
    public GameObject AudioButton;
    public GameObject AccessibilityButton;
    public GameObject PrefabVideosettings;
    public GameObject PrefabAudiosettings;
    public GameObject PrefabAccessibilitysettings;

    private GameObject Content;

    public void OnEnable()
    {
        Audio();
    }

    public void Video()
    {
        VideoButton.GetComponent<Button>().interactable = false;
        AudioButton.GetComponent<Button>().interactable = true;
        AccessibilityButton.GetComponent<Button>().interactable = true;

        if (Content != null)
        {
            Destroy(Content);
        }
        Content = Instantiate(PrefabVideosettings, transform);

        GetComponent<Image>().color = VideoButton.GetComponent<Image>().color;
    }

    public void Audio()
    {
        VideoButton.GetComponent<Button>().interactable = true;
        AudioButton.GetComponent<Button>().interactable = false;
        AccessibilityButton.GetComponent<Button>().interactable = true;

        if (Content != null)
        {
            Destroy(Content);
        }
        Content = Instantiate(PrefabAudiosettings, transform);

        GetComponent<Image>().color = AudioButton.GetComponent<Image>().color;   
    }

    public void Accessibility()
    {
        VideoButton.GetComponent<Button>().interactable = true;
        AudioButton.GetComponent<Button>().interactable = true;
        AccessibilityButton.GetComponent<Button>().interactable = false;

        if (Content != null)
        {
            Destroy(Content);
        }
        Content = Instantiate(PrefabAccessibilitysettings, transform);

        GetComponent<Image>().color = AccessibilityButton.GetComponent<Image>().color;   
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
