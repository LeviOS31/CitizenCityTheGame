using UnityEngine;
using UnityEngine.EventSystems;

public class DataSpaceGameObjectManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private DataSpacesController dataSpacesController;
    [SerializeField] private GameObject municipalDSPrefab;
    [SerializeField] private GameObject regionalDSPrefab;
    [SerializeField] private Transform dataSpaceContainer;
    private Player activeplayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeplayer = gameController.activePlayer;
    }

    public void ReloadPlayer(Player player)
    {
        activeplayer = player;
    }

    private void GenerateDataSpaceObjects()
    {
        foreach (Transform child in dataSpaceContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (DataSpaceData data in activeplayer.DataSpaces)
        {
            switch (data.type)
            {
                case DataSpaceType.municipal:
                    GameObject municiaplDataSpace = Instantiate(municipalDSPrefab, dataSpaceContainer);
                    Transform container = municiaplDataSpace.transform.Find("Data&CostArea");
                    container.transform.Find("Button").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { dataSpacesController.EnableDataSpace(data.id); });
                    
                    EventTrigger municipalImageTrigger = container.transform.Find("Image").GetComponent<EventTrigger>();

                    EventTrigger.Entry municipalEntry = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerClick
                    };

                    municipalEntry.callback.AddListener(delegate
                    {
                        dataSpacesController.SubmitData(data.id);
                    });

                    municipalImageTrigger.triggers.Add(municipalEntry);
                    break;
                case DataSpaceType.regional:
                    GameObject regionalDataSpace = Instantiate(municipalDSPrefab, dataSpaceContainer);
                    Transform container2 = regionalDataSpace.transform.Find("Data&CostArea");
                    container2.transform.Find("Button").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { dataSpacesController.EnableDataSpace(data.id); });
                    EventTrigger municipalImageTrigger2 = container2.transform.Find("Image").GetComponent<EventTrigger>();

                    EventTrigger.Entry municipalEntry2 = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerClick
                    };

                    municipalEntry2.callback.AddListener(delegate
                    {
                        dataSpacesController.SubmitData(data.id);
                    });

                    municipalImageTrigger2.triggers.Add(municipalEntry2);
                    break;
            }
        }
    }
}
