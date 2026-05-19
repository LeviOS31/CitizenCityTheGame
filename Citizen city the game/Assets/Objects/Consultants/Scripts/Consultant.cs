using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Consultant
{
    public string Name;
    public Sprite Icon;
    public List<DataCardType> Specializations;
    public int BasePrice;
    public int PricePerData;
    public int Duration;
    public event Action<HiredConsultant> OnHire;

    public Consultant()
    {

    }
    public Consultant (string name, Sprite consultantIcon, List<DataCardType> specializations, int basePrice, int pricePerData, int duration)
    {
        Name = name;
        Icon = consultantIcon;
        Specializations = specializations;
        BasePrice = basePrice;
        PricePerData = pricePerData;
        Duration = duration;
    }

    public void HireConsultant(List<DataCard> selection, Player player)
    {
        HiredConsultant hiredConsultant = new HiredConsultant(this, selection, player);
        OnHire.Invoke(hiredConsultant);
    }
}

public class HiredConsultant : Consultant
{
    public Player requestingPlayer;
    public List<DataCard> selection;
    public int turnsLeft;

    public HiredConsultant(
        Consultant consultant,
        List<DataCard> selection,
        Player player
        )
    {
        Name = consultant.Name;
        Icon = consultant.Icon;
        Specializations = consultant.Specializations;
        BasePrice = consultant.BasePrice;
        PricePerData = consultant.PricePerData;
        Duration = consultant.Duration;
        turnsLeft = consultant.Duration;
        this.selection = selection;
        requestingPlayer = player;
    }

#nullable enable
    public List<DataCard>? UpdateConsultantProgress()
    {
        turnsLeft--;
        if (turnsLeft <= 0)
        {
            return selection;
        }
        else
        {
            return null;
        }
    }
}

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
