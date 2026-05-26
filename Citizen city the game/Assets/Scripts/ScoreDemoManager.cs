using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class ScoreDemoManager : MonoBehaviour
{
    [Header("Simulation Stress Tester")]
    [Tooltip("Automatically increase random scores for everyone test")]
    public bool simulateGameplay = true;

    [Header("Data Monitoring")]
    public List<SpelerData> spelers = new List<SpelerData>();

    [Header("1. Detailed Overlay Setup (Tab)")]
    [Tooltip("The chunky prefab row containing the ModularRadarChart")]
    public GameObject detailedRowPrefab;
    [Tooltip("The parent container inside your Tab/Pause panel (Vertical Layout Group)")]
    public Transform detailedRowsContainer;

    [Header("2. Gameplay HUD Setup (Always Visible)")]
    [Tooltip("The sleek, thin prefab row for the HUD side-list")]
    public GameObject hudRowPrefab;
    [Tooltip("The parent container on the left of your screen (Vertical Layout Group)")]
    public Transform hudRowsContainer;

    private void Start()
    {
        // 1. Seed singleplayer match setup if empty
        if (spelers.Count == 0)
        {
            spelers.Add(new SpelerData { spelerNaam = "Player (You)", isBot = false, assignedColorIndex = 0 });
            spelers.Add(new SpelerData { spelerNaam = "Bot Alpha", isBot = true, assignedColorIndex = 1 });
            spelers.Add(new SpelerData { spelerNaam = "Bot Bravo", isBot = true, assignedColorIndex = 2 });
            spelers.Add(new SpelerData { spelerNaam = "Bot Charlie", isBot = true, assignedColorIndex = 3 });
        }

        // 2. Clear out editor placeholder layout elements
        ClearContainer(detailedRowsContainer);
        ClearContainer(hudRowsContainer);

        // 3. Dynamically spawn both UI variants for every player
        for (int i = 0; i < spelers.Count; i++)
        {
            if (detailedRowPrefab != null && detailedRowsContainer != null)
            {
                GameObject spawnedDetailed = Instantiate(detailedRowPrefab, detailedRowsContainer);
                Transform rowT = spawnedDetailed.transform;

                spelers[i].detailedRowTransform = rowT;
                spelers[i].mijnRadarChart = spawnedDetailed.GetComponentInChildren<ModularRadarChart>();

                // NEW: Find the left container column first
                Transform leftColumn = rowT.Find("Left_Content_Column");
                if (leftColumn != null)
                {
                    // Find Rank_And_Name inside the column
                    Transform rankNameGroup = leftColumn.Find("Rank_And_Name");
                    if (rankNameGroup != null)
                    {
                        Transform tRank = rankNameGroup.Find("Text_Rank");
                        Transform tName = rankNameGroup.Find("Text_Name");
                        if (tRank != null) spelers[i].textDetailedRank = tRank.GetComponent<TextMeshProUGUI>();
                        if (tName != null) spelers[i].textDetailedName = tName.GetComponent<TextMeshProUGUI>();
                    }

                    // Find Stats_Display inside the column
                    Transform statsGroup = leftColumn.Find("Stats_Display");
                    if (statsGroup != null)
                    {
                        System.Func<string, TextMeshProUGUI> GetValueText = (holderName) => {
                            Transform holder = statsGroup.Find(holderName);
                            if (holder != null)
                            {
                                Transform val = holder.Find("Value");
                                if (val != null) return val.GetComponent<TextMeshProUGUI>();
                            }
                            return null;
                        };

                        spelers[i].textDetailedAUT = GetValueText("Holder_AUT");
                        spelers[i].textDetailedSTR = GetValueText("Holder_STR");
                        spelers[i].textDetailedBOM = GetValueText("Holder_BOM");
                        spelers[i].textDetailedPOP = GetValueText("Holder_POP");
                        spelers[i].textDetailedHUI = GetValueText("Holder_HUI");
                    }
                }
            }

            // Spawn Compact Gameplay HUD Bar
            if (hudRowPrefab != null && hudRowsContainer != null)
            {
                GameObject spawnedHUD = Instantiate(hudRowPrefab, hudRowsContainer);
                spelers[i].hudRowTransform = spawnedHUD.transform;

                // Find the Rank_And_Name group first
                Transform rankNameGroup = spawnedHUD.transform.Find("Rank_And_Name");
                if (rankNameGroup != null)
                {
                    Transform tRank = rankNameGroup.Find("Text_Rank");
                    Transform tName = rankNameGroup.Find("Text_Name");

                    if (tRank != null) spelers[i].textHudRank = tRank.GetComponent<TextMeshProUGUI>();
                    if (tName != null) spelers[i].textHudName = tName.GetComponent<TextMeshProUGUI>();
                }

                // Find the line by name
                foreach (Transform child in spawnedHUD.transform)
                {
                    if (child.name == "HUD_Color_Line")
                    {
                        spelers[i].hudColorLineImage = child.GetComponent<UnityEngine.UI.Image>();
                        break;
                    }
                }
            }
        }

        if (simulateGameplay)
        {
            StartCoroutine(SimulateRandomScoreIncreases());
        }
    }

    private void Update()
    {
        // Execute baseline game math updates
        foreach (var speler in spelers)
        {
            speler.BerekenScore();
        }

        // Lock in the global score ceiling maximum limit for infinite dynamic chart scaling
        float hoogsteGevondenScore = 100f;
        foreach (var speler in spelers)
        {
            if (speler.scoreAuto > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreAuto;
            if (speler.scoreStroom > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreStroom;
            if (speler.scoreBoom > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreBoom;
            if (speler.scorePoppetje > hoogsteGevondenScore) hoogsteGevondenScore = speler.scorePoppetje;
            if (speler.scoreHuis > hoogsteGevondenScore) hoogsteGevondenScore = speler.scoreHuis;
        }

        // Sort descending based on real-time calculated general standings
        var gesorteerdeSpelers = spelers.OrderByDescending(s => s.algemeneScore).ToList();

        UpdateVisueleRanglijsten(gesorteerdeSpelers, hoogsteGevondenScore);
    }

    private void UpdateVisueleRanglijsten(List<SpelerData> gesorteerdeLijst, float globaleMax)
    {
        for (int i = 0; i < gesorteerdeLijst.Count; i++)
        {
            var speler = gesorteerdeLijst[i];
            string identityTag = speler.isBot ? " <size=18>[BOT]</size>" : " <size=18>[YOU]</size>";

            // --- 1. UPDATE DETAILED OVERLAY (TAB / PAUSE) ---
            if (speler.textDetailedRank != null)
            {
                string playerColorHex = GetHexForColorIndex(speler.assignedColorIndex);
                speler.textDetailedRank.text = $"<color=#{playerColorHex}>#{i + 1}</color>";
            }

            if (speler.textDetailedName != null)
            {
                string playerColorHex = GetHexForColorIndex(speler.assignedColorIndex);
                speler.textDetailedName.text = $"<color=#{playerColorHex}>{speler.spelerNaam}{identityTag}\n<color=#00FF00>{speler.algemeneScore} ptn</color>";
            }

            // Push raw asset totals into their respective icon text sub-nodes
            if (speler.textDetailedAUT != null) speler.textDetailedAUT.text = speler.aantalAuto.ToString();
            if (speler.textDetailedSTR != null) speler.textDetailedSTR.text = speler.aantalStroom.ToString();
            if (speler.textDetailedBOM != null) speler.textDetailedBOM.text = speler.aantalBoom.ToString();
            if (speler.textDetailedPOP != null) speler.textDetailedPOP.text = speler.aantalPoppetje.ToString();
            if (speler.textDetailedHUI != null) speler.textDetailedHUI.text = speler.aantalHuis.ToString();

            if (speler.detailedRowTransform != null)
            {
                speler.detailedRowTransform.SetSiblingIndex(i);
            }
            if (speler.mijnRadarChart != null)
            {
                List<float> scoresToDisplay = new List<float> { speler.scoreAuto, speler.scoreStroom, speler.scoreBoom, speler.scorePoppetje, speler.scoreHuis };
                speler.mijnRadarChart.UpdateChartData(speler.assignedColorIndex, scoresToDisplay, globaleMax);
            }

            // --- 2. UPDATE COMPACT GAMEPLAY HUD (LEFT SIDE LIST) ---
            if (speler.textHudRank != null)
            {
                string playerColorHex = GetHexForColorIndex(speler.assignedColorIndex);
                speler.textHudRank.text = $"<color=#{playerColorHex}><b>#{i + 1}</b></color>";
            }

            // Put the colored player name and green points into the name text box!
            if (speler.textHudName != null)
            {
                string playerColorHex = GetHexForColorIndex(speler.assignedColorIndex);
                speler.textHudName.text = $"<color=#{playerColorHex}>{speler.spelerNaam}</color> - <color=#059669>{speler.algemeneScore}pt</color>";
            }

            if (speler.hudColorLineImage != null)
            {
                speler.hudColorLineImage.color = GetColorForIndex(speler.assignedColorIndex);
            }
            if (speler.hudRowTransform != null)
            {
                speler.hudRowTransform.SetSiblingIndex(i);
            }

            if (detailedRowsContainer != null)
            {
                // Force calculations down the structural UI tree
                Canvas.ForceUpdateCanvases();

                // Re-initialize the grid layout positions instantly
                var gridLayout = detailedRowsContainer.GetComponent<UnityEngine.UI.GridLayoutGroup>();
                if (gridLayout != null)
                {
                    UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(detailedRowsContainer.GetComponent<RectTransform>());
                }
            }
        }
    }

    private string GetHexForColorIndex(int index)
    {
        switch (index)
        {
            case 0: return "007EFF"; // Blue
            case 1: return "FF0000"; // Red
            case 2: return "009B12"; // Green
            case 3: return "FFAD00"; // Yellow
            default: return "FFFFFF";
        }
    }

    private Color GetColorForIndex(int index)
    {
        switch (index)
        {
            case 0: return new Color(0.00f, 0.49f, 1.00f, 1f);  // Blue
            case 1: return new Color(1.00f, 0.00f, 0.00f, 1f);  // Red
            case 2: return new Color(0.00f, 0.61f, 0.07f, 1f);  // Green
            case 3: return new Color(1.00f, 0.68f, 0.00f, 1f);  // Yellow
            default: return Color.white;
        }
    }

    private void ClearContainer(Transform container)
    {
        if (container == null) return;
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    // --- Interactive Input Button / Event Modifier Triggers ---
    public void VerhoogAuto(int spelerIndex) { if (IndexValid(spelerIndex)) spelers[spelerIndex].aantalAuto++; }
    public void VerlaagAuto(int spelerIndex) { if (IndexValid(spelerIndex) && spelers[spelerIndex].aantalAuto > 0) spelers[spelerIndex].aantalAuto--; }

    public void VerhoogStroom(int spelerIndex) { if (IndexValid(spelerIndex)) spelers[spelerIndex].aantalStroom++; }
    public void VerlaagStroom(int spelerIndex) { if (IndexValid(spelerIndex) && spelers[spelerIndex].aantalStroom > 0) spelers[spelerIndex].aantalStroom--; }

    public void VerhoogBoom(int spelerIndex) { if (IndexValid(spelerIndex)) spelers[spelerIndex].aantalBoom++; }
    public void VerlaagBoom(int spelerIndex) { if (IndexValid(spelerIndex) && spelers[spelerIndex].aantalBoom > 0) spelers[spelerIndex].aantalBoom--; }

    public void VerhoogPoppetje(int spelerIndex) { if (IndexValid(spelerIndex)) spelers[spelerIndex].aantalPoppetje++; }
    public void VerlaagPoppetje(int spelerIndex) { if (IndexValid(spelerIndex) && spelers[spelerIndex].aantalPoppetje > 0) spelers[spelerIndex].aantalPoppetje--; }

    public void VerhoogHuis(int spelerIndex) { if (IndexValid(spelerIndex)) spelers[spelerIndex].aantalHuis++; }
    public void VerlaagHuis(int spelerIndex) { if (IndexValid(spelerIndex) && spelers[spelerIndex].aantalHuis > 0) spelers[spelerIndex].aantalHuis--; }

    private bool IndexValid(int index) => index >= 0 && index < spelers.Count;

    private IEnumerator SimulateRandomScoreIncreases()
    {
        yield return new WaitForSeconds(1.5f);

        while (simulateGameplay)
        {
            // 1. Pick a random player from the live list
            int randomPlayerIndex = Random.Range(0, spelers.Count);

            // 2. Pick a random category modifier (0 to 4)
            int randomCategory = Random.Range(0, 5);

            // 3. Fire the appropriate increment method
            switch (randomCategory)
            {
                case 0: VerhoogAuto(randomPlayerIndex); break;
                case 1: VerhoogStroom(randomPlayerIndex); break;
                case 2: VerhoogBoom(randomPlayerIndex); break;
                case 3: VerhoogPoppetje(randomPlayerIndex); break;
                case 4: VerhoogHuis(randomPlayerIndex); break;
            }

            // 4. Wait between 1 to 2.5 seconds before hitting the next random score upgrade
            float randomInterval = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(randomInterval);
        }
    }
}


[System.Serializable]
public class SpelerData
{
    public string spelerNaam;
    public bool isBot;
    [Range(0, 3)] public int assignedColorIndex; // 0=Red, 1=Blue, 2=Green, 3=Yellow

    [Header("Runtime Cache Layout Links")]
    [HideInInspector] public Transform detailedRowTransform;
    [HideInInspector] public ModularRadarChart mijnRadarChart;

    [HideInInspector] public TextMeshProUGUI textDetailedRank;
    [HideInInspector] public TextMeshProUGUI textDetailedName;

    // Decoupled tracking references for individual resource text strings
    [HideInInspector] public TextMeshProUGUI textDetailedAUT;
    [HideInInspector] public TextMeshProUGUI textDetailedSTR;
    [HideInInspector] public TextMeshProUGUI textDetailedBOM;
    [HideInInspector] public TextMeshProUGUI textDetailedPOP;
    [HideInInspector] public TextMeshProUGUI textDetailedHUI;

    [HideInInspector] public Transform hudRowTransform;
    [HideInInspector] public TextMeshProUGUI textHudRank;
    [HideInInspector] public TextMeshProUGUI textHudName;
    [HideInInspector] public UnityEngine.UI.Image hudColorLineImage;

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

    public SpelerData() 
    {
        GameController.Completedproject += AddScore;
    }

    public void BerekenScore()
    {
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

        // Variatie Multiplier Calculation Layers
        int actieveCategorieen = 0;
        if (aantalAuto > 0) actieveCategorieen++;
        if (aantalStroom > 0) actieveCategorieen++;
        if (aantalBoom > 0) actieveCategorieen++;
        if (aantalPoppetje > 0) actieveCategorieen++;
        if (aantalHuis > 0) actieveCategorieen++;

        float variatieBonus = Mathf.Max(0, (actieveCategorieen - 1) * 0.10f);

        // Balans Multiplier Calculation Layers
        float balansBonus = 0f;
        int[] alleScores = { scoreAuto, scoreStroom, scoreBoom, scorePoppetje, scoreHuis };
        var actieveScores = alleScores.Where(s => s > 0).ToArray();

        if (actieveScores.Length > 1)
        {
            float minActief = actieveScores.Min();
            float maxActief = actieveScores.Max();
            float balansVerhouding = minActief / maxActief;
            balansBonus = balansVerhouding * 0.30f;
        }

        float totaleMultiplier = 1.0f + variatieBonus + balansBonus;
        algemeneScore = Mathf.RoundToInt(basisScore * totaleMultiplier);
    }

    public void AddScore(Player player)
    {
        if (player == null) return;

        if (Vector4.Distance(player.color, hudColorLineImage.color) > 0.1f) return;

        List<ProjectData> playerProjects = player.PersonalProjects.Concat(player.ProvicialProjects).ToList();

        foreach(ProjectData project in playerProjects)
        {
            if (!project.IsDone || project.IsClaimed) continue;
            foreach (var req in project.NeededData)
            {
                if (req == null) continue;

                string typeName = req.CardType != null && req.CardType.dataType != null ? req.CardType.dataType.ToLowerInvariant() : string.Empty;

                int index = -1;
                switch (typeName)
                {
                    case "traffic":
                        index = 0; // Auto
                        break;
                    case "utility":
                        index = 1; // Stroom
                        break;
                    case "nature":
                        index = 2; // Boom
                        break;
                    case "civil":
                        index = 3; // Poppetje
                        break;
                    case "residential":
                        index = 4; // Huis
                        break;
                    default:
                        Debug.LogWarning($"Unrecognized CardType '{typeName}' in project '{project.name}'. No score added.");
                        break;
                }

                switch (index)
                {
                    case 0: aantalAuto ++; break;
                    case 1: aantalStroom ++; break;
                    case 2: aantalBoom ++; break;
                    case 3: aantalPoppetje ++; break;
                    case 4: aantalHuis ++; break;
                    default: break;
                }
            }
            project.IsClaimed = true;
        }

        // Recalculate derived scores after updating counts
        BerekenScore();
    }
}
