using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DataRequirement
{
    public string requirementName;
    public bool isMet;
}

[CreateAssetMenu(fileName = "NewProject", menuName = "Project Object/New Project")]
public class Project : ScriptableObject
{
    public string Projectdescription;
    public int ProjectBudget;
    public List<DataRequirement> DataList = new List<DataRequirement>();
}
