using System;
using System.Collections.Generic;
using UnityEngine;

public class DataCard
{
    public Guid ID;
    public DataCardType CardType;
    public Color Color;
    public bool IsTradable = true;
    public bool IsCorrectType = true;

    public DataCard(DataCardType cardType, Color color, bool tradable)
    {
        ID = Guid.NewGuid();
        CardType = cardType;
        Color = color;
        IsTradable = tradable;
    }

    public DataCard(DataCardType cardType, Color color)
    {
        ID = Guid.NewGuid();
        CardType = cardType;
        Color = color;
    }
}