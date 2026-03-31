using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Projects : MonoBehaviour
{
    private List<ProjectData> PersonalProjects;
    private List<ProjectData> GroupProjects;
    private List<ProjectData> OpenProjects;

    private void Start()
    {
        PersonalProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Personal"));
        GroupProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Group"));
        OpenProjects = new List<ProjectData>(Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Open"));
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

    public ProjectData GetPersonalProject(Color color)
    {
        if (PersonalProjects.Count == 0)
        {
            Debug.LogWarning("No personal projects available.");
            return null;
        }

        shuffle(PersonalProjects);
        ProjectData project = PersonalProjects.First();

        foreach (DataRequired data in project.NeededData)
        {
            data.Color = color;
        }

        return project;
    }

    public ProjectData GetGroupProject(Color color)
    {
        if (GroupProjects.Count == 0)
        {
            Debug.LogWarning("No group projects available.");
            return null;
        }

        shuffle(GroupProjects);
        foreach (ProjectData project in GroupProjects)
        {
            if (project.HasRequiredColor(color))
            {
                return project;
            }
        }

        Debug.LogWarning("No group projects found with the required color.");
        return null;
    }

    public ProjectData GetOpenProject()
    {
        if (OpenProjects.Count == 0)
        {
            Debug.LogWarning("No open projects available.");
            return null;
        }

        shuffle(OpenProjects);
        return OpenProjects.First();
    }
}
