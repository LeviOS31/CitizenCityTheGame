using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradingWindowUI : MonoBehaviour
{
    [SerializeField] GameObject dataCardPrefab;
    [SerializeField] GameObject offerContainer;
    [SerializeField] GameObject requestContainer;

    Trader _trader = new Trader();

    Player initiatingPlayer;
    Player receivingPlayer;

    List<DataCardUI> offerSelection = new List<DataCardUI>();
    List<DataCardUI> requestSelection = new List<DataCardUI>();

    public void OpenTradingMenu(Player receivingPlayer)
    {
        if (initiatingPlayer == null) 
        {
            initiatingPlayer = FindAnyObjectByType<GameController>().activePlayer;
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

    public void StartTrading()
    {
        List<DataCard> offer = new List<DataCard>();
        List<DataCard> request = new List<DataCard>();

        foreach (DataCardUI element in offerSelection)
        {
            if (element.isSelected)
            {
                offer.Add(element.dataCard);
            }
        }

        foreach (DataCardUI element in requestSelection)
        {
            if (element.isSelected)
            {
                request.Add(element.dataCard);
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
        receivingPlayer = FindAnyObjectByType<GameController>().activePlayer;
        offerSelection.Clear();
        requestSelection.Clear();
        OpenTradingMenu(receivingPlayer);
    }

    private void ClearValues()
    {
        initiatingPlayer = null;
        receivingPlayer = null;

        foreach (DataCardUI element in offerSelection)
        {
            Destroy(element.gameObject);
        }

        foreach (DataCardUI element in requestSelection)
        {
            Destroy(element.gameObject);
        }

        offerSelection.Clear();
        requestSelection.Clear();
    }
}
