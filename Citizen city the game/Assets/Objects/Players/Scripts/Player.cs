using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{
    public Guid ID = new Guid();
    public string name = string.Empty;
    public int scorePerTurn = 0;
    public int score = 0;
    public float scoreMultiplier = 0f;
    public int money = 1500;
    public Color color;
    public List<DataCard> cards = new List<DataCard>();
    public List<ProjectData> PersonalProjects = new List<ProjectData>();
    public List<ProjectData> ProvicialProjects = new List<ProjectData>();
    public List<DataSpaceData> DataSpaces = new List<DataSpaceData>();
    public bool isAI = false;

    private DataCardType[] dataCardTypes;

    public static event Action<Player> UIChangeplayer;
    public event Action<DataCard> OnDrawCard;
    public event Action UIChange;
    public event Action<List<DataCard>> OnReceiveTradeCards;
    public event Action<List<DataCard>, List<DataCard>> OnTradeCards;

    public Player(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }

    public void UpdateScore()
    {
        score += scorePerTurn;
    }

    //public void DrawProjectCard()
    //{
    //    projectCards.Add(new ProjectCard());
    //}

    public void DrawDataCard(DataCardType[] dataCardTypes)
    {
        DataCardType randomType = dataCardTypes.First(card => card.dataType == "Civil");

        DataCard dataCard = new DataCard(randomType, color);

        cards.Add(dataCard);

        OnDrawCard?.Invoke(dataCard);
        UIChangeplayer?.Invoke(this);
    }

    public DataCard DrawDataSpaceCard(DataCardType[] dataCardTypes)
    {
        DataCardType randomType = dataCardTypes[UnityEngine.Random.Range(0, dataCardTypes.Length)];

        DataCard dataCard = new DataCard(randomType, color, true);

        UIChange.Invoke();
        UIChangeplayer.Invoke(this);

        return dataCard;
    }

    public void TradeCards(List<DataCard> cardsReceived, List<DataCard> cardsGiven)
    {
        foreach (DataCard card in cardsReceived)
        {
            card.IsTradable = false;
            cards.Add(card);
        }

        foreach (DataCard card in cardsGiven)
        {
            card.IsTradable = false;
            cards.Remove(card);
        }

        OnTradeCards.Invoke(cardsReceived, cardsGiven);
    }

    public void ReceiveConsultantCards(List<DataCard> receivedCards)
    {
        Debug.Log("Received cards count " + receivedCards.Count);
        foreach (DataCard card in receivedCards)
        {
            card.IsTradable = true;
            cards.Add(card);
        }

        UIChange.Invoke();
        UIChangeplayer.Invoke(this);
    }

    //jasons version
    public void DrawDataSpaceCard(DataCard card)
    {
        cards.Add(card);

        OnDrawCard(card);
        UIChange.Invoke();
        UIChangeplayer.Invoke(this);
    }
}
