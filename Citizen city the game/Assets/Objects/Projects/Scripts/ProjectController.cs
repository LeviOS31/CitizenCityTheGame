using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class ProjectController : MonoBehaviour
{
    public ProjectData OpenProject;

    private Player activeplayer;
    private List<ProjectData> AllPersonalProjects;
    private List<ProjectData> AllGroupProjects;
    private List<ProjectData> AllOpenProjects;

    public static Action ProjectDone;

    private void Awake()
    {
        AllPersonalProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Personal"));
        AllGroupProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Group"));
        AllOpenProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Open"));
    }

    private void Start()
    {
        GetComponent<ProjectsUI>().CheckCards += CheckPlayerCards;
        ProjectDone += GivePlayersScoreandMoney;
    }

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

    public void ReloadProjects(Player player)
    {
        activeplayer = player;

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

    public ProjectData GetLocalProject(Color color)
    {

        if (AllPersonalProjects.Count == 0)
        {
            Debug.LogWarning("No local projects available.");
            return null;
        }

        shuffle(AllPersonalProjects);
        ProjectData project = Instantiate(AllPersonalProjects.First());
        
        if (GameController.activePlayer.LocalProjects.Count == 0 && PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            project = Instantiate(AllPersonalProjects.First(x => x.Name == "Project Flow-Pure"));
        }

        foreach (DataRequired data in project.NeededData)
        {
            data.Color = color;
        }

        return project;
    }

    public ProjectData GetRegionalProject(Color color)
    {
        if (AllGroupProjects.Count == 0)
        {
            Debug.LogWarning("No group projects available.");
            return null;
        }

        shuffle(AllGroupProjects);
        foreach (ProjectData project in AllGroupProjects)
        {
            if (project.HasColor(color))
            {
                return Instantiate(project);
            }
        }

        Debug.LogWarning("No group projects found with the required color.");
        return null;
    }

    public void CheckPlayerCards(DataRequired Data)
    {
        Debug.Log("checking " + Data.CardType + " with color " + Data.Color);
        if (activeplayer != null)
        {
            List<DataCard> remove = new List<DataCard>();

            foreach (DataCard card in activeplayer.cards)
            {
                float colordiff = Vector4.Distance(card.Color, Data.Color);

                if (card.CardType == Data.CardType && colordiff < 0.05f)
                {
                    Debug.Log("card found");
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
    }

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
                Debug.Log($"[REWARD] {GameController.activePlayer.name} ontvangt €{project.ScoreMoney} voor project {project.Name}");
                GameController.Completedproject?.Invoke(GameController.activePlayer, project);

                project.IsClaimed = true;
                break;
            }
        }
    }
}
