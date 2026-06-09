using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataCardUI : MonoBehaviour
{
    [SerializeField] public Image border;
    [SerializeField] public Image background;
    [SerializeField] public Image icon;
    [SerializeField] public Image Interoperability;

    public DataCard dataCard;
    public bool isSelected = false;
    private bool isInteractable = true;

    public Action<GameObject, bool> OnHover;

    public void Initialize(DataCard dataCard, bool isInteractable)
    {
        this.dataCard = dataCard;
        icon.sprite = dataCard.CardType.dataIcon;
        background.color = dataCard.Color;
        this.isInteractable = isInteractable;

        if (dataCard.IsCorrectType)
        {
            Interoperability.gameObject.SetActive(false);
        }
    }

    public void Click()
    {
        if(!isInteractable) return;

        if (isSelected)
        {
            isSelected = false;
            border.color = new Color(0.0f, 1, 0.5f, 0);
        }
        else
        {
            isSelected = true;
            border.color = new Color(0.0f, 1, 0.5f, 1);
        }
    }
}
