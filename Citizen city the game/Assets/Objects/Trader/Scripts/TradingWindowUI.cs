using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradingWindowUI : MonoBehaviour
{
    [SerializeField] AIController aiController;
    [SerializeField] GameObject dataCardPrefab;
    [SerializeField] GameObject tradingWindow;
    [SerializeField] GameObject offerContainer;
    [SerializeField] GameObject requestContainer;
    [SerializeField] GameObject acceptButton;
    [SerializeField] GameObject refuseButton;
    [SerializeField] GameObject cardHolder;

    Trader _trader = new Trader();

    Player initiatingPlayer;
    Player receivingPlayer;

    List<DataCardUI> offerSelection = new List<DataCardUI>();
    List<DataCardUI> requestSelection = new List<DataCardUI>();

    public static event Action<bool> OpenTradingWindow;

    private void Start()
    {
        _trader.aiController = aiController;

        _trader.TradeComplete += ClearValues;
    }

    public void OpenTradingMenu(Player receivingPlayer)
    {
        if (Tutorial.Tutorialposition == 15)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }

        RefuseTrade();

        tradingWindow.SetActive(true);
        acceptButton.SetActive(false);
        refuseButton.SetActive(false);
        OpenTradingWindow.Invoke(true);

        if (initiatingPlayer == null) 
        {
            initiatingPlayer = GameController.activePlayer;
            this.receivingPlayer = receivingPlayer;
        } 

        foreach (DataCard card in initiatingPlayer.cards)
        { 
            if (card.IsTradable)
            {
                DataCardUI cardInstance = Instantiate(dataCardPrefab).GetComponent<DataCardUI>();
                cardInstance.Initialize(card, true);
                offerSelection.Add(cardInstance);
                cardInstance.transform.SetParent(offerContainer.transform, false);
            }
        }

        foreach (DataCard card in receivingPlayer.cards)
        {
            if (card.IsTradable)
            {
                DataCardUI cardInstance = Instantiate(dataCardPrefab).GetComponent<DataCardUI>();
                cardInstance.Initialize(card, true);
                requestSelection.Add(cardInstance);
                cardInstance.transform.SetParent(requestContainer.transform, false);
            }
        }
    }

    public void CloseTradingMenu()
    {
        ClearValues();
    }

    public void StartTrade()
    {

        if (!receivingPlayer.isAI)
        {
            acceptButton.SetActive(true);
            refuseButton.SetActive(true);
        }

        List<DataCard> offer = new List<DataCard>();
        List<DataCard> request = new List<DataCard>();

        foreach (DataCardUI dataCard in offerSelection)
        {
            Destroy(dataCard.gameObject);
        }

        foreach (DataCardUI dataCard in requestSelection)
        {
            Destroy(dataCard.gameObject);
        }

        foreach (DataCardUI dataCard in offerSelection)
        {
            Debug.Log(offerSelection.Count);
            if (dataCard.isSelected)
            {
                offer.Add(dataCard.dataCard);
                DataCardUI cardInstance = Instantiate(dataCardPrefab).GetComponent<DataCardUI>();
                cardInstance.Initialize(dataCard.dataCard, false);
                cardInstance.transform.SetParent(offerContainer.transform, false);
            }
        }

        foreach (DataCardUI dataCard in requestSelection)
        {
            Debug.Log(requestSelection.Count);
            if (dataCard.isSelected)
            {
                request.Add(dataCard.dataCard);
                DataCardUI cardInstance = Instantiate(dataCardPrefab).GetComponent<DataCardUI>();
                cardInstance.Initialize(dataCard.dataCard, false);
                cardInstance.transform.SetParent(requestContainer.transform, false);
            }
        }

        _trader.StartTrade(initiatingPlayer, receivingPlayer, offer, request);
    }

    public void AcceptTrade()
    {
        _trader.AcceptTrade();
        ClearValues();
    }

    public void RefuseTrade()
    {
        _trader.RefuseTrade();
        ClearValues();
    }

    public void CounterOffer()
    {
        initiatingPlayer = receivingPlayer;
        receivingPlayer = GameController.activePlayer;
        offerSelection.Clear();
        requestSelection.Clear();
        OpenTradingMenu(receivingPlayer);
    }

    private void ClearValues()
    {
        if (Tutorial.Tutorialposition == 16)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }

        initiatingPlayer = null;
        receivingPlayer = null;

        foreach (DataCardUI dataCard in offerContainer.GetComponentsInChildren<DataCardUI>())
        {
            Destroy(dataCard.gameObject);
        }

        foreach (DataCardUI dataCard in requestContainer.GetComponentsInChildren<DataCardUI>())
        {
            Destroy(dataCard.gameObject);
        }

        offerSelection.Clear();
        requestSelection.Clear();
        acceptButton.SetActive(false);
        refuseButton.SetActive(false);
        OpenTradingWindow.Invoke(false);
        tradingWindow.SetActive(false);
    }
}
