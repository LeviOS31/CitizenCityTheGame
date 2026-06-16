
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadarFiller : MonoBehaviour
{

    public List<ModularRadarChart> Radars;
    public List<TextMeshProUGUI> Name;
    public List<TextMeshProUGUI> Score;
    public List<Image> ColorStrip;

    public void EndGame(List<SpelerData> Players, List<Player> playerColors)
    {
        // Execute baseline game math updates
        foreach (var Player in Players)
        {
            Player.BerekenScore();
        }

        // Lock in the global score ceiling maximum limit for infinite dynamic chart scaling
        float hoogsteGevondenScore = 100f;
        foreach (var Player in Players)
        {
            if (Player.scoreAuto > hoogsteGevondenScore) hoogsteGevondenScore = Player.scoreAuto;
            if (Player.scoreStroom > hoogsteGevondenScore) hoogsteGevondenScore = Player.scoreStroom;
            if (Player.scoreBoom > hoogsteGevondenScore) hoogsteGevondenScore = Player.scoreBoom;
            if (Player.scorePoppetje > hoogsteGevondenScore) hoogsteGevondenScore = Player.scorePoppetje;
            if (Player.scoreHuis > hoogsteGevondenScore) hoogsteGevondenScore = Player.scoreHuis;
        }

        // Sort descending based on real-time calculated general standings
        var gesorteerdeSpelers = Players.OrderByDescending(s => s.algemeneScore).ToList();

        UpdateVisueleRanglijsten(gesorteerdeSpelers, hoogsteGevondenScore, playerColors);
    }

    private void UpdateVisueleRanglijsten(List<SpelerData> gesorteerdeLijst, float globaleMax, List<Player> players)
    {
        for (int i = 0; i < gesorteerdeLijst.Count; i++)
        {
            var Player = gesorteerdeLijst[i];

            if (Radars != null || Name != null || Score != null)
            {
                // 1. Pack the 5 category scores into a list
                List<float> scoresToDisplay = new List<float> { Player.scoreAuto, Player.scoreStroom, Player.scoreBoom, Player.scorePoppetje, Player.scoreHuis };

                // 2. Send the data directly to the radar chart component
                Radars[i].UpdateChartData(Player.assignedColorIndex, scoresToDisplay, globaleMax);

                Name[i].text = Player.spelerNaam;

                Score[i].text = Player.algemeneScore.ToString() + " Punten";

                ColorStrip[i].color = players.FirstOrDefault(p => p.name == Player.spelerNaam)?.color ?? Color.white;
            }
        }
    }
}
