using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] List<Color> availableColors = new List<Color>();
    [SerializeField] PlayerContainerUI playerContainer;
    public List<Player> players = new List<Player>();
    public Player activePlayer;
    public int totalPlayers = 4;
    public int roundNumber = 0;
    public int turnNumber = 0;

    public DataCardType[] dataCardTypes; //TODO: Make private again
    private ConnectorNode[] connectorNodes;
    private List<PlayerCardUI> playerCards;

    public Action<Player> NewTurn;

    void Start()
    {
        roundNumber = 0;
        turnNumber = 0;

        dataCardTypes = Resources.LoadAll<DataCardType>("ScriptableObjects/DataCardTypes");

        for (int i = 0; i < totalPlayers; i++)
        {
            Player player = CreatePlayer("Player " + (i + 1).ToString(), availableColors[i]);
            players.Add(player);
        }

        playerCards = playerContainer.Initialize(players);

        activePlayer = players[0];

        foreach (PlayerCardUI playerCardUI in playerCards)
        {
            playerCardUI.ToggleInteractability(activePlayer);
        }

        StartNewRound();
    }

    private void StartNewRound()
    {
        foreach (Player player in players)
        {
            player.DrawDataCard(dataCardTypes);
            player.UpdateScore();
        }

        if (connectorNodes == null)
        {
            connectorNodes = FindObjectsByType<ConnectorNode>(FindObjectsSortMode.None);
        }

        foreach (ConnectorNode connectorNode in connectorNodes)
        {
            if (connectorNode.isConnected)
            {
                Player playerOne = connectorNode.playerOne;
                Player playerTwo = connectorNode.playerTwo;

                playerOne.cards.Add(playerTwo.DrawDataSpaceCard(dataCardTypes));
                playerTwo.cards.Add(playerOne.DrawDataSpaceCard(dataCardTypes));
            }
        }

        NewTurn.Invoke(activePlayer);
    }

    public void EndTurn()
    {
        turnNumber++;
        if (turnNumber < players.Count)
        {
            activePlayer = players[turnNumber];
        }
        else
        {
            turnNumber = 0;
            roundNumber++;
            activePlayer = players[0];
            StartNewRound();
        }

        foreach (PlayerCardUI playerCard in playerCards) 
        {
            playerCard.UpdateUI();
            playerCard.ToggleInteractability(activePlayer);
        }

        NewTurn.Invoke(activePlayer);
    }

    private Player CreatePlayer(string name, Color color)
    {
        return new Player(name, color);
    }
}
