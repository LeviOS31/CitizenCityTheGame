using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsultantOption", menuName = "Consultant/New ConsultantOption")]
public class ConsultantOption : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public List<DataCardType> Specializations;
    public int BasePrice;
    public int PricePerData;
    public int Duration;
}
