using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using static UnityEngine.Analytics.IAnalytic;
public class DataSpacesController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private DataSpaceGameObjectManager manager;
    [SerializeField] private List<DataCardType> dataCardTypes;
    private Player activeplayer;
    public List<DataSpaceData> dataSpaces = new List<DataSpaceData>();
    private List<DataCard> selectedDataCards = new List<DataCard>();
    public static Action<DataCard> addSelectedCards;
    private int requiredDataSpaceSelectionCount = 0;
    private bool hasCollectedDataSpaceCardsThisTurn = false;
    void Start()
    {
        CreateAllDataSpaces();
        addSelectedCards += FillSelectedDataSpaceCardsList;
    }

    private void CreateAllDataSpaces()
    {
        Debug.Log($"Player count: {gameController.players.Count}");

        int dataSpaceId = 0;
        for (int i = 0; i < gameController.players.Count; i++)
        {
            List<DataSpaceDataRequired> dataForRegionalDataSpaces = new List<DataSpaceDataRequired>();
            List<DataSpaceDataRequired> dataForMunicipalDataSpaces = new List<DataSpaceDataRequired>();

            int cardTypeCount = 0;

            // Regional data space:
            // Contains data from the current player and the next player.
            for (int j = 0; j < dataCardTypes.Count * 2; j++)
            {
                if (cardTypeCount >= dataCardTypes.Count)
                {
                    cardTypeCount = 0;
                }

                Player dataOwner;

                if (j >= dataCardTypes.Count)
                {
                    int nextPlayerIndex = GetNextPlayerIndex(i);
                    dataOwner = gameController.players[nextPlayerIndex];
                }
                else
                {
                    dataOwner = gameController.players[i];
                }

                DataSpaceDataRequired data = new DataSpaceDataRequired(
                    dataCardTypes[cardTypeCount],
                    dataOwner.color,
                    false
                );

                dataForRegionalDataSpaces.Add(data);
                cardTypeCount++;
            }

            DataSpaceData regionalDataSpace = new DataSpaceData(
                dataSpaceId,
                DataSpaceType.regional,
                dataForRegionalDataSpaces,
                250,
                false,
                false,
                false
            );

            dataSpaces.Add(regionalDataSpace);
            dataSpaceId++;

            // Municipal data space:
            // Contains only data from the current player.
            cardTypeCount = 0;

            for (int k = 0; k < dataCardTypes.Count; k++)
            {
                DataSpaceDataRequired data = new DataSpaceDataRequired(
                    dataCardTypes[cardTypeCount],
                    gameController.players[i].color,
                    false
                );

                dataForMunicipalDataSpaces.Add(data);
                cardTypeCount++;
            }

            DataSpaceData municipalDataSpace = new DataSpaceData(
                dataSpaceId,
                DataSpaceType.municipal,
                dataForMunicipalDataSpaces,
                250,
                false,
                false,
                false
            );

            dataSpaces.Add(municipalDataSpace);
            dataSpaceId++;
        }
    }

    private int GetNextPlayerIndex(int currentPlayerIndex)
    {
        int nextPlayerIndex = currentPlayerIndex + 1;

        if (nextPlayerIndex >= gameController.totalPlayers)
        {
            nextPlayerIndex = 0;
        }

        return nextPlayerIndex;
    }

    public void ReloadPlayer(Player player)
    {
        activeplayer = player;

        if (activeplayer == null)
        {
            Debug.LogWarning("ReloadPlayer was called with a null player.");
            return;
        }

        if (activeplayer.DataSpaces.Count > 0)
        {
            return;
        }

        foreach (DataSpaceData dataSpace in dataSpaces)
        {
            bool theRightDataspace = false;

            foreach (DataSpaceDataRequired data in dataSpace.neededData)
            {
                if (data.color == player.color)
                {
                    theRightDataspace = true;
                    break;
                }
            }

            if (theRightDataspace)
            {
                activeplayer.DataSpaces.Add(dataSpace);
            }
        }
        manager.ClearDataSpaceWindow();
    }

    public void SubmitData(DataSpaceData dataSpace, DataSpaceDataRequired requiredData)
    {
        if (activeplayer == null)
        {
            Debug.LogWarning("Cannot submit data because activeplayer is null.");
            return;
        }

        if (dataSpace == null)
        {
            Debug.LogWarning("Cannot submit data because dataSpace is null.");
            return;
        }

        foreach (DataSpaceDataRequired dataRequired in dataSpace.neededData)
        {
            if (!dataRequired.isMet && dataRequired == requiredData)
            {
                if (dataSpace.type == DataSpaceType.regional)
                {
                    if(dataRequired.color != activeplayer.color)
                    {
                        dataRequired.isMet = CheckPlayerCards(dataRequired).isMet;
                        Debug.Log("Data was sumbited");
                    }
                    else
                    {
                        Debug.Log("You cant submit this type of data");                        
                    }                 
                }
                else
                {
                    dataRequired.isMet = CheckPlayerCards(dataRequired).isMet;
                    Debug.Log("Data was sumbited");
                }                
            }
        }
    }

    public DataSpaceDataRequired CheckPlayerCards(DataSpaceDataRequired dataRequired)
    {
        if (activeplayer == null)
        {
            return dataRequired;
        }

        DataCard cardToRemove = null;

        foreach (DataCard card in activeplayer.cards)
        {
            if (card.CardType == dataRequired.cardType && card.Color == dataRequired.color)
            {
                cardToRemove = card;
                dataRequired.isMet = true;
                break;
            }
        }

        if (cardToRemove != null)
        {
            activeplayer.cards.Remove(cardToRemove);
            TurnHistory.AddTurnAction?.Invoke($"{activeplayer.name} submitted a <color=#{ColorUtility.ToHtmlStringRGB(dataRequired.color)}> {dataRequired.cardType} data card </color> to a data space.");
            FeedbackManager.Instance.ShowFeedback($"De {cardToRemove.CardType.dataType} kaart is succesvol ingeleverd voor de dataspace!", FeedbackType.Success);


            Player.FireUIChangePlayer(activeplayer);
            activeplayer.FireUIChange();
        }
        else
        {
            FeedbackManager.Instance.ShowFeedback($"Je hebt geen kaart van het type {dataRequired.cardType.dataType}", FeedbackType.Error);
        }

        return dataRequired;
    }

    public void InvestInRegionalDataSpace(DataSpaceData dataSpace)
    {
        if (activeplayer == null)
        {
            Debug.LogWarning("Cannot enable data space because activeplayer is null.");
            return;
        }

        if (dataSpace == null)
        {
            Debug.LogWarning("Cannot enable data space because dataSpace is null.");
            return;
        }

        if (dataSpace.player1HasInvested && dataSpace.player2HasInvested)
        {
            Debug.LogWarning("Players already invested in this DataSpace");
            return;
        }

        int completedRequirements = 0;
        int dataCardToInvest = 0;

        foreach (DataSpaceDataRequired data in dataSpace.neededData)
        {
            if (data.isMet && activeplayer.color != data.color)
            {
                completedRequirements++;
            }

            if (activeplayer.color != data.color)
            {
                dataCardToInvest++;
            }
        }

        int dataSpaceCostToInvest = dataSpace.cost / 2;
        bool allDataSubmitted = completedRequirements == dataCardToInvest;
        bool playerCanPay = activeplayer.money >= dataSpaceCostToInvest;
        bool justchanged = false;

        if (allDataSubmitted && playerCanPay)
        {
            activeplayer.money -= dataSpaceCostToInvest;
            if (!dataSpace.player1HasInvested)
            {
                dataSpace.player1HasInvested = true;
                justchanged = true;
                EnableStampForContracts.enableSignedStamp?.Invoke(dataSpace);
                Debug.Log("You invested in the data space");
                FeedbackManager.Instance.ShowFeedback($"Je hebt geïnvesteerd in de regionale dataspace", FeedbackType.Success);
            }

            if (dataSpace.player1HasInvested && !dataSpace.player2HasInvested && !justchanged)
            {
                dataSpace.player2HasInvested = true;
                EnableStampForContracts.enableSignedStamp?.Invoke(dataSpace);
                Debug.Log("You invested in the data space");
                FeedbackManager.Instance.ShowFeedback($"Je hebt geïnvesteerd in de regionale dataspace", FeedbackType.Success);
            }
        }
        else
        {
            if (!allDataSubmitted)
            {
                Debug.Log("There are still data to submit");
                FeedbackManager.Instance.ShowFeedback($"Je hebt niet alle benodigde data ingeleverd", FeedbackType.Error);
            }

            if (!playerCanPay)
            {
                Debug.Log("You don't have enough money to fully invest");
                FeedbackManager.Instance.ShowFeedback($"Je hebt niet genoeg geld om in deze dataspace te beleggen", FeedbackType.Error);
            }

            return;
        }

        if (dataSpace.player1HasInvested && dataSpace.player2HasInvested)
        {
            dataSpace.isEnabled = true;
            Debug.Log("Data space is enabled");
            EnableStampForContracts.enableStamp?.Invoke(dataSpace);
            UpdateDataSpaceControllerList(dataSpace.id);

            FeedbackManager.Instance.ShowFeedback($"Dataspace is succesvol geactiveerd", FeedbackType.Success);
        }

    }

    public bool EnableDataSpace(DataSpaceData dataSpace)
    {
        if (activeplayer == null)
        {
            Debug.LogWarning("Cannot enable data space because activeplayer is null.");
            return false;
        }

        if (dataSpace == null)
        {
            Debug.LogWarning("Cannot enable data space because dataSpace is null.");
            return false;
        }

        int completedRequirements = 0;

        foreach (DataSpaceDataRequired data in dataSpace.neededData)
        {
            if (data.isMet)
            {
                completedRequirements++;
            }
        }
        bool allDataSubmitted = completedRequirements == dataSpace.neededData.Count;
        bool playerCanPay = activeplayer.money >= dataSpace.cost;

        if (allDataSubmitted && playerCanPay)
        {
            activeplayer.money -= dataSpace.cost;
            dataSpace.isEnabled = true;
            EnableStampForContracts.enableStamp?.Invoke(dataSpace);
            UpdateDataSpaceControllerList(dataSpace.id);

            FeedbackManager.Instance.ShowFeedback($"Dataspace is succesvol geactiveerd", FeedbackType.Success);
        }
        else if (!playerCanPay)
        {
            FeedbackManager.Instance.ShowFeedback($"Je hebt niet genoeg geld om in deze dataspace te beleggen", FeedbackType.Error);
        }
        else if (!allDataSubmitted)
        {
            FeedbackManager.Instance.ShowFeedback($"Je hebt niet alle benodigde data ingeleverd", FeedbackType.Error);
        }

            return dataSpace.isEnabled;
    }

    private void UpdateDataSpaceControllerList(int id)
    {
        foreach (DataSpaceData dataSpaceData in dataSpaces)
        {
            if (dataSpaceData.id == id)
            {
                dataSpaceData.isEnabled = true;
            }
        }
    }

    private void FillSelectedDataSpaceCardsList(DataCard card)
    {
        if (hasCollectedDataSpaceCardsThisTurn)
        {
            Debug.Log("This player has already collected data space cards this turn.");
            return;
        }

        DataCard existingCardFromSameRegion = null;

        foreach (DataCard selectedCard in selectedDataCards)
        {
            if (selectedCard.Color == card.Color)
            {
                existingCardFromSameRegion = selectedCard;
                break;
            }
        }

        if (existingCardFromSameRegion != null)
        {
            selectedDataCards.Remove(existingCardFromSameRegion);
        }

        selectedDataCards.Add(card);

        Debug.Log($"Selected {card.CardType} data from region color {card.Color}");
    }

    public void GetSelectedDataSpaceCards()
    {
        if (hasCollectedDataSpaceCardsThisTurn)
        {
            Debug.Log("You have already collected from the data spaces this turn.");
            FeedbackManager.Instance.ShowFeedback($"Je hebt de dataspace al gebruikt deze beurt", FeedbackType.Error);
            return;
        }

        if (selectedDataCards.Count <= 0)
        {
            Debug.Log("No cards selected.");
            FeedbackManager.Instance.ShowFeedback($"Je hebt geen kaarten geselecteerd om te verzamelen", FeedbackType.Error);
            return;
        }

        if (selectedDataCards.Count < requiredDataSpaceSelectionCount)
        {
            //TODO: Change this shit

            FeedbackManager.Instance.ShowFeedback($"Je hebt niet data geselecteerd van elke beschikbare regio", FeedbackType.Error);

            Debug.Log($"You must select one data card from each available region before collecting. Selected {selectedDataCards.Count}/{requiredDataSpaceSelectionCount}.");
            return;
        }

        foreach (DataCard dataCard in selectedDataCards)
        {
            activeplayer.DrawDataSpaceCard(dataCard);
        }

        selectedDataCards.Clear();
        hasCollectedDataSpaceCardsThisTurn = true;

        Debug.Log("Data space cards collected.");
        FeedbackManager.Instance.ShowFeedback($"De geselecteerde kaarten zijn verzameld", FeedbackType.Success);
        TurnHistory.AddTurnAction?.Invoke($"{activeplayer.name} collected {selectedDataCards.Count} data card(s) from the data spaces.");
    }

    public void SetRequiredDataSpaceSelectionCount(int count)
    {
        requiredDataSpaceSelectionCount = count;
        selectedDataCards.Clear();
    }

    public void ResetDataSpaceCollectionForNewTurn()
    {
        hasCollectedDataSpaceCardsThisTurn = false;
        selectedDataCards.Clear();
        requiredDataSpaceSelectionCount = 0;
    }
}
