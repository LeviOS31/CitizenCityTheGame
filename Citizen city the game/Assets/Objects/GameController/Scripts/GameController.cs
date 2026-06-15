using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] List<Color> availableColors = new List<Color>();
    [SerializeField] PlayerContainerUI playerContainer;
    [SerializeField] ProjectController projectController;
    [SerializeField] DataSpacesController dataSpaceController;
    [SerializeField] DistributeDataFromDataSpace distribute;
    [SerializeField] ConsultantManagerUI consultantManagerUI;
    [SerializeField] AIController aiController;
    [SerializeField] ScoreDemoManager scoreDemoManager;
    [SerializeField] Canvas GameUI;
    [SerializeField] RadarFiller EndScreen;
    [SerializeField] int scorelimit;
    public List<Player> players = new List<Player>();
    public static Player activePlayer;
    public int totalPlayers = 4;
    public int roundNumber = 0;
    public int turnNumber = 0;
    public int enableTutorial = 0; //0 if you want it enabled, other number if you dont
    private ConnectorNode[] connectorNodes;
    private List<PlayerCardUI> playerCards;

    public static DataCardType[] dataCardTypes; //TODO: Make private again
    public static Action<Player> NewTurn;
    public static Action UpdateUI;
    public static Action<Player, ProjectData> Completedproject;
    public static Action<GameObject> OpenWindow;

    bool done = false;

    void Awake()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            string playername = "";

            if(i == 0) playername = "Eindhoven";
            else if(i == 1) playername = "Veldhoven";
            else if(i == 2) playername = "Veghel";
            else if(i == 3) playername = "Geldrop";

            Player player = CreatePlayer(playername, availableColors[i]);
            players.Add(player);
        }
    }

    async void Start()
    {
        roundNumber = 0;
        turnNumber = 0;

        dataCardTypes = Resources.LoadAll<DataCardType>("ScriptableObjects/DataCardTypes");        
        
        playerCards = playerContainer.Initialize(players);
        aiController.Initialize(this, players, projectController, dataSpaceController, distribute, consultantManagerUI.consultantManager);

        activePlayer = players[0];

        foreach (PlayerCardUI playerCardUI in playerCards)
        {
            playerCardUI.ToggleInteractability(activePlayer);
        }

        await Task.Delay(500);
        StartNewRound();

        //Debug.Log("Active Player: " + activePlayer.name);
        //Debug.Log("Active Player: " + activePlayer.color);

        projectController.ReloadProjects(activePlayer);
        dataSpaceController.ReloadPlayer(activePlayer);

        TurnHistory.AddTurnAction.Invoke("Started the game");

        PlayerPrefs.SetInt("TutorialCompleted", enableTutorial); 

        if (PlayerPrefs.GetInt("TutorialCompleted", enableTutorial) != 1)
        {
            Tutorial.AdvanceTutorial();
        }
        else
        {
            Tutorial.Tutorialposition = 29;
            Tutorial.AdvanceTutorial();
        }
    }

    private void Update()
    {
        if (done) return; 

        SpelerData speler = scoreDemoManager.spelers.FirstOrDefault(p => p.algemeneScore > scorelimit);
        if (speler != null)
        {
            done = true;
            EndScreen.gameObject.SetActive(true);
            EndScreen.EndGame(scoreDemoManager.spelers);
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            done = true;
            EndScreen.gameObject.SetActive(true);
            EndScreen.EndGame(scoreDemoManager.spelers);
        }
    }

    private void StartNewRound()
    {
        if (Tutorial.Tutorialposition == 12 || Tutorial.Tutorialposition == 19)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }

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
        if (Tutorial.Tutorialposition == 11 || Tutorial.Tutorialposition == 18)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }

        activePlayer.money += 300;  

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
            TurnHistory.AddTurnAction.Invoke("Round " + roundNumber + " started");
            StartNewRound();
        }

        TurnHistory.AddTurnAction.Invoke("<color=#" + ColorUtility.ToHtmlStringRGB(activePlayer.color) + ">" + activePlayer.name + "'s turn</color>");

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


    }

    private Player CreatePlayer(string name, Color color)
    {
        return new Player(name, color);
    }
}
