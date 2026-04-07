using System.Collections.Generic;
using UnityEngine;

public class ProjectUI : MonoBehaviour
{
    public GameObject ProjectUIObject;
    public GameObject ProjectPrefab;

    List<ProjectData> Personalprojects = new List<ProjectData>();
    List<ProjectData> Provincialprojects = new List<ProjectData>();
    ProjectData openproject;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadProjects(Player player)
    {
        List<ProjectData> dummyplayerdata = new List<ProjectData>();//TODO: replace with player.projects

        foreach (ProjectData project in dummyplayerdata)
        {
            if (project.Type == ProjectType.Personal)
            {
                Personalprojects.Add(project);
            }
            else if (project.Type == ProjectType.Provicial)
            {
                Provincialprojects.Add(project);
            }
            else
            {
                openproject = project;
            }
        }

        GameObject personal = ProjectUIObject.transform.Find("Personal").gameObject;
        GameObject provincial = ProjectUIObject.transform.Find("Provincial").gameObject;
        GameObject open = ProjectUIObject.transform.Find("openproject").gameObject;

        foreach (Transform child in personal.transform) { 
            Destroy(child.gameObject);
        }
        foreach (Transform child in provincial.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in open.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
