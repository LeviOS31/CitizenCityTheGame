using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProjectController : MonoBehaviour
{
    public ProjectData OpenProject;

    private Player activeplayer;
    private List<ProjectData> AllPersonalProjects;
    private List<ProjectData> AllGroupProjects;
    private List<ProjectData> AllOpenProjects;

    private void Awake()
    {
        AllPersonalProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Personal"));
        AllGroupProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Group"));
        AllOpenProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Open"));
    }

    private void Start()
    {
        OpenProject = GetOpenProject();
        GetComponent<ProjectsUI>().CheckCards += CheckPlayerCards;
    }

    private List<ProjectData> shuffle(List<ProjectData> list)
    {
        int i = list.Count;
        while (i > 1)
        {
            i--;
            int j = Random.Range(0, i + 1);
            ProjectData temp = list[j];
            list[j] = list[i];
            list[i] = temp;
        }
        return list;
    }

    public void ReloadProjects(Player player)
    {
        activeplayer = player;

        if (activeplayer.PersonalProjects.Count == 0 || activeplayer.PersonalProjects.Count(x => !x.IsDone) == 0)
        {
            activeplayer.PersonalProjects.Add(GetPersonalProject(activeplayer.color));
        }
        if (activeplayer.ProvicialProjects.Count == 0 || activeplayer.ProvicialProjects.Count(x => !x.IsDone) == 0)
        {
            activeplayer.ProvicialProjects.Add(GetGroupProject(activeplayer.color));
        }

        //Debug.Log(activeplayer.name + " " + activeplayer.color + " Projects:");

        foreach (ProjectData project in activeplayer.PersonalProjects)
        {
            //Debug.Log(project.Name);
        }
        foreach (ProjectData project in activeplayer.ProvicialProjects)
        {
            //Debug.Log(project.Name);
        }

        GetComponent<ProjectsUI>().ReloadProjectsUI(activeplayer.PersonalProjects, activeplayer.ProvicialProjects, OpenProject);
    }

    public ProjectData GetPersonalProject(Color color)
    {

        if (AllPersonalProjects.Count == 0)
        {
            Debug.LogWarning("No personal projects available.");
            return null;
        }

        shuffle(AllPersonalProjects);
        ProjectData project = Instantiate(AllPersonalProjects.First());

        foreach (DataRequired data in project.NeededData)
        {
            data.Color = color;
        }

        return project;
    }

    public ProjectData GetGroupProject(Color color)
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

    public ProjectData GetOpenProject()
    {
        if (AllOpenProjects.Count == 0)
        {
            Debug.LogWarning("No open projects available.");
            return null;
        }

        shuffle(AllOpenProjects);
        return Instantiate(AllOpenProjects.First());
    }

    public void CheckPlayerCards(DataRequired Data)
    {
        Debug.Log("checking " + Data.CardType + " with color " + Data.Color);
        if (activeplayer != null)
        {
            List<DataCard> remove = new List<DataCard>();

            foreach (DataCard card in activeplayer.cards)
            {
                if (card.CardType == Data.CardType && card.Color == Data.Color)
                {
                    Debug.Log("card found");
                    remove.Add(card);
                    Data.IsMet = true;
                    AudioSignalHandler.PlaySound.Invoke("ProjectPling");
                    break;
                }
            }

            foreach (DataCard card in remove)
            {
                activeplayer.cards.Remove(card);
                TurnHistory.AddTurnAction?.Invoke(activeplayer.name + " used a <color=#" + ColorUtility.ToHtmlStringRGB(card.Color) + ">" + card.CardType + " card</color> for a project");
            }
        }
    }
}
