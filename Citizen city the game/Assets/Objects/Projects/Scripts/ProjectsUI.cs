using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ProjectsUI : MonoBehaviour
{
    public GameObject PrefabProjectPaper;

    public void ReloadProjectsUI(List<ProjectData> personalProjects, List<ProjectData> provincialProjects, ProjectData OpenProject)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ProjectPaper");
        foreach (GameObject obj in objects) {
            Destroy(obj);
        }

        foreach (ProjectData project in personalProjects)
        {
            GameObject projectPaper = Instantiate(PrefabProjectPaper, transform);
            projectPaper.GetComponent<ProjectPrefab>().project = project;
            projectPaper.GetComponent<ProjectPrefab>().Initialize();
        }
    }
}
