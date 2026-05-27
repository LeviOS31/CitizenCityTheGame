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
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerScore;
    [SerializeField] Image playerIcon;
    [SerializeField] Image backGround;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] GameObject cardPrefab;
    private Button interactableComponent;

    private GameController gameController;
    private Player player;
    private Dictionary<Guid, GameObject> cardEntities = new Dictionary<Guid, GameObject>();
    private int maxHandSize = 10;

    public void Initialize(Player player)
    {
        this.player = player;
        UpdateUI();
        this.player.UIChange += UpdateUI;
        interactableComponent = gameObject.GetComponentInChildren<Button>();
        interactableComponent.onClick.AddListener(() => FindAnyObjectByType<TradingWindowUI>().OpenTradingMenu(player));
        gameController = FindAnyObjectByType<GameController>();
        GameController.NewTurn += NewTurn;
        this.player.OnDrawCard += card => CreateCard(card, true);
        this.player.OnTradeCards += TradeCards;
    }

    private void NewTurn(Player activePlayer)
    {
        splineContainer.gameObject.SetActive(activePlayer != player);
    }

    public void UpdateUI()
    {
        playerName.text = player.name;
        playerScore.text = player.money.ToString();
        playerIcon.color = player.color;
        cardEntities.Values.ToList().ForEach(c => Destroy(c));
        cardEntities.Clear();

        foreach (DataCard card in player.cards)
        {
            CreateCard(card, true);
        }
    }

    private void CreateCard(DataCard playerCard, bool updatePostion)
    {
        GameObject cardInstance = Instantiate(cardPrefab);
        cardInstance.transform.SetParent(splineContainer.transform, true);
        cardInstance.GetComponent<DataCardUI>().Initialize(playerCard, false);
        //cardInstance.GetComponent<DataCardUI>().OnHover += RedrawCardPositions;
        cardEntities.Add(playerCard.ID, cardInstance);
        cardInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        if (updatePostion) UpdateCardPositions();
    }

    public void TradeCards(List<DataCard> cardsReceived, List<DataCard> cardsGiven)
    {
        foreach(DataCard card in cardsReceived)
        {
            CreateCard(card, false);
        }
        
        foreach(DataCard card in cardsGiven)
        {
            cardEntities[card.ID].transform.DOKill();
            Destroy(cardEntities[card.ID]);
            cardEntities.Remove(card.ID);
        }

        UpdateCardPositions();
    }

    public void ToggleInteractability(Player activePlayer)
    {
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
        if (cardEntities.Count == 0) return;
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
            cardEntities.ElementAt(i).Value.transform.DOLocalMove(splinePosition, 0.25f);
            cardEntities.ElementAt(i).Value.transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }

    private void RedrawCardPositions(GameObject hoveredCard, bool isHovered)
    {
        Debug.Log("redraw");
    }

    private void ClearHolder()
    {
        foreach (GameObject card in cardEntities.Values)
        {
            Destroy(card);
        }

        cardEntities.Clear();
    }
}
