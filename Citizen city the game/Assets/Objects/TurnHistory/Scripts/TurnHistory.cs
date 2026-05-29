using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnHistory : MonoBehaviour
{
    public TextMeshProUGUI History;
    public GameObject HistoryPanel;
    public Button openclosebutton;

    public static Action<string> AddTurnAction;

    void Awake()
    {
        AddTurnAction += addTurnAction;
    }

    void addTurnAction(string action)
    {
        History.text += "> " + action + "\n";

        Canvas.ForceUpdateCanvases();

        HistoryPanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }

    public void Open()
    {
        HistoryPanel.SetActive(true);
        openclosebutton.onClick.RemoveAllListeners();
        openclosebutton.GetComponentInChildren<TextMeshProUGUI>().text = "Close history";
        openclosebutton.onClick.AddListener(Close);
    }
    public void Close()
    {
        HistoryPanel.SetActive(false);
        openclosebutton.onClick.RemoveAllListeners();
        openclosebutton.GetComponentInChildren<TextMeshProUGUI>().text = "Open history";
        openclosebutton.onClick.AddListener(Open);
    }
}
