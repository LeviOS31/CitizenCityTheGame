using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] List<Color> availableColors = new List<Color>();
    [SerializeField] PlayerContainerUI playerContainer;
    [SerializeField] ProjectController projectController;
    [SerializeField] DataSpacesController dataSpaceController;
    [SerializeField] ConsultantManagerUI consultantManagerUI;
    [SerializeField] AIController aiController;
    [SerializeField] Canvas GameUI;
    public List<Player> players = new List<Player>();
    public static Player activePlayer;
    public int totalPlayers = 4;
    public int roundNumber = 0;
    public int turnNumber = 0;

    public DataCardType[] dataCardTypes; //TODO: Make private again
    private ConnectorNode[] connectorNodes;
    private List<PlayerCardUI> playerCards;

    public static Action<Player> NewTurn;
    public static Action UpdateUI;
    public static Action<Player> Completedproject;

    void Awake()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            Player player = CreatePlayer("Player " + (i + 1).ToString(), availableColors[i]);
            players.Add(player);
        }
    }

    void Start()
    {
        roundNumber = 0;
        turnNumber = 0;

        dataCardTypes = Resources.LoadAll<DataCardType>("ScriptableObjects/DataCardTypes");        
        
        playerCards = playerContainer.Initialize(players);
        aiController.Initialize(this, players, projectController, dataSpaceController, consultantManagerUI.consultantManager);

        activePlayer = players[0];

        foreach (PlayerCardUI playerCardUI in playerCards)
        {
            playerCardUI.ToggleInteractability(activePlayer);
        }

        StartNewRound();

        //Debug.Log("Active Player: " + activePlayer.name);
        //Debug.Log("Active Player: " + activePlayer.color);

        projectController.ReloadProjects(activePlayer);
        dataSpaceController.ReloadPlayer(activePlayer);
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
        activePlayer.money += 100;  

        turnNumber++;

        foreach (PlayerCardUI playerCard in playerCards)
        {
            playerCard.UpdateUI();
            playerCard.ToggleInteractability(activePlayer);
        }

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

        projectController.ReloadProjects(activePlayer);
        dataSpaceController.ReloadPlayer(activePlayer);
        dataSpaceController.ResetDataSpaceCollectionForNewTurn();

        foreach (PlayerCardUI playerCard in playerCards) 
        {
            playerCard.UpdateUI();
            playerCard.ToggleInteractability(activePlayer);
        }

        if (activePlayer.isAI)
        {
            Debug.Log("player is AI");
            Button[] allButtons = GameUI.GetComponentsInChildren<Button>();

            foreach (Button button in allButtons)
            {
                button.interactable = false;
            }
        }
        else
        {
            Button[] allButtons = GameUI.GetComponentsInChildren<Button>();

            foreach (Button button in allButtons)
            {
                button.interactable = true;
            }
        }

        NewTurn.Invoke(activePlayer);

        if (activePlayer.isAI)
        {
            aiController.TakeTurn(activePlayer);
        }

        List<ProjectData> playerprojects = new List<ProjectData>();
        playerprojects.AddRange(activePlayer.PersonalProjects);
        playerprojects.AddRange(activePlayer.ProvicialProjects);

        foreach (ProjectData project in playerprojects)
        {
            if (project.IsDone && !project.IsClaimed)
            {
                Completedproject.Invoke(activePlayer);
                break;
            }
        }
    }

    private Player CreatePlayer(string name, Color color)
    {
        return new Player(name, color);
    }
}
