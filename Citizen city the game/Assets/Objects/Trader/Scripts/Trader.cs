using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trader
{
    public AIController aiController;

    Player initiatingPlayer;
    Player receivingPlayer;

    List<DataCard> cardsOffered = new List<DataCard>();
    List<DataCard> cardsRequested = new List<DataCard>();

    public Action TradeComplete;

    public async Task StartTrade(Player initiatingPlayer, Player receivingPlayer, List<DataCard> cardsOffered, List<DataCard> cardsRequested)
    {
        this.initiatingPlayer = initiatingPlayer;
        this.receivingPlayer = receivingPlayer;
        this.cardsOffered = cardsOffered;
        this.cardsRequested = cardsRequested;

        if (receivingPlayer.isAI)
        {
            bool awnser = aiController.TradingRequest(receivingPlayer);
            await Task.Delay(500);
            if (awnser)
            {
                AcceptTrade();
                TradeComplete.Invoke();
            }
            else
            {
                RefuseTrade();
                TradeComplete.Invoke();
            }
        }
    }

    public void AcceptTrade()
    {
        initiatingPlayer.TradeCards(cardsRequested, cardsOffered);

        receivingPlayer.TradeCards(cardsOffered, cardsRequested);

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
