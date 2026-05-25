using System;
using System.Collections.Generic;
using UnityEngine;
public class DataSpacesController : MonoBehaviour
{
    public GameController gameController;
    [SerializeField] List<DataCardType> dataCardTypes;
    private Player activeplayer;
    private List<DataSpaceData> dataSpaces = new List<DataSpaceData>();
    private List<DataSpaceDataRequired> dataForRegionalDataSpaces;
    private List<DataSpaceDataRequired> dataForMunicipalDataSpaces;
    public void initialize()
    {
        int dataSpaceId = 0;
        for (int i = 0; i < gameController.totalPlayers; i++)
        {
            dataForRegionalDataSpaces = new List<DataSpaceDataRequired>();
            dataForMunicipalDataSpaces = new List<DataSpaceDataRequired>();
            int cardTypeCount = 0;
            for (int j = 0; j < (dataCardTypes.Count * 2); j++)
            {
                if (cardTypeCount <= dataCardTypes.Count)
                {
                    cardTypeCount = 0;
                }

                if (j >= dataCardTypes.Count)
                {
                    DataSpaceDataRequired data = new DataSpaceDataRequired(dataCardTypes[cardTypeCount], gameController.players[i++ % gameController.totalPlayers].color, false);
                    dataForRegionalDataSpaces.Add(data);
                    cardTypeCount++;
                }
                else
                {
                    DataSpaceDataRequired data = new DataSpaceDataRequired(dataCardTypes[cardTypeCount], gameController.players[i].color, false);
                    dataForRegionalDataSpaces.Add(data);
                    cardTypeCount++;
                }
            }
            DataSpaceData regionalDataSpace = new DataSpaceData(dataSpaceId, DataSpaceType.regional, dataForRegionalDataSpaces, 250, false);
            dataSpaces.Add(regionalDataSpace);
            dataSpaceId++;
            for (int k = 0; k < dataCardTypes.Count; k++)
            {
                if (cardTypeCount <= dataCardTypes.Count)
                {
                    cardTypeCount = 0;
                }
                DataSpaceDataRequired data = new DataSpaceDataRequired(dataCardTypes[cardTypeCount], gameController.players[i].color, false);
                dataForMunicipalDataSpaces.Add(data);
                cardTypeCount++;
            }
            DataSpaceData municipalDataSpace = new DataSpaceData(dataSpaceId, DataSpaceType.regional, dataForMunicipalDataSpaces, 250, false);
            dataSpaces.Add(municipalDataSpace);
            dataSpaceId++;
        }
    }

    public void SubmitData(int id)
    {
        foreach(DataSpaceData dataSpace in activeplayer.DataSpaces)
        {
            if(dataSpace.id == id)
            {
                foreach(DataSpaceDataRequired dataRequired in dataSpace.neededData)
                {
                    CheckPlayerCards(dataRequired);
                }
                return;
            }
        }
    }

    public void ReloadPlayer(Player player)
    {
        activeplayer = player;

        if (activeplayer.DataSpaces.Count == 0)
        {
            foreach(DataSpaceData dataSpace in dataSpaces)
            {
                bool theRightDataspace = false;
                foreach(DataSpaceDataRequired data in dataSpace.neededData)
                {
                    if (data.color == player.color)
                    {
                        theRightDataspace = true;
                    }
                }

                if (theRightDataspace)
                {
                    activeplayer.DataSpaces.Add(dataSpace);
                }
            }            
        }
    }

    public void CheckPlayerCards(DataSpaceDataRequired Data)
    {
        if (activeplayer != null)
        {
            List<DataCard> remove = new List<DataCard>();

            foreach (DataCard card in activeplayer.cards)
            {
                if (card.CardType == Data.cardType && card.Color == Data.color)
                {
                    Debug.Log("card found");
                    remove.Add(card);
                    Data.isMet = true;
                    break;
                }
            }

            foreach (DataCard card in remove)
            {
                activeplayer.cards.Remove(card);
            }
        }
    }

    public bool EnableDataSpace(int id)
    {
        if (activeplayer == null)
        {
            Debug.LogWarning("EnableDataSpace called but activeplayer is null.");
            return false;
        }

        DataSpaceData dataspace = null;
        foreach (DataSpaceData space in activeplayer.DataSpaces)
        {
            if (space.id == id)
            {
                dataspace = space;
                break;
            }
        }

        int check = 0;
        foreach (DataSpaceDataRequired data in dataspace.neededData)
        {
            if (data.isMet)
            {
                check++;
            }
        }

        // require player to have at least the cost (>=) rather than exact equality
        if (check == dataspace.neededData.Count && activeplayer.currency >= dataspace.cost)
        {
            dataspace.isEnabled = true;
            return true;
        }

        return false;
    }
}
    
