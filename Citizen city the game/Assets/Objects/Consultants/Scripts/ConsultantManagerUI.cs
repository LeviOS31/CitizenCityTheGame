using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsultantManagerUI : MonoBehaviour
{
    readonly ConsultantManager consultantManager = new ConsultantManager();
    [SerializeField] List<GameObject> consultantSlots = new List<GameObject>();
    [SerializeField] GameObject DashboardBody;
    [SerializeField] Button ConsultancyButton;
    [SerializeField] Button ActiveContractsButton;
    [SerializeField] Button DataSpaceButton;
    [SerializeField] GameObject ConsultantUIPrefab;

    private void Start()
    {
        ConsultantOption[] consultantOptions = Resources.LoadAll<ConsultantOption>("ScriptableObjects/Consultants");
        Consultant[] consultants = new Consultant[consultantOptions.Length];

        for (int i = 0; i < consultantOptions.Length; i++) 
        {
            ConsultantOption option = consultantOptions[i];
            
            Consultant consultant = 
                new Consultant(
                    option.Name,
                    option.Icon,
                    option.Specializations,
                    option.BasePrice,
                    option.PricePerData,
                    option.Duration
                    );

            consultants[i] = consultant;
        }

        consultantManager.SetConsultantOptions(consultants);
        CreateConsultants();
    }

    public void CreateConsultants()
    {
        foreach (Consultant consultant in consultantManager.consultantOptions)
        {
            ConsultantUI instance = Instantiate(ConsultantUIPrefab).GetComponent<ConsultantUI>();
            instance.Initialize(consultant);
            instance.transform.SetParent(DashboardBody.transform, false);
        }
    }

    private void ClearConsultants()
    {

    }

    public void OpenConsultancyScreen()
    {
        ConsultancyButton.GetComponentInChildren<TMP_Text>().color = Color.black;
        ActiveContractsButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        DataSpaceButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        CreateConsultants();
    }

    public void OpenActiveContractScreen()
    {
        ConsultancyButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        ActiveContractsButton.GetComponentInChildren<TMP_Text>().color = Color.black;
        DataSpaceButton.GetComponentInChildren<TMP_Text>().color = Color.white;
    }

    public void OpenDataSpaceScreen()
    {
        ConsultancyButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        ActiveContractsButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        DataSpaceButton.GetComponentInChildren<TMP_Text>().color = Color.black;
    }
}
