using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;

public class CardHolderUI : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform cardSpawnPoint;

    private List<GameObject> cards = new List<GameObject>();
    private Player player;

    /// <summary>
    /// Called when the game starts. Subscribes methods to game events 
    /// like turn switches, player changes, and trading window toggles.
    /// </summary>
    private void Start()
    {
        GameController.NewTurn += CreateCards;
        TradingWindowUI.OpenTradingWindow += ToggleRender;
        Player.UIChangeplayer += CreateCards;
    }

    /// <summary>
    /// Spawns and configures the UI cards for the active player. It groups identical 
    /// cards together so duplicates stack visually rather than crowding the screen.
    /// </summary>
    /// <param name="player">The player whose hand is currently being generated.</param>
    public void CreateCards(Player player) //TODO: Make private again
    {
        // Clean up previous event subscription to avoid memory leaks
        if (player != null)
        {
            player.OnReceiveTradeCards -= TradeCards;
        }

        // Only draw cards if this player is the active one taking their turn
        if (player != GameController.activePlayer)
        {
            return;
        }

        this.player = player;

        // Listen for when this player receives cards from a trade
        this.player.OnReceiveTradeCards += TradeCards;

        // Destroy old card UI objects
        ClearHolder();

        if (player.cards == null || player.cards.Count == 0) return;

        // Group cards by matching color and icon to stack duplicates together
        var groupedCards = player.cards
            .GroupBy(card => new { card.Color, card.CardType.dataIcon })
            .Select(group => new
            {
                FirstCardSample = group.First(), // Use the first instance to build visuals
                Count = group.Count()            // Total number of these duplicates
            });

        // Instantiate a single card UI element for each unique group
        foreach (var group in groupedCards)
        {
            GameObject cardInstance = Instantiate(cardPrefab);
            cardInstance.transform.SetParent(transform, false);

            // Pass the group count into our updated initializer
            cardInstance.GetComponent<DataCardUI>().Initialize(group.FirstCardSample, false, group.Count);
            cardInstance.GetComponent<DataCardUI>().OnHover += RedrawCardPositions;

            cards.Add(cardInstance);
        }

        // Position the new card layout instantly
        UpdateCardPositions();
    }

    /// <summary>
    /// Instantly calculates and distributes card positions and rotations along 
    /// a curved Unity Spline path, centering the hand layout horizontally.
    /// </summary>
    private void UpdateCardPositions()
    {
        if (cards.Count == 0) return;
        float cardSpacing = 0.1f;

        // Calculate the starting point on the spline so the hand remains perfectly centered
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            float position = firstCardPosition + i * cardSpacing;
            Spline spline = splineContainer.Spline;

            // Sample coordinates, tangents, and up-vectors from the spline curve
            Vector3 splinePosition = spline.EvaluatePosition(position);
            Vector3 forward = spline.EvaluateTangent(position);
            Vector3 up = spline.EvaluateUpVector(position);

            // Calculate a clean rotation matching the orientation of the spline curvature
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);

            cards[i].transform.localPosition = splinePosition;
            cards[i].transform.localRotation = rotation;
        }
    }

    /// <summary>
    /// Smoothly animates card positions and rotations using DOTween when a card is hovered over, 
    /// lifting the targeted card upwards and gently making room around it.
    /// </summary>
    /// <param name="hoveredCard">The specific card GameObject the cursor is interacting with.</param>
    /// <param name="isHovered">True if mouse is hovering over, false if mouse has exited.</param>
    private void RedrawCardPositions(GameObject hoveredCard, bool isHovered)
    {
        // If the cursor left the card, restore the standard centered layout layout instantly
        if (!isHovered)
        {
            UpdateCardPositions();
            return;
        }

        if (cards.Count == 0) return;
        float cardSpacing = 0.1f;
        float cardHoverSpacing = 0.15f; // Extra space allocated when a card is selected
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

            // Pop the hovered card slightly upward along the Y-axis to grab attention
            if (cards[i] == hoveredCard) splinePosition += Vector3.up * 50;

            // Smoothly ease the movement and rotation over 0.25 seconds using DOTween
            cards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
            cards[i].transform.DOLocalMove(splinePosition, 0.25f);

            // Adjust the layout spacing offset for subsequent cards based on current hover status
            currentCardSpace += cards[i] == hoveredCard ? cardHoverSpacing : cardSpacing;
        }
    }

    /// <summary>
    /// Callback triggered by a completed trade. It forces a reconstruction of the 
    /// player's card hand layout to show the updated inventory.
    /// </summary>
    private void TradeCards(List<DataCard> cards) => CreateCards(player);

    /// <summary>
    /// Iterates through the list of generated card GameObjects, completely destroys them, 
    /// and empties the tracking collection list.
    /// </summary>
    private void ClearHolder()
    {
        foreach (GameObject card in cards)
        {
            if (card != null) Destroy(card);
        }
        cards.Clear();
    }

    /// <summary>
    /// Toggles the visibility of the card hand container so it automatically hides 
    /// when the trading menu is open, and redraws it when closing.
    /// </summary>
    /// <param name="isTradingWindowActive">Is the fullscreen trading screen active?</param>
    private void ToggleRender(bool isTradingWindowActive)
    {
        gameObject.SetActive(!isTradingWindowActive);
        if (!isTradingWindowActive) CreateCards(player);
    }
}