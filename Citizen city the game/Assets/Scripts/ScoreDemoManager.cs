using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class ScoreDemoManager : MonoBehaviour
{
    public List<SpelerData> spelers = new List<SpelerData>();

    [Header("TextMeshPro's")]
    public List<TextMeshProUGUI> positieTeksten = new List<TextMeshProUGUI>();

    [Header("Radar Chart UI Elements")]
    public List<ModularRadarChart> alleRadarCharts = new List<ModularRadarChart>();

    private void Update()
    {
        // 1. Bereken continu de scores voor elke speler
        foreach (var speler in spelers)
        {
            speler.BerekenScore();
        }

        // 2. FIND THE GLOBAL MAX VALUE FOR INFINITE DYNAMIC GROWING
        // We look through all active players and find the highest single sub-score in the game.
        float hoogsteGevondenScore = 100f;
        foreach (var speler in spelers)
        {
            if (speler.scoreAuto > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreAuto;
            if (speler.scoreStroom > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreStroom;
            if (speler.scoreBoom > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreBoom;
            if (speler.scorePoppetje > hoogsteGevondenScore) hoogsteGevondenScore = speler.scorePoppetje;
            if (speler.scoreHuis > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreHuis;
        }

        // 3. Tijdelijke, gesorteerde kopie van de spelerslijst voor het scorebord
        var gesorteerdeSpelers = spelers.OrderByDescending(s => s.algemeneScore).ToList();

        // 4. Stuur deze gesorteerde lijst EN de globale max door naar de UI
        UpdateVisueleRanglijst(gesorteerdeSpelers, hoogsteGevondenScore);
    }

    private void UpdateVisueleRanglijst(List<SpelerData> gesorteerdeLijst, float globaleMax)
    {
        for (int i = 0; i < positieTeksten.Count; i++)
        {
            if (positieTeksten[i] != null && i < gesorteerdeLijst.Count)
            {
                var speler = gesorteerdeLijst[i];

                positieTeksten[i].text = $"<b>#{i + 1} {speler.spelerNaam}</b> - Score: <color=#00FF00>{speler.algemeneScore}</color> ptn\n" +
                                         $"<size=32>AUT: {speler.aantalAuto} | STR: {speler.aantalStroom} | BOM: {speler.aantalBoom} | POP: {speler.aantalPoppetje} | HUI: {speler.aantalHuis}</size>";
            }
        }

        // Update INDIVIDUAL radar charts using the shared global ceiling
        foreach (var speler in spelers)
        {
            if (speler.mijnRadarChart != null)
            {
                List<float> scoresToDisplay = new List<float>
                {
                    speler.scoreAuto,
                    speler.scoreStroom,
                    speler.scoreBoom,
                    speler.scorePoppetje,
                    speler.scoreHuis
                    // If you expand categories later, add the new score property here!
                };

                // Pass the custom score list AND the dynamically scaling maximum limit
                speler.mijnRadarChart.UpdateChartData(scoresToDisplay, globaleMax);
            }
        }
    }

    private void Start()
    {
        if (spelers.Count == 0)
        {
            // We create the players and automatically link the chart at the matching index
            spelers.Add(new SpelerData
            {
                spelerNaam = "Speler 1",
                mijnRadarChart = alleRadarCharts.Count > 0 ? alleRadarCharts[0] : null
            });

            spelers.Add(new SpelerData
            {
                spelerNaam = "Speler 2",
                mijnRadarChart = alleRadarCharts.Count > 1 ? alleRadarCharts[1] : null
            });

            spelers.Add(new SpelerData
            {
                spelerNaam = "Speler 3",
                mijnRadarChart = alleRadarCharts.Count > 2 ? alleRadarCharts[2] : null
            });

            spelers.Add(new SpelerData
            {
                spelerNaam = "Speler 4",
                mijnRadarChart = alleRadarCharts.Count > 3 ? alleRadarCharts[3] : null
            });
        }
    }

    // 0 = Speler 1, 1 = Speler 2, 2 = Speler 3, 3 = Speler 4.

    public void VerhoogAuto(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            spelers[spelerIndex].aantalAuto++;
        }
    }

    public void VerlaagAuto(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            if (spelers[spelerIndex].aantalAuto > 0)
                spelers[spelerIndex].aantalAuto--;
        }
    }

    public void VerhoogStroom(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            spelers[spelerIndex].aantalStroom++;
        }
    }

    public void VerlaagStroom(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            if (spelers[spelerIndex].aantalStroom > 0)
                spelers[spelerIndex].aantalStroom--;
        }
    }

    public void VerhoogBoom(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            spelers[spelerIndex].aantalBoom++;
        }
    }

    public void VerlaagBoom(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            if (spelers[spelerIndex].aantalBoom > 0)
                spelers[spelerIndex].aantalBoom--;
        }
    }

    public void VerhoogPoppetje(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            spelers[spelerIndex].aantalPoppetje++;
        }
    }

    public void VerlaagPoppetje(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            if (spelers[spelerIndex].aantalPoppetje > 0)
                spelers[spelerIndex].aantalPoppetje--;
        }
    }

    public void VerhoogHuis(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            spelers[spelerIndex].aantalHuis++;
        }
    }

    public void VerlaagHuis(int spelerIndex)
    {
        if (spelerIndex >= 0 && spelerIndex < spelers.Count)
        {
            if (spelers[spelerIndex].aantalHuis > 0)
                spelers[spelerIndex].aantalHuis--;
        }
    }
}

