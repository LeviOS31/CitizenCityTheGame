using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using System;

public class PlayerCardUI : MonoBehaviour
{
    [Header("Core UI Components")]
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerScore;
    [SerializeField] Image playerIcon;
    [SerializeField] Image backGround;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] GameObject cardPrefab;

    [Header("Leaderboard Extensions")]
    [SerializeField] TMP_Text playerRank;
    [SerializeField] TMP_Text playerPoints;

    private Button interactableComponent;

    private GameController gameController;
    private Player player;
    private SpelerData matchedScoreData;

    private List<GameObject> cardEntities = new List<GameObject>();
    private int maxHandSize = 10;

    public void Initialize(Player player)
    {
        this.player = player;

        LinkToScoreData();

        UpdateUI();
        RegenerateHandVisuals();

        this.player.UIChange += UpdateUI;
        this.player.UIChange += RegenerateHandVisuals;

        interactableComponent = gameObject.GetComponentInChildren<Button>();
        if (interactableComponent != null)
        {
            interactableComponent.onClick.AddListener(() => FindAnyObjectByType<TradingWindowUI>().OpenTradingMenu(player));
        }

        gameController = FindAnyObjectByType<GameController>();
        GameController.NewTurn += NewTurn;

        this.player.OnDrawCard += (card) => RegenerateHandVisuals();
        this.player.OnTradeCards += (received, given) => RegenerateHandVisuals();
    }

    public void LinkToScoreData()
    {
        ScoreManager scoreMgr = FindAnyObjectByType<ScoreManager>();
        if (scoreMgr != null && player != null)
        {
            matchedScoreData = scoreMgr.spelers.FirstOrDefault(s => s.spelerNaam == player.name);

            if (matchedScoreData == null)
            {
                int myPlayerIndex = FindAnyObjectByType<GameController>().players.IndexOf(player);
                if (myPlayerIndex >= 0 && myPlayerIndex < scoreMgr.spelers.Count)
                {
                    matchedScoreData = scoreMgr.spelers[myPlayerIndex];
                    matchedScoreData.spelerNaam = player.name;
                }
            }
        }
    }

    private void NewTurn(Player activePlayer)
    {
        if (splineContainer != null)
            splineContainer.gameObject.SetActive(activePlayer != player);
    }

    public void UpdateUI()
    {
        if (player == null) return;

        playerName.text = player.name;
        playerScore.text = $"€{player.money}";
        playerIcon.color = player.color;

        if (matchedScoreData != null && playerPoints != null)
        {
            playerPoints.text = $"<color=#059669>{matchedScoreData.algemeneScore} pt</color>";
        }
    }

    public void RegenerateHandVisuals()
    {
        ClearHolder();

        if (player.cards == null || player.cards.Count == 0)
        {
            // If the AI has no cards, the layout stays empty. Return early.
            return;
        }

        // Group cards sharing the same Color and Icon symbol
        var groupedCards = player.cards
            .GroupBy(card => new { card.Color, card.CardType.dataIcon })
            .Select(group => new
            {
                SampleCard = group.First(),
                Count = group.Count()
            });

        foreach (var group in groupedCards)
        {
            CreateGroupedCardVisual(group.SampleCard, group.Count);
        }

        UpdateCardPositions();
    }

    private void CreateGroupedCardVisual(DataCard playerCard, int count)
    {
        if (splineContainer == null || cardPrefab == null) return;

        GameObject cardInstance = Instantiate(cardPrefab);
        cardInstance.transform.SetParent(splineContainer.transform, false); // Using false keeps UI scales stable

        // Pass the calculated stack count to the DataCardUI component
        cardInstance.GetComponent<DataCardUI>().Initialize(playerCard, false, count);

        cardInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        cardEntities.Add(cardInstance);
    }

    private void ClearHolder()
    {
        foreach (GameObject card in cardEntities)
        {
            if (card != null)
            {
                card.transform.DOKill();
                Destroy(card);
            }
        }
        cardEntities.Clear();
    }

    public void ToggleInteractability(Player activePlayer)
    {
        if (interactableComponent == null || backGround == null) return;

        interactableComponent.enabled = true;

        if (activePlayer == player)
        {
            interactableComponent.enabled = false;
            backGround.color = Color.gray;
        }
        else
        {
            interactableComponent.enabled = true;
            backGround.color = Color.white;
        }
    }

    private void UpdateCardPositions()
    {
        if (cardEntities.Count == 0 || splineContainer == null) return;
        float cardSpacing = 0.15f;
        float firstCardPosition = 0f;
        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cardEntities.Count; i++)
        {
            float position = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(position);
            Vector3 forward = spline.EvaluateTangent(position);
            Vector3 up = spline.EvaluateUpVector(position);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);

            var cardVisual = cardEntities[i];
            if (cardVisual != null)
            {
                cardVisual.transform.DOLocalMove(splinePosition, 0.25f);
                cardVisual.transform.DOLocalRotateQuaternion(rotation, 0.25f);
            }
        }
    }

    public void SetVisualRank(int rankNumber)
    {
        if (playerRank != null)
        {
            playerRank.text = $"#{rankNumber}";
            playerRank.color = player.color;
        }
    }

    public Player GetPlayerModel() => player;
    public SpelerData GetScoreData() => matchedScoreData;
}
