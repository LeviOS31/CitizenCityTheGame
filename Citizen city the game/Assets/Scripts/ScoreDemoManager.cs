using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class ScoreDemoManager : MonoBehaviour
{
    public List<SpelerData> spelers = new List<SpelerData>();

    [Header("UI Elementen (Sleep hier je TextMeshPro's in)")]
    // Een lijst waarin we onze 4 TextMeshPro-tekstvelden uit de scene gaan slepen
    public List<TextMeshProUGUI> positieTeksten = new List<TextMeshProUGUI>();

    private void Update()
    {
        // 1. Bereken continu de scores voor elke speler in de Inspector
        foreach (var speler in spelers)
        {
            speler.BerekenScore();
        }

        // 2. tijdelijke, gesorteerde kopie van de spelerslijst (van Hoog naar Laag) voor het scorebord
        var gesorteerdeSpelers = spelers.OrderByDescending(s => s.algemeneScore).ToList();

        // 3. Stuur deze gesorteerde lijst live door naar de tekstvelden op het scherm
        UpdateVisueleRanglijst(gesorteerdeSpelers);
    }

    private void UpdateVisueleRanglijst(List<SpelerData> gesorteerdeLijst)
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
    }

    private void Start()
    {
        if (spelers.Count == 0)
        {
            spelers.Add(new SpelerData { spelerNaam = "Speler 1" });
            spelers.Add(new SpelerData { spelerNaam = "Speler 2" });
            spelers.Add(new SpelerData { spelerNaam = "Speler 3" });
            spelers.Add(new SpelerData { spelerNaam = "Speler 4" });
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
        // We gebruiken de scorewaarde van 10 punten per item uit je voorbeeld
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