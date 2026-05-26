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

    private Color selectedColor;

    public void Setup(Color playerColor, bool hasPeopleData, bool hasTrafficData, bool hasUtilityData)
    {
        SetupCard(peopleDataCardObject, peopleDataCardBackground, playerColor, hasPeopleData);

        SetupCard(trafficDataCardObject, trafficDataCardBackground, playerColor, hasTrafficData);

        SetupCard(utilityDataCardObject, utilityDataCardBackground, playerColor, hasUtilityData);
    }

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

    public void GetSelectedColor(Image image)
    {
        if(image.color == null)
        {
            Debug.Log("No Color was given");
            return;
        }

        selectedColor = image.color;
    }

    public void AddSelectedDataCardInList(DataCardType dataCardType)
    {
        if(dataCardType == null)
        {
            Debug.Log("No card type was given");
            return;
        }

        DataCard dataCard = new DataCard(dataCardType, selectedColor, false);
        DataSpacesController.addSelectedCards.Invoke(dataCard);
    }
}