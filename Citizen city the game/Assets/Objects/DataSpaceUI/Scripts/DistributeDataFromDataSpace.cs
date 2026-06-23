using System.Collections.Generic;
using UnityEngine;

public class DistributeDataFromDataSpace : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GameController gameController;
    [SerializeField] private DataSpacesController dataSpacesController;

    [Header("Accessible Data UI")]
    [SerializeField] private Transform accessibleDataParent;
    [SerializeField] private AccessibleDataSpaceUIItem accessibleDataPrefab;

    [Header("Data Card Types")]
    [SerializeField] private DataCardType peopleDataCardType;
    [SerializeField] private DataCardType trafficDataCardType;
    [SerializeField] private DataCardType utilityDataCardType;
    [SerializeField] private DataCardType residentDataCardType;
    [SerializeField] private DataCardType ecologyDataCardType;
    //Instantiates the data spaces that the player has access to
    public void DisplayAccessibleDataSpaces()
    {
        ClearAccessibleDataUI();

        List<Color> reachableColors = GetReachablePlayerColors();

        foreach (Color reachableColor in reachableColors)
        {
            bool hasMunicipalDataSpaceEnabled = HasMunicipalDataSpaceEnabled(reachableColor);

            bool hasPeopleData = true;
            bool hasTrafficData = hasMunicipalDataSpaceEnabled;
            bool hasUtilityData = hasMunicipalDataSpaceEnabled;
            bool hasResidentData = hasMunicipalDataSpaceEnabled;
            bool hasEcologicalData = hasMunicipalDataSpaceEnabled;

            AccessibleDataSpaceUIItem uiItem = Instantiate(accessibleDataPrefab, accessibleDataParent);

            uiItem.Setup(
                reachableColor,
                hasPeopleData,
                hasTrafficData,
                hasUtilityData,
                hasResidentData,
                hasEcologicalData
            );
        }
    }
    //The ai uses this script to check which data he can get from the data space
    public void AISelectAndCollectRandomDataSpaceCards()
    {
        Player activePlayer = GameController.activePlayer;

        if (activePlayer == null)
        {
            Debug.LogWarning("AI cannot collect data space cards because activePlayer is null.");
            return;
        }

        List<Color> reachableColors = GetReachablePlayerColors();

        if (reachableColors == null || reachableColors.Count <= 0)
        {
            Debug.Log("AI has no available data spaces to collect from.");
            return;
        }

        foreach (Color reachableColor in reachableColors)
        {
            DataCard randomCard = CreateRandomAvailableDataSpaceCard(reachableColor);

            if (randomCard == null)
            {
                Debug.LogWarning($"AI could not create a random data card for region color {reachableColor}.");
                continue;
            }

            DataSpacesController.addSelectedCards?.Invoke(randomCard);
        }

        dataSpacesController.GetSelectedDataSpaceCards();
    }
    //Clears the body where the data spaces get instantiated 
    private void ClearAccessibleDataUI()
    {
        if (accessibleDataParent == null)
        {
            Debug.LogWarning("Accessible Data Parent is not assigned.");
            return;
        }

        foreach (Transform child in accessibleDataParent)
        {
            Destroy(child.gameObject);
        }
    }
    //Gets the different colors of data cards the player has access to
    private List<Color> GetReachablePlayerColors()
    {
        List<DataSpaceData> controllersDataSpaces = dataSpacesController.dataSpaces;

        Color playerColor = GameController.activePlayer.color;

        HashSet<Color> reachableColors = new HashSet<Color>();
        Queue<Color> colorsToCheck = new Queue<Color>();

        reachableColors.Add(playerColor);
        colorsToCheck.Enqueue(playerColor);

        while (colorsToCheck.Count > 0)
        {
            Color currentColor = colorsToCheck.Dequeue();

            foreach (DataSpaceData dataSpace in controllersDataSpaces)
            {
                if (!dataSpace.isEnabled)
                    continue;

                if (dataSpace.type != DataSpaceType.regional)
                    continue;

                if (dataSpace.neededData == null)
                    continue;

                bool dataSpaceContainsCurrentColor = false;

                foreach (DataSpaceDataRequired required in dataSpace.neededData)
                {
                    if (required.color == currentColor)
                    {
                        dataSpaceContainsCurrentColor = true;
                        break;
                    }
                }

                if (!dataSpaceContainsCurrentColor)
                    continue;

                foreach (DataSpaceDataRequired required in dataSpace.neededData)
                {
                    Color connectedColor = required.color;

                    if (!reachableColors.Contains(connectedColor))
                    {
                        reachableColors.Add(connectedColor);
                        colorsToCheck.Enqueue(connectedColor);
                    }
                }
            }
        }

        return new List<Color>(reachableColors);
    }
    //Checks if the player has his municispality data space activated
    private bool HasMunicipalDataSpaceEnabled(Color playerColor)
    {
        foreach (DataSpaceData dataSpace in dataSpacesController.dataSpaces)
        {
            if (!dataSpace.isEnabled)
                continue;

            if (dataSpace.type != DataSpaceType.municipal)
                continue;

            if (dataSpace.neededData == null)
                continue;

            foreach (DataSpaceDataRequired requiredData in dataSpace.neededData)
            {
                if (requiredData.color == playerColor)
                {
                    return true;
                }
            }
        }

        return false;
    }
    //Choses which data space the ai will use
    private DataCard CreateRandomAvailableDataSpaceCard(Color regionColor)
    {
        List<DataCardType> availableCardTypes = new List<DataCardType>();

        //if (peopleDataCardType != null)
        //{
        //    availableCardTypes.Add(peopleDataCardType);
        //}

        bool hasMunicipalDataSpaceEnabled = HasMunicipalDataSpaceEnabled(regionColor);

        if (hasMunicipalDataSpaceEnabled)
        {
            if (trafficDataCardType != null)
            {
                availableCardTypes.Add(trafficDataCardType);
            }

            if (utilityDataCardType != null)
            {
                availableCardTypes.Add(utilityDataCardType);
            }

            if (residentDataCardType != null)
            {
                availableCardTypes.Add(residentDataCardType);
            }

            if (ecologyDataCardType != null)
            {
                availableCardTypes.Add(ecologyDataCardType);
            }
        }

        if (availableCardTypes.Count <= 0)
        {
            return null;
        }

        DataCardType randomCardType = availableCardTypes[Random.Range(0, availableCardTypes.Count)];

        return new DataCard(randomCardType, regionColor, false);
    }
}