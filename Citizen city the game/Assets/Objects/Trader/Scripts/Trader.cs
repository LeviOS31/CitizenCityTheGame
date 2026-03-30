using System.Collections.Generic;
using UnityEngine;

public class Trader
{
    Player initiatingPlayer;
    Player receivingPlayer;

    List<DataCard> cardsOffered = new List<DataCard>();
    List<DataCard> cardsRequested = new List<DataCard>();

    public void StartTrade(Player initiatingPlayer, Player receivingPlayer, List<DataCard> cardsOffered, List<DataCard> cardsRequested)
    {
        this.initiatingPlayer = initiatingPlayer;
        this.receivingPlayer = receivingPlayer;
        this.cardsOffered = cardsOffered;
        this.cardsRequested = cardsRequested;
    }

    public void AcceptTrade()
    {

        foreach(DataCard card in cardsOffered)
        {
            card.IsTradable = false;
            receivingPlayer.cards.Add(card);
            initiatingPlayer.cards.Remove(card);
        }

        foreach (DataCard card in cardsRequested)
        {
            card.IsTradable = false;
            initiatingPlayer.cards.Add(card);
            receivingPlayer.cards.Remove(card);
        }

        ClearTradeVariables();
    }

    public void RefuseTrade()
    {
        ClearTradeVariables();
    }

    public void ClearTradeVariables()
    {
        initiatingPlayer = null;
        receivingPlayer = null;
        cardsOffered = new List<DataCard>();
        cardsRequested = new List<DataCard>();
    }
}
