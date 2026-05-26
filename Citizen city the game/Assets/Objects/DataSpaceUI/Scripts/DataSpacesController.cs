using System;
using System.Collections.Generic;
using UnityEngine;
public class DataSpacesController : MonoBehaviour
{
    [SerializeField] private GameController gameController;    
    [SerializeField] private DataSpaceGameObjectManager manager;
    [SerializeField] private List<DataCardType> dataCardTypes;
    private Player activeplayer;
    public List<DataSpaceData> dataSpaces = new List<DataSpaceData>();
    private bool check = false;

    void Start()
    {
        CreateAllDataSpaces();
        check = true;
    }

    private void CreateAllDataSpaces()
    {
        if (check)
        {
            Debug.Log("start happened again");
            return;
        }

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
                dataRequired.isMet = CheckPlayerCards(dataRequired).isMet;
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
        }

        return dataRequired;
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
        activeplayer.currency = dataSpace.cost;
        bool allDataSubmitted = completedRequirements == dataSpace.neededData.Count;
        bool playerCanPay = activeplayer.currency >= dataSpace.cost;

        if (allDataSubmitted && playerCanPay)
        {
            activeplayer.currency -= dataSpace.cost;
            dataSpace.isEnabled = true;
        }

        if(dataSpace.isEnabled)
        {
            UpdateDataSpaceControllerList(dataSpace.id);
        }

        return dataSpace.isEnabled;
    }

    private void UpdateDataSpaceControllerList(int id)
    {
        foreach(DataSpaceData dataSpaceData in dataSpaces)
        {
            if(dataSpaceData.id == id)
            {
                dataSpaceData.isEnabled = true;
            }
        }
    }
}
