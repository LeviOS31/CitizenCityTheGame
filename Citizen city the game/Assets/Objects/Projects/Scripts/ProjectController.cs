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
    }

    private List<ProjectData> shuffle (List<ProjectData> list)
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

        GetComponent<ProjectsUI>().ReloadProjectsUI(activeplayer.PersonalProjects, activeplayer.ProvicialProjects, OpenProject);
    }

    public ProjectData GetPersonalProject(Color color)
    {
        Debug.Log(AllGroupProjects.Count);

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
}
