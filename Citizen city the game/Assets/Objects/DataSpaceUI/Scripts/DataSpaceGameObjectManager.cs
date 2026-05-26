using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataSpaceGameObjectManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private DataSpacesController dataSpacesController;

    [Header("Icons")]
    [SerializeField] private Sprite powerIcon;
    [SerializeField] private Sprite carIcon;
    [SerializeField] private Sprite personIcon;

    [Header("Window Prefabs")]
    [SerializeField] private GameObject municipalDSPrefab;
    [SerializeField] private GameObject regionalDSPrefab;
    [SerializeField] private GameObject closeButton;

    [Header("Window Parent")]
    [SerializeField] private Transform dataSpaceContainer;

    private Player activeplayer;
    private bool toggle = false;
    private void Start()
    {
        activeplayer = GameController.activePlayer;
    }

    public void DataSpaceButton()
    {
        activeplayer = GameController.activePlayer;
        toggle = !toggle;
        if (toggle)
        {
            ClearDataSpaceWindow();
            closeButton.SetActive(true);
            foreach (DataSpaceData data in activeplayer.DataSpaces)
            {
                GenerateDataSpaceObjects(data);
            }
        }
        else
        {
            ClearDataSpaceWindow();
            closeButton.SetActive(false);
        }
    }

    public void GenerateDataSpaceObjects(DataSpaceData dataSpace)
    {
        if (dataSpace == null)
        {
            Debug.LogWarning("GenerateDataSpaceObjects was called with a null dataSpace.");
            return;
        }

        GameObject prefabToUse = GetPrefabForDataSpace(dataSpace);

        if (prefabToUse == null)
        {
            Debug.LogWarning($"No prefab assigned for data space type: {dataSpace.type}");
            return;
        }

        CreateDataSpaceWindow(prefabToUse, dataSpace);
    }

    public void ClearDataSpaceWindowWithButton()
    {
        toggle = false;
        ClearDataSpaceWindow();
    }
    
    public void ClearDataSpaceWindow()
    {
        foreach (Transform child in dataSpaceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private GameObject GetPrefabForDataSpace(DataSpaceData dataSpace)
    {
        switch (dataSpace.type)
        {
            case DataSpaceType.municipal:
                return municipalDSPrefab;

            case DataSpaceType.regional:
                return regionalDSPrefab;

            default:
                return null;
        }
    }

    private void CreateDataSpaceWindow(GameObject prefab, DataSpaceData dataSpace)
    {
        GameObject dataSpaceWindow = Instantiate(prefab, dataSpaceContainer);

        Transform container = dataSpaceWindow.transform.Find("Data&CostArea");

        if (container == null)
        {
            Debug.LogWarning("Could not find Data&CostArea in the data space prefab.");
            return;
        }

        SetupEnableButton(container, dataSpace);
        SetupCostText(container, dataSpace);
        SetupCardClickAreas(container, dataSpace);
    }

    private void SetupEnableButton(Transform container, DataSpaceData dataSpace)
    {
        Transform buttonTransform = container.Find("Button");

        if (buttonTransform == null)
        {
            Debug.LogWarning("Could not find Button in Data&CostArea.");
            return;
        }

        Button button = buttonTransform.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning("Button object does not have a Button component.");
            return;
        }

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            bool enabled = dataSpacesController.EnableDataSpace(dataSpace);

            if (enabled)
            {
                Debug.Log($"Data space {dataSpace.id} is now enabled.");
            }
            else
            {
                Debug.Log($"Data space {dataSpace.id} could not be enabled yet.");
            }
        });
    }

    private void SetupCostText(Transform container, DataSpaceData dataSpace)
    {
        Transform costContainer = container.Find("CostText");

        if (costContainer == null)
        {
            Debug.LogWarning("Could not find CostText in Data&CostArea.");
            return;
        }

        Transform costTextTransform = costContainer.Find("text2");

        if (costTextTransform == null)
        {
            Debug.LogWarning("Could not find text2 inside CostText.");
            return;
        }

        TextMeshProUGUI costText = costTextTransform.GetComponent<TextMeshProUGUI>();

        if (costText == null)
        {
            Debug.LogWarning("text2 does not have a TextMeshProUGUI component.");
            return;
        }

        costText.text = $"Cost: {dataSpace.cost}";
    }

    private void SetupCardClickAreas(Transform container, DataSpaceData dataSpace)
    {
        Transform cardHolder = container.Find("Cards");

        if (cardHolder == null)
        {
            Debug.LogWarning("Could not find Cards in Data&CostArea.");
            return;
        }

        for (int i = 0; i < cardHolder.childCount; i++)
        {
            Transform clickableArea = cardHolder.GetChild(i);

            if (i >= dataSpace.neededData.Count)
            {
                Debug.LogWarning($"There are more clickable card areas than neededData entries. Extra clickable area index: {i}");
                continue;
            }

            DataSpaceDataRequired dataRequired = dataSpace.neededData[i];

            SetupClickableAreaVisuals(clickableArea, dataRequired);
            bool isCheckActive = SetupCheckMark(clickableArea, dataRequired);
            if (!isCheckActive)
            {
                SetupClickableAreaEvent(clickableArea, dataSpace, dataRequired);
            }
        }
    }

    private void SetupClickableAreaVisuals(Transform clickableArea, DataSpaceDataRequired dataRequired)
    {
        Image clickableAreaImage = clickableArea.GetComponent<Image>();

        if (clickableAreaImage != null)
        {
            clickableAreaImage.color = dataRequired.color;
        }
        else
        {
            Debug.LogWarning($"Clickable area {clickableArea.name} does not have an Image component.");
        }

        Transform iconTransform = clickableArea.Find("Icon");

        if (iconTransform == null)
        {
            Debug.LogWarning($"Clickable area {clickableArea.name} does not have a child named Icon.");
            return;
        }

        Image iconImage = iconTransform.GetComponent<Image>();

        if (iconImage == null)
        {
            Debug.LogWarning($"Icon child on {clickableArea.name} does not have an Image component.");
            return;
        }

        iconImage.sprite = GetIconForDataCardType(dataRequired.cardType);
    }

    private bool SetupCheckMark(Transform clickableArea, DataSpaceDataRequired dataRequired)
    {
        Transform checkMark = clickableArea.transform.Find("Check");
        if (checkMark == null)
        {
            Debug.LogWarning($"Clickable area {clickableArea.name} does not have a child named Check.");
            return false;
        }
        else
        {
            checkMark.gameObject.SetActive(dataRequired.isMet);
            return dataRequired.isMet;
        }
    }

    private void SetupClickableAreaEvent(Transform clickableArea, DataSpaceData dataSpace, DataSpaceDataRequired required)
    {
        Button button = clickableArea.GetComponent<Button>();
        Transform checkMark = clickableArea.transform.Find("Check");
        if (button == null)
        {
            button = clickableArea.gameObject.AddComponent<Button>();
        }

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            dataSpacesController.SubmitData(dataSpace, required);
            checkMark.gameObject.SetActive(required.isMet);
        });
    }

    private Sprite GetIconForDataCardType(DataCardType cardType)
    {
        switch (cardType.dataType)
        {
            case "Utility":
                return powerIcon;

            case "Traffic":
                return carIcon;

            case "Civil":
                return personIcon;

            default:
                Debug.LogWarning($"No icon assigned for DataCardType: {cardType.dataType}");
                return null;
        }
    }
}