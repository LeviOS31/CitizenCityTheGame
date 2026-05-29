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
        if (player.money < BasePrice) return;

        player.money -= BasePrice;

        HiredConsultant hiredConsultant = new HiredConsultant(this, selection, player);
        OnHire.Invoke(hiredConsultant);
        GameController.UpdateUI.Invoke();
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
            TurnHistory.AddTurnAction?.Invoke($"{requestingPlayer.name}'s consultant {Name} has completed their work.");
            return selection;
        }
        else
        {
            return null;
        }
    }
}
