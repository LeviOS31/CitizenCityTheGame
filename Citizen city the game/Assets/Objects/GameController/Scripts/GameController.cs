using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] public List<Player> players = new List<Player>();

    public int totalPlayers = 4;
    public int roundNumber = 0;
    public int turnNumber = 0;

    public Player activePlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roundNumber = 0;
        turnNumber = 0;
        activePlayer = players[0];
        //StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void StartRound()
    //{
    //    foreach (Player p in players)
    //    {
    //        p.DrawCard();
    //    }

    //    List<ConnectorNode> nodes = FindObjectsByType<ConnectorNode>(FindObjectsSortMode.None).ToList();

    //    foreach (ConnectorNode node in nodes)
    //    {
    //        node.DrawDataSpaceCard();
    //    }

    //    cardHolderUI.Render(activePlayer);

    //    foreach (PlayerCard p in FindObjectsByType<PlayerCard>(FindObjectsSortMode.None))
    //    {
    //        if (p.player == activePlayer)
    //        {
    //            p.Disable();
    //        } 
    //        else
    //        {
    //            p.Enable();
    //        }

    //        p.UpdateScore();
    //    }
    //}

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
            //StartRound();
        }

        foreach (PlayerCard p in FindObjectsByType<PlayerCard>(FindObjectsSortMode.None))
        {
            if (p.player == activePlayer)
            {
                p.Disable();
            }
            else
            {
                p.Enable();
            }
        }
    }
}
