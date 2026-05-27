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

    public void DisplayAccessibleDataSpaces()
    {
        ClearAccessibleDataUI();

        List<Color> reachableColors = GetReachablePlayerColors();

        dataSpacesController.SetRequiredDataSpaceSelectionCount(reachableColors.Count);

        foreach (Color reachableColor in reachableColors)
        {
            bool hasMunicipalDataSpaceEnabled = HasMunicipalDataSpaceEnabled(reachableColor);

            bool hasPeopleData = true;
            bool hasTrafficData = hasMunicipalDataSpaceEnabled;
            bool hasUtilityData = hasMunicipalDataSpaceEnabled;

            AccessibleDataSpaceUIItem uiItem = Instantiate(accessibleDataPrefab, accessibleDataParent);

            uiItem.Setup(
                reachableColor,
                hasPeopleData,
                hasTrafficData,
                hasUtilityData
            );
        }
    }

    public void FindAvailableDataForAI()
    {
        ClearAccessibleDataUI();

        List<Color> reachableColors = GetReachablePlayerColors();

        dataSpacesController.SetRequiredDataSpaceSelectionCount(reachableColors.Count);

        foreach (Color reachableColor in reachableColors)
        {
            bool hasMunicipalDataSpaceEnabled = HasMunicipalDataSpaceEnabled(reachableColor);

            bool hasPeopleData = true;
            bool hasTrafficData = hasMunicipalDataSpaceEnabled;
            bool hasUtilityData = hasMunicipalDataSpaceEnabled;

            AccessibleDataSpaceUIItem uiItem = Instantiate(accessibleDataPrefab, accessibleDataParent);

            uiItem.AiSetup(
                reachableColor,
                hasPeopleData,
                hasTrafficData,
                hasUtilityData
            );
        }
    }

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

    private List<Color> GetReachablePlayerColors()
    {
        List<DataSpaceData> controllersDataSpaces = dataSpacesController.dataSpaces;

        Color playerColor = gameController.activePlayer.color;

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
}