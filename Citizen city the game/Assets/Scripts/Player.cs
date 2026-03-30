using System;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public Guid ID =  new Guid();
    public string name = string.Empty;
    public int scorePerTurn = 0;
    public int playerScore = 0;
    public float playerMultiplier = 0f;
    public Color playerColor;
    public List<DataCard> cards = new List<DataCard>();
    public List<ProjectCard> projectCards = new List<ProjectCard>();

    private DataCardType[] dataCardTypes;

    public void UpdateScore()
    {
        playerScore += scorePerTurn;
    }

    public void DrawProjectCard()
    {
        projectCards.Add(new ProjectCard());
    }

    public void DrawDataCard(DataCardType[] dataCardTypes)
    {
        DataCardType randomType = dataCardTypes[UnityEngine.Random.Range(0, dataCardTypes.Length)];

        DataCard dataCard = new DataCard(randomType, playerColor);

        cards.Add(dataCard);
    }

    public DataCard DrawDataSpaceCard(DataCardType[] dataCardTypes)
    {
        DataCardType randomType = dataCardTypes[UnityEngine.Random.Range(0, dataCardTypes.Length)];

        DataCard dataCard = new DataCard(randomType, playerColor, false);

        return dataCard;
    }
}
