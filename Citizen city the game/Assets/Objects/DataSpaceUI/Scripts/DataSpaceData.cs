using System.Collections.Generic;
using UnityEngine;

public class DataSpaceData
{
    public int id;
    public DataSpaceType type;
    public List<DataSpaceDataRequired> neededData = new List<DataSpaceDataRequired>();
    public int cost;
    public bool isEnabled = false;

    public DataSpaceData (int id, DataSpaceType type, List<DataSpaceDataRequired> neededData, int cost, bool isEnabled = false)
    {
        this.id = id;
        this.type = type;
        this.neededData = neededData;
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