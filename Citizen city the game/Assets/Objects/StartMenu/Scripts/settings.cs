using System;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    public GameObject VideoButton;
    public GameObject AudioButton;
    public GameObject CreditButton;
    public GameObject PrefabVideosettings;
    public GameObject PrefabAudiosettings;
    public GameObject PrefabAccesibilitysettings;

    private GameObject Content;

    public void Video()
    {
        VideoButton.GetComponent<Button>().interactable = false;
        AudioButton.GetComponent<Button>().interactable = true;
        CreditButton.GetComponent<Button>().interactable = true;

        Content = Instantiate(PrefabVideosettings, transform);

        GetComponent<Image>().color = VideoButton.GetComponent<Image>().color;
    }

    public void Audio()
    {
        VideoButton.GetComponent<Button>().interactable = true;
        AudioButton.GetComponent<Button>().interactable = false;
        CreditButton.GetComponent<Button>().interactable = true;

        GetComponent<Image>().color = AudioButton.GetComponent<Image>().color;   
    }

    public void Credits()
    {
        VideoButton.GetComponent<Button>().interactable = true;
        AudioButton.GetComponent<Button>().interactable = true;
        CreditButton.GetComponent<Button>().interactable = false;

        GetComponent<Image>().color = CreditButton.GetComponent<Image>().color;   
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
