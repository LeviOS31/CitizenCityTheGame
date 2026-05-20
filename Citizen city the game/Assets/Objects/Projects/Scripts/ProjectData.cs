using System.Collections.Generic;
using UnityEngine;
public enum ProjectType
{
    Personal,
    Provincial,
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
    [TextArea(3, 10)]
    public string Description;
    public List<DataRequired> NeededData = new List<DataRequired>();
    [Tooltip("money the player gets when the player completes the project")]
    public int ScoreMoney;
    [Tooltip("Score that player gets in form of the data the player used")]
    public int[] ScoreData;
    public bool IsDone;

    private void OnValidate()
    {
        // Set your maximum limit here
        int maxLimit = 5;

        if (ScoreData != null && ScoreData.Length > maxLimit)
        {
            Debug.LogWarning($"Array limited to {maxLimit} items!");

            // Resize the array back to the maximum allowed limit
            System.Array.Resize(ref ScoreData, maxLimit);
        }
    }

    public bool HasColor(Color targetColor)
    {
        foreach (DataRequired requirement in NeededData)
        {;
            if (Vector4.Distance(requirement.Color, targetColor) < 0.01)
            {
                return true;
            }
        }
        return false;
    }

}

[System.Serializable]
public class DataRequired
{
    public DataCardType CardType;
    public Color Color;
    public bool IsMet;
}