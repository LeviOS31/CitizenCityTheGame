using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSpaceData
{
    public int id;
    public DataSpaceType type;
    public List<DataSpaceDataRequired> neededData = new List<DataSpaceDataRequired>();
    public int cost;
    public bool isEnabled;    
    public bool player1HasInvested; //used for the regional contracts
    public bool player2HasInvested; //used for the regional contracts
    public DataSpaceData (int id, DataSpaceType type, List<DataSpaceDataRequired> neededData, int cost, bool isEnabled, bool player1HasInvested, bool player2HasInvested)
    {
        this.id = id;
        this.type = type;
        this.neededData = neededData;
        this.cost = cost;
        this.isEnabled = isEnabled;
        this.player1HasInvested = player1HasInvested;
        this.player2HasInvested = player2HasInvested;
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