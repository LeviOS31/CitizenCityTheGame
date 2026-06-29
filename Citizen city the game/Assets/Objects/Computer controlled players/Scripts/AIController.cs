using System.Collections.Generic;
using UnityEngine;

// This class is responsible for controlling the AI players in the game.
public class AIController : MonoBehaviour
{
    public int AICount = 0;
    public List<AIPlayer> AIPlayers = new List<AIPlayer>();
    public GameController gameController;
    public int ThinkingDelay = 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(GameController gameController, List<Player> players, ProjectController projectController, DataSpacesController dataSpacesController, DistributeDataFromDataSpace distribute, ConsultantManager consultantManager)
    {
        AICount = PlayerPrefs.GetInt("AICount", 0);

        if (AICount == 0)
        {
            this.gameObject.SetActive(false);
        }

        this.gameController = gameController;

        Debug.Log("Initializing AI Controller with " + AICount + " AI players.");
        for (int i = 1; i <= AICount; i++)
        {
            Player p = players[players.Count - i];
            p.isAI = true;
            AIPlayer aiPlayer = new AIPlayer();
            aiPlayer.Initialize(p, players, gameController, projectController, dataSpacesController, distribute, consultantManager, ThinkingDelay);
            AIPlayers.Add(aiPlayer);
            Debug.Log("Initialized AI Player: " + p.name);
        }
    }

    public void TakeTurn(Player ai)
    {
        // Delegate to the matching AIPlayer instance
        AIPlayer aiPlayer = AIPlayers.Find(x => x.self == ai);
        if (aiPlayer != null)
        {
            aiPlayer.TakeTurn();
        }
        else
        {
            Debug.LogWarning("No AIPlayer instance found for " + ai.name + ", ending turn.");
            gameController.EndTurn();
        }
    }

    public bool TradingRequest(Player ai)
    {
        // Delegate to the matching AIPlayer instance
        AIPlayer aiPlayer = AIPlayers.Find(x => x.self == ai);
        if (aiPlayer != null)
        {
            bool decision = aiPlayer.TradingDecision();
            if (!decision)
            {
                Debug.Log("AI Player " + ai.name + " decided not to trade.");
            }
            return decision;
        }
        else
        {
            Debug.LogWarning("No AIPlayer instance found for " + ai.name + ", ending turn.");
            return false;
        }
    }
}
