using System.Collections.Generic;
using UnityEngine;

public class DataSpacesController : MonoBehaviour
{
    public GameController gameController;
    [SerializeField] List<DataCardType> dataCardTypes;
    public List<DataSpaceData> dataSpaces = new List<DataSpaceData>();
    private List<DataSpaceDataRequired> dataForRegionalDataSpaces;
    private List<DataSpaceDataRequired> dataForMunicipalDataSpaces;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
                    DataSpaceDataRequired data = new DataSpaceDataRequired(dataCardTypes[cardTypeCount], gameController.players[i+1].color, false);
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
            for(int k =0; k<dataCardTypes.Count; k++)
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

    // Update is called once per frame
    void Update()
    {

    }
}
