using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class CardHolderUI : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform cardSpawnPoint;

    private List<GameObject> cards = new List<GameObject>();
    private Player player;

    private void Start()
    {
        GameController.NewTurn += CreateCards;
        TradingWindowUI.OpenTradingWindow += ToggleRender;
    }

    private void CreateCards(Player player)
    {
        if (player != null) 
        {
            player.OnReceiveTradeCards -= TradeCards;
        }

        this.player = player;

        this.player.OnReceiveTradeCards += TradeCards;

        ClearHolder();
        foreach(DataCard card in player.cards)
        {
            GameObject cardInstance = Instantiate(cardPrefab);
            cardInstance.transform.SetParent(transform, false);
            cardInstance.GetComponent<DataCardUI>().Initialize(card, false);
            cardInstance.GetComponent<DataCardUI>().OnHover += RedrawCardPositions;
            cards.Add(cardInstance);
            UpdateCardPositions();
        }
    }

    private void UpdateCardPositions()
    {
        if (cards.Count == 0) return;
        float cardSpacing = 0.1f;
        float cardHoverSpacing = 0.2f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            float position = firstCardPosition + i * cardSpacing;
            Spline spline = splineContainer.Spline;
            Vector3 splinePosition = spline.EvaluatePosition(position);
            Vector3 forward = spline.EvaluateTangent(position);
            Vector3 up = spline.EvaluateUpVector(position);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            cards[i].transform.localPosition = splinePosition;
            cards[i].transform.localRotation = rotation;
        }
    }

    private void RedrawCardPositions(GameObject hoveredCard, bool isHovered)
    {
        if (!isHovered)
        {
            UpdateCardPositions();
            return;
        }

        if (cards.Count == 0) return;
        float cardSpacing = 0.1f;
        float cardHoverSpacing = 0.15f;
        float currentCardSpace = 0f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            float position = firstCardPosition + currentCardSpace;
            Spline spline = splineContainer.Spline;
            Vector3 splinePosition = spline.EvaluatePosition(position);
            Vector3 forward = spline.EvaluateTangent(position);
            Vector3 up = spline.EvaluateUpVector(position);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);

            if (cards[i] == hoveredCard) splinePosition += Vector3.up * 50;

            cards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
            cards[i].transform.DOLocalMove(splinePosition, 0.25f);
            currentCardSpace += cards[i] == hoveredCard ? cardHoverSpacing : cardSpacing;
        }
    }

    private void TradeCards(List<DataCard> cards)
    {
        CreateCards(player);
    }

    private void ClearHolder()
    {
        if (cards.Count == 0) return;

        foreach (GameObject card in cards)
        {
            Destroy(card);
        }

        cards.Clear();
    }

    private void ToggleRender(bool isTradingWindowActive)
    {
        gameObject.SetActive(!isTradingWindowActive);

        if (!isTradingWindowActive) CreateCards(player);
    }
}
