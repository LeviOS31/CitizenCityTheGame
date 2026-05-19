using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsultantUI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject specializationContainer;
    [SerializeField] GameObject specializtionIconPrefab;
    [SerializeField] TMP_Text consultantName;
    [SerializeField] TMP_Text duration;
    [SerializeField] TMP_Text cost;
    [SerializeField] Image logo;

    private bool isMenuOpen = false;
    private Consultant consultant;

    public void Initialize(Consultant consultant)
    {
        this.consultant = consultant;

        foreach(DataCardType card in consultant.Specializations) 
        {
            GameObject instance = Instantiate(specializtionIconPrefab);
            specializtionIconPrefab.GetComponent<Image>().sprite = card.dataIcon;
            instance.transform.SetParent(specializationContainer.transform, false);
        }

        consultantName.text = consultant.Name;
        duration.text = consultant.Duration.ToString();
        cost.text = consultant.BasePrice.ToString();
        logo.sprite = consultant.Icon;
    }

    public void TriggerMenu()
    {
        if (isMenuOpen)
        {
            isMenuOpen = false;
            animator.SetBool("isMenuOpen", isMenuOpen);
        }
        else
        {
            isMenuOpen = true;
            animator.SetBool("isMenuOpen", isMenuOpen);
        }
    }

    public void Hire()
    {
        //DataCardUI[] dataCards = GetComponentsInChildren<DataCardUI>();

        //List<DataCard> selection = new List<DataCard>();
        //foreach (DataCardUI dataCard in dataCards)
        //{
        //    if (dataCard.isSelected)
        //    {
        //        selection.Add(dataCard.dataCard);
        //    }
        //}

        List<DataCard> selection = new List<DataCard>();

        foreach(DataCardType card in consultant.Specializations)
        {
            DataCard dataCard = new DataCard(card, GameController.activePlayer.color, false);
            selection.Add(dataCard);
        }

        consultant.HireConsultant(selection, GameController.activePlayer);
    }
}
