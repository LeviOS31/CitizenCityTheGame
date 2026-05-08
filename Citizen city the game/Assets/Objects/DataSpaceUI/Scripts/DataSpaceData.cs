using System.Collections.Generic;
using UnityEngine;

public class DataSpaceData : MonoBehaviour
{
    public int id;
    public DataSpaceType type;
    public List<DataSpaceDataRequired> NeededData = new List<DataSpaceDataRequired>();
    public int cost;
    public bool isEnabled;

    public DataSpaceData (int id, DataSpaceType type, List<DataSpaceDataRequired> NeededData, int cost, bool isEnabled)
    {
        this.id = id;
        this.type = type;
        this.NeededData = NeededData;
        this.cost = cost;
        this.isEnabled = isEnabled;
    }
}

public enum DataSpaceType
{
    municipal,
    regional
}

public class DataSpaceDataRequired
{
    public DataCardType cardType;
    public Color color;
    public bool isMet;

    public DataSpaceDataRequired (DataCardType cardType, Color color, bool isMet)
    {
        this.cardType = cardType;
        this.color = color;
        this.isMet = isMet;
    }
}