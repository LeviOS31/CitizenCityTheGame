using System.Collections.Generic;
using UnityEngine;
public enum ProjectType
{
    Local,
    Regional
}

// This class represents a project in the game. It contains information about the project, such as its name, type, description, required data, and rewards for completion.
[CreateAssetMenu(fileName = "NewProject", menuName = "Project/New Project")]
public class ProjectData : ScriptableObject
{
    public string Name;
    [Tooltip("Local: all data from the same player used in project \n Group: data from different municipalities needed for project completion \n Open: for the multiplier project")]
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
    public bool IsClaimed;

    // This method is called when the script is loaded or a value is changed in the inspector. It checks if the ScoreData array exceeds a maximum limit and resizes it if necessary.
    // it makes sure that the ScoreData array does not exceed a certain limit, preventing potential issues with data handling and ensuring that the project data remains manageable.
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

    // This method checks if the project requires a specific color in its needed data.
    public bool HasColor(Color targetColor)
    {

        foreach (DataRequired requirement in NeededData)
        {
            if (Vector4.Distance(requirement.Color, targetColor) < 0.01)
            {
                return true;
            }
        }
        return false;
    }
}

// This class represents the data required for a project. It contains information about the type of data card, its color, and whether the requirement has been met.
[System.Serializable]
public class DataRequired
{
    public DataCardType CardType;
    public Color Color;
    public bool IsMet;
}