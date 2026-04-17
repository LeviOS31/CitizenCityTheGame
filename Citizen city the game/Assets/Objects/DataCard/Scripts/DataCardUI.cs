using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Image border;
    [SerializeField] public Image background;
    [SerializeField] public Image icon;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover?.Invoke(gameObject, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHover?.Invoke(gameObject, false);
    }
}
