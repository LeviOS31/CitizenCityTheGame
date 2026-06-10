using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataCardUI : MonoBehaviour
{
    [SerializeField] public Image border;
    [SerializeField] public Image background;
    [SerializeField] public Image icon;
    [SerializeField] public TMP_Text countText;

    public DataCard dataCard;
    public bool isSelected = false;
    private bool isInteractable = true;

    public Action<GameObject, bool> OnHover;

    public void Initialize(DataCard dataCard, bool isInteractable, int count = 1)
    {
        this.dataCard = dataCard;
        icon.sprite = dataCard.CardType.dataIcon;
        background.color = dataCard.Color;
        this.isInteractable = isInteractable;

        UpdateCount(count);
    }

    public void UpdateCount(int count)
    {
        if (countText != null)
        {
            // Only show the multiplier text if you possess more than 1
            countText.gameObject.SetActive(count > 1);
            countText.text = $"x{count}";
        }
    }

    public void Click()
    {
        if (!isInteractable) return;

        isSelected = !isSelected;
        border.color = isSelected ? new Color(0.0f, 1f, 0.5f, 1f) : new Color(0.0f, 1f, 0.5f, 0f);
    }
}
