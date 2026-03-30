using UnityEngine;
using System.Collections.Generic;

public class Projects : MonoBehaviour
{
    private ProjectData[] PersonalProjects;
    private ProjectData[] GroupProjects;
    private ProjectData[] OpenProjects;


    private void Start()
    {
        PersonalProjects = Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Personal");
        GroupProjects = Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Group");
        OpenProjects = Resources.LoadAll<ProjectData>("ScriptableObjects/Projects/Open");
    }
}
