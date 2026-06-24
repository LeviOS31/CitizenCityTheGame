using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

// This class is responsible for managing projects in the game. It handles loading project data, checking player cards against project requirements, and rewarding players for completed projects.
public class ProjectController : MonoBehaviour
{
    public ProjectData OpenProject;

    private Player activeplayer;
    private List<ProjectData> AllLocalProjects;
    private List<ProjectData> AllRegionalProjects;

    public static Action ProjectDone;

    // This initializes the lists of local and regional projects by loading them from the Resources folder.
    private void Awake()
    {
        AllLocalProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Local"));
        AllRegionalProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Regional"));
    }

    private void Start()
    {
        GetComponent<ProjectsUI>().CheckCards += CheckPlayerCards;
        ProjectDone += GivePlayersScoreandMoney;
    }

    // This method shuffles a list of ProjectData objects.
    private List<ProjectData> shuffle(List<ProjectData> list)
    {
        int i = list.Count;
        while (i > 1)
        {
            i--;
            int j = UnityEngine.Random.Range(0, i + 1);
            ProjectData temp = list[j];
            list[j] = list[i];
            list[i] = temp;
        }
        return list;
    }

    // This method reloads the projects for a given player, ensuring they have a minimum number of local and regional projects available.
    public void ReloadProjects(Player player)
    {
        activeplayer = player;

        // Ensure the player has at least 3 local projects.
        if (activeplayer.LocalProjects.Count == 0)
        {
            activeplayer.LocalProjects.Insert(0,GetLocalProject(activeplayer.color));
            activeplayer.LocalProjects.Insert(0,GetLocalProject(activeplayer.color));
            activeplayer.LocalProjects.Insert(0,GetLocalProject(activeplayer.color));
        }
        else if (activeplayer.LocalProjects.Count(x => !x.IsDone) < 3)
        {
            int needed = activeplayer.LocalProjects.Count(x => !x.IsDone);

            for (int i = 0; i < needed; i++)
            {
                activeplayer.LocalProjects.Insert(0,GetLocalProject(activeplayer.color));
            }
        }

        // Ensure the player has at least 2 regional projects.
        if (activeplayer.RegionalProjects.Count == 0)
        {
            activeplayer.RegionalProjects.Insert(0,GetRegionalProject(activeplayer.color));
            activeplayer.RegionalProjects.Insert(0,GetRegionalProject(activeplayer.color));
        }
        else if (activeplayer.RegionalProjects.Count(x => !x.IsDone) < 2)
        {
            int needed = activeplayer.RegionalProjects.Count(x => !x.IsDone);

            for (int i = 0; i < needed; i++)
            {
                activeplayer.RegionalProjects.Insert(0,GetRegionalProject(activeplayer.color));
            }
        }

        GetComponent<ProjectsUI>().ReloadProjectsUI(activeplayer.LocalProjects, activeplayer.RegionalProjects, OpenProject);
    }


    // This method retrieves a local project for the player and sets the required data color to the color of the player. If no local projects are available, it logs a warning and returns null.
    public ProjectData GetLocalProject(Color color)
    {

        if (AllLocalProjects.Count == 0)
        {
            Debug.LogWarning("No local projects available.");
            return null;
        }

        shuffle(AllLocalProjects);
        ProjectData project = Instantiate(AllLocalProjects.First());

        // If the player has no local projects and hasn't completed the tutorial, assign a specific project.
        if (GameController.activePlayer.LocalProjects.Count == 0 && PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            project = Instantiate(AllLocalProjects.First(x => x.Name == "Project Flow-Pure"));
        }

        // Set the color of the required data for the project to match the player's color.
        foreach (DataRequired data in project.NeededData)
        {
            data.Color = color;
        }

        return project;
    }

    // This method retrieves a regional project for the player that matches the specified color. If no matching projects are found, it logs a warning and returns null.
    public ProjectData GetRegionalProject(Color color)
    {
        if (AllRegionalProjects.Count == 0)
        {
            Debug.LogWarning("No group projects available.");
            return null;
        }

        shuffle(AllRegionalProjects);
        foreach (ProjectData project in AllRegionalProjects)
        {
            if (project.HasColor(color))
            {
                return Instantiate(project);
            }
        }

        Debug.LogWarning("No group projects found with the required color.");
        return null;
    }


    // This method checks if the active player has the required cards to meet the project's requirements. If a matching card is found, it is removed from the player's hand, and feedback is provided. If no matching card is found, an error feedback is shown.
    public void CheckPlayerCards(DataRequired Data)
    {
        if (activeplayer == null)
        { return; }

        List<DataCard> remove = new List<DataCard>();

        // Iterate through the active player's cards to find a matching card for the data the player clicked on.
        foreach (DataCard card in activeplayer.cards)
        {
            float colordiff = Vector4.Distance(card.Color, Data.Color);

            if (card.CardType == Data.CardType && colordiff < 0.05f)
            {
                remove.Add(card);
                Data.IsMet = true;

                FeedbackManager.Instance.ShowFeedback($"De {card.CardType.dataType} kaart is ingeleverd voor het project", FeedbackType.Success);

                AudioSignalHandler.PlaySound.Invoke("ProjectPling");
                break;
            }
        }

        if (remove.Count == 0)
        {
            FeedbackManager.Instance.ShowFeedback($"Je hebt geen kaart van type {Data.CardType.dataType}", FeedbackType.Error);
            return;
        }

        foreach (DataCard card in remove)
        {
            activeplayer.cards.Remove(card);
            TurnHistory.AddTurnAction?.Invoke(activeplayer.name + " used a <color=#" + ColorUtility.ToHtmlStringRGB(card.Color) + ">" + card.CardType.dataType + " card</color> for a project");
        }

        Player.FireUIChangePlayer(activeplayer);
        activeplayer.FireUIChange();
    }

    // This method rewards the active player with money for completed projects that have not yet been claimed. It iterates through the player's local and regional projects, checks if they are done and not claimed, and then adds the score money to the player's total. invokes an event for completed projects.
    public void GivePlayersScoreandMoney() 
    {
        List<ProjectData> playerprojects = new List<ProjectData>();
        playerprojects.AddRange(GameController.activePlayer.LocalProjects);
        playerprojects.AddRange(GameController.activePlayer.RegionalProjects);

        foreach (ProjectData project in playerprojects)
        {
            if (project.IsDone && !project.IsClaimed)
            {
                GameController.activePlayer.money += project.ScoreMoney;
                GameController.Completedproject?.Invoke(GameController.activePlayer, project);

                project.IsClaimed = true;
                break;
            }
        }
    }
}
