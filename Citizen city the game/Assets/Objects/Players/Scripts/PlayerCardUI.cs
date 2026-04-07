using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class PlayerCardUI : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerScore;
    [SerializeField] Image playerIcon;
    [SerializeField] TMP_Text scorePerTurn;
    [SerializeField] TMP_Text scoreMulitplier;
    [SerializeField] Image backGround;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] GameObject cardPrefab;
    private Button interactableComponent;

    private GameController gameController;
    private Player player;
    private Dictionary<DataCard, GameObject> cardEntities = new Dictionary<DataCard, GameObject>();
    private int maxHandSize = 10;

    public void Initialize(Player player)
    {
        this.player = player;
        UpdateUI();
        interactableComponent = gameObject.GetComponentInChildren<Button>();
        interactableComponent.onClick.AddListener(() => FindAnyObjectByType<TradingWindowUI>().OpenTradingMenu(player));
        gameController = FindAnyObjectByType<GameController>();
        gameController.NewTurn += NewTurn;
        this.player.OnDrawCard += CreateCard;
        this.player.OnTradeCards += TradeCards;
    }

    private void NewTurn(Player activePlayer)
    {
        splineContainer.gameObject.SetActive(activePlayer != player);
    }

    public void UpdateUI()
    {
        playerName.text = player.name;
        playerScore.text = player.score.ToString();
        playerIcon.color = player.color;
        scorePerTurn.text = player.scorePerTurn.ToString();
        scoreMulitplier.text = player.scoreMultiplier.ToString();
    }

    private void CreateCard(DataCard playerCard)
    {
        GameObject cardInstance = Instantiate(cardPrefab);
        cardInstance.transform.SetParent(splineContainer.transform, true);
        cardInstance.GetComponent<DataCardUI>().Initialize(playerCard, false);
        //cardInstance.GetComponent<DataCardUI>().OnHover += RedrawCardPositions;
        cardEntities.Add(playerCard, cardInstance);
        cardInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        UpdateCardPositions();
    }

    public void TradeCards(List<DataCard> cardsReceived, List<DataCard> cardsGiven)
    {
        foreach(DataCard card in cardsReceived)
        {
            CreateCard(card);
        }
        
        foreach(DataCard card in cardsGiven)
        {
            Destroy(cardEntities[card]);
            cardEntities.Remove(card);
            UpdateCardPositions();
        }
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
