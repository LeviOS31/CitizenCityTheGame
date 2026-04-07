using System.Collections.Generic;
using UnityEngine;
public enum ProjectType
{
    Personal,
    Provicial,
    Open
}

[CreateAssetMenu(fileName = "NewProject", menuName = "Project/New Project")]
public class ProjectData : ScriptableObject
{
    public string Name;
    [Tooltip("Personal: all data from the same player used in project \n Group: data from different municipalities needed for project completion \n Open: for the multiplier project")]
    public ProjectType Type;
    [Tooltip("Image not required")]
    public Sprite Image;
    public string Description;
    public List<DataRequired> NeededData;
    public int ScoreValue;

    public bool HasColor( Color targetColor)
    {
        foreach (DataRequired requirement in NeededData)
        {
            if (requirement.Color == targetColor)
            {
                return true;
            }
        }
        return false;
    }

}

public class DataRequired
{
    public DataCardType CardType;
    public Color Color;
    public bool IsMet;
}