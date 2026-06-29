using UnityEngine;
using UnityEngine.UI;

public class AccessibleDataSpaceUIItem : MonoBehaviour
{
    [Header("People Data Card")]
    [SerializeField] private GameObject peopleDataCardObject;
    [SerializeField] private Image peopleDataCardBackground;

    [Header("Traffic Data Card")]
    [SerializeField] private GameObject trafficDataCardObject;
    [SerializeField] private Image trafficDataCardBackground;

    [Header("Utility Data Card")]
    [SerializeField] private GameObject utilityDataCardObject;
    [SerializeField] private Image utilityDataCardBackground;

    [Header("Residential Data Card")]
    [SerializeField] private GameObject residentDataCardObject;
    [SerializeField] private Image residentDataCardBackground;

    [Header("Ecological Data Card")]
    [SerializeField] private GameObject ecologyDataCardObject;
    [SerializeField] private Image ecologyDataCardBackground;

    private Color selectedColor;
    //Passes on information to SetupCard
    public void Setup(Color playerColor, bool hasPeopleData, bool hasTrafficData, bool hasUtilityData, bool hasResidentData, bool hasEcologicalData)
    {
        SetupCard(peopleDataCardObject, peopleDataCardBackground, playerColor, hasPeopleData);

        SetupCard(trafficDataCardObject, trafficDataCardBackground, playerColor, hasTrafficData);

        SetupCard(utilityDataCardObject, utilityDataCardBackground, playerColor, hasUtilityData);

        SetupCard(residentDataCardObject, residentDataCardBackground, playerColor, hasResidentData);

        SetupCard(ecologyDataCardObject, ecologyDataCardBackground, playerColor, hasEcologicalData);
    }
    //Setup the cards of the data space prefab, setup the color and if its available
    private void SetupCard(GameObject cardObject, Image cardBackground, Color playerColor, bool isAvailable)
    {
        if (cardObject != null)
        {
            cardObject.SetActive(isAvailable);
        }

        if (cardBackground != null)
        {
            cardBackground.color = playerColor;
        }
    }
    //Gets the color of the data card that the player color selected
    public void GetSelectedColor(Image image)
    {
        if (image.color == null)
        {
            Debug.Log("No Color was given");
            return;
        }

        selectedColor = image.color;
    }
    //Adds the card the player selected to the list of data cards tht the player cna get from the dataspace
    public void AddSelectedDataCardInList(DataCardType dataCardType)
    {
        if (dataCardType == null)
        {
            Debug.Log("No card type was given");
            return;
        }

        DataCard dataCard = new DataCard(dataCardType, selectedColor);
        DataSpacesController.addSelectedCards.Invoke(dataCard);
    }
}