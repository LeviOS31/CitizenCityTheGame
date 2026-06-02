using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ConsultantManager
{
    public Consultant[] consultantOptions { get; private set; }

    private Player activePlayer;

    public Dictionary<Player, List<HiredConsultant>> playerHiredConsultants = new Dictionary<Player, List<HiredConsultant>>();

    private GameController gameController;
    private bool subscribed = false;

    private void UpdateInProgressConsultants(Player player) 
    {
        activePlayer = player;

        if (!playerHiredConsultants.ContainsKey(activePlayer)) return;

        List<HiredConsultant> hiredConsultants = playerHiredConsultants[activePlayer];

        for (int i = hiredConsultants.Count - 1; i >= 0; i--)
        {
            List<DataCard> consultResult = hiredConsultants[i].UpdateConsultantProgress();

            if (consultResult == null)
            {
                continue;
            }
            else
            {
                activePlayer.ReceiveConsultantCards(consultResult);
                int index = playerHiredConsultants[activePlayer].FindIndex(c => c == hiredConsultants[i]);
                playerHiredConsultants[activePlayer][index].OnHire -= HireConsultant;
                playerHiredConsultants[activePlayer].Remove(hiredConsultants[i]);
            }
        }

        if (playerHiredConsultants.Count == 0)
        {
            GameController.NewTurn -= UpdateInProgressConsultants;
            subscribed = false;
        }
    }

    public void HireConsultant(HiredConsultant hiredConsultant)
    {
        if (!subscribed)
        {
            GameController.NewTurn += UpdateInProgressConsultants;
            subscribed = true;
        }

        if (!playerHiredConsultants.ContainsKey(hiredConsultant.requestingPlayer)) playerHiredConsultants.Add(hiredConsultant.requestingPlayer, new List<HiredConsultant>());

        playerHiredConsultants[hiredConsultant.requestingPlayer].Add(hiredConsultant);
        TurnHistory.AddTurnAction?.Invoke($"{hiredConsultant.requestingPlayer.name} hired {hiredConsultant.Name}");

        if (Tutorial.Tutorialposition == 7 && playerHiredConsultants[hiredConsultant.requestingPlayer].Count == 2)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }
    }

    public void SetConsultantOptions(Consultant[] consultants)
    {
        consultantOptions = consultants;

        foreach (Consultant consultant in consultants) 
        {
            consultant.OnHire += HireConsultant;
        }
    }
}
