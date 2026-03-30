using System;
using System.Collections.Generic;
using UnityEngine;

public class DataCard : MonoBehaviour
{
    public Guid ID;
    public DataCardType CardType;
    public Color Color;
    public bool IsTradable = true;

    public DataCard(DataCardType cardType, Color color, bool tradable)
    {
        ID = new Guid();
        CardType = cardType;
        Color = color;
        IsTradable = tradable;
    }

    public DataCard(DataCardType cardType, Color color)
    {
        CardType = cardType;
        Color = color;
    }
}