[System.Serializable]
public class SpelerData
{
    public string spelerNaam;

    [Header("UI Reference")]
    public ModularRadarChart mijnRadarChart;

    [Header("Aantal Symbolen")]
    public int aantalAuto;
    public int aantalStroom;
    public int aantalBoom;
    public int aantalPoppetje;
    public int aantalHuis;

    [Header("Resultaten")]
    public int scoreAuto;
    public int scoreStroom;
    public int scoreBoom;
    public int scorePoppetje;
    public int scoreHuis;
    public int algemeneScore;

    public void BerekenScore()
    {
        // scorewaarde van 10 punten per item
        scoreAuto = aantalAuto * 10;
        scoreStroom = aantalStroom * 10;
        scoreBoom = aantalBoom * 10;
        scorePoppetje = aantalPoppetje * 10;
        scoreHuis = aantalHuis * 10;

        int basisScore = scoreAuto + scoreStroom + scoreBoom + scorePoppetje + scoreHuis;

        if (basisScore == 0)
        {
            algemeneScore = 0;
            return;
        }

        // 1. VARIATIE-BONUS: Hoeveel unieke categorieën heeft deze speler geactiveerd (> 0)?
        int actieveCategorieen = 0;
        if (aantalAuto > 0) actieveCategorieen++;
        if (aantalStroom > 0) actieveCategorieen++;
        if (aantalBoom > 0) actieveCategorieen++;
        if (aantalPoppetje > 0) actieveCategorieen++;
        if (aantalHuis > 0) actieveCategorieen++;

        // Bonus: +10% per extra categorie bovenop de eerste (maximaal +40% bonus bij alle 5 typen actief)
        float variatieBonus = Mathf.Max(0, (actieveCategorieen - 1) * 0.10f);

        // 2. ACTIEVE BALANS-BONUS: Hoe goed zijn de categorieën verdeeld die de speler wel heeft gebouwd?
        float balansBonus = 0f;
        int[] alleScores = { scoreAuto, scoreStroom, scoreBoom, scorePoppetje, scoreHuis };

        // Filter alle 0-scores eruit om de balans van de actieve keuzes te bepalen
        var actieveScores = alleScores.Where(s => s > 0).ToArray();

        if (actieveScores.Length > 1)
        {
            float minActief = actieveScores.Min();
            float maxActief = actieveScores.Max();
            float balansVerhouding = minActief / maxActief;

            // Maximaal +30% bonus voor een perfecte verdeling tussen de gebouwde categorieën
            balansBonus = balansVerhouding * 0.30f;
        }

        // Totale multiplier = Basis (100%) + Variatie Bonus + Actieve Balans Bonus
        float totaleMultiplier = 1.0f + variatieBonus + balansBonus;

        // Eindscore berekenen en afronden naar een heel getal
        algemeneScore = Mathf.RoundToInt(basisScore * totaleMultiplier);
    }
}