using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] GameObject ActiveContractUIPrefab;
    [SerializeField] GameObject ConsultancyScreen;
    [SerializeField] TMP_Text Funds;

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
        CloseConsultDashboard();
        GameController.UpdateUI += RefreshUI;
        GameController.NewTurn += (Player) => CloseConsultDashboard();
    }

    private void CreateConsultants()
    {
        ClearDashboardBody();

        Debug.Log(consultantManager.consultantOptions.Length);

        foreach (Consultant consultant in consultantManager.consultantOptions)
        {
            ConsultantUI instance = Instantiate(ConsultantUIPrefab).GetComponent<ConsultantUI>();
            instance.Initialize(consultant);
            instance.transform.SetParent(DashboardBody.transform, false);
        }
    }

    private void CreateActiveContractsUIElements()
    {
        ClearDashboardBody();
         
        List<HiredConsultant> hiredConsultants;
        if (!consultantManager.playerHiredConsultants.TryGetValue(GameController.activePlayer, out hiredConsultants)) return;

        foreach (HiredConsultant consultant in hiredConsultants)
        {
            ActiveContractUI instance = Instantiate(ActiveContractUIPrefab).GetComponent<ActiveContractUI>();
            instance.Initialize(consultant);
            instance.transform.SetParent(DashboardBody.transform, false);
        }
    }

    private void ClearDashboardBody()
    {
        ConsultantUI[] children = DashboardBody.GetComponentsInChildren<ConsultantUI>();

        foreach (ConsultantUI child in children) 
        { 
            Destroy(child.gameObject);
        }

        ActiveContractUI[] activeContracts = DashboardBody.GetComponentsInChildren<ActiveContractUI>();

        foreach (ActiveContractUI activeContract in activeContracts) 
        { 
            Destroy(activeContract.gameObject); 
        }
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
        CreateActiveContractsUIElements();
    }

    public void OpenDataSpaceScreen()
    {
        ConsultancyButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        ActiveContractsButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        DataSpaceButton.GetComponentInChildren<TMP_Text>().color = Color.black;
    }

    public void OpenConsultantDashboard()
    {
        ConsultancyScreen.SetActive(true);
        CreateConsultants();
        Funds.text = GameController.activePlayer.money.ToString();
    }

    public void CloseConsultDashboard()
    {
        ConsultancyScreen.SetActive(false);
    }

    private void RefreshUI()
    {
        OpenConsultancyScreen();
        Funds.text = GameController.activePlayer.money.ToString();
    }
}
