using System.Collections.Generic;
using UnityEngine;
public enum ProjectType
{
    Personal,
    Group,
    Open
}

[CreateAssetMenu(fileName = "NewProject", menuName = "Project/New Project")]
public class ProjectData : ScriptableObject
{
    public string ProjectName;
    [Tooltip("Personal: all data from the same player used in project \n Group: data from different municipalities needed for project completion \n Open: for the multiplier project")]
    public ProjectType ProjectType;
    public Sprite ProjectIcon;
    public string ProjectDescription;
    public List<DataRequired> NeededData;
    public int ScoreValue;
}

public class DataRequired
{
    public DataCardType CardType;
    public Color Color;
    public bool IsMet;
}