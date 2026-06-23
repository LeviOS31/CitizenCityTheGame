using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsultantManagerUI : MonoBehaviour
{
    public readonly ConsultantManager consultantManager = new ConsultantManager();
    [SerializeField] List<GameObject> consultantSlots = new List<GameObject>();
    [SerializeField] GameObject DashboardBody;
    [SerializeField] Button ConsultancyButton;
    [SerializeField] Button ActiveContractsButton;
    [SerializeField] Button DataSpaceButton;
    [SerializeField] Button DataSpaceContractButton;
    [SerializeField] GameObject ConsultantUIPrefab;
    [SerializeField] GameObject ActiveContractUIPrefab;
    [SerializeField] GameObject ConsultancyScreen;
    [SerializeField] TMP_Text Funds;

    private readonly Color selectedTabColor = new Color(0.96f, 0.96f, 0.96f, 1f); // #F5F5F5
    private readonly Color unselectedTabColor = new Color(0.18f, 0.18f, 0.18f, 1f); // #2E2E2E

    private void Start()
    {
        ConsultantOption[] consultantOptions = Resources.LoadAll<ConsultantOption>("ScriptableObjects/Consultants");
        Consultant[] consultants = new Consultant[consultantOptions.Length];

        for (int i = 0; i < consultantOptions.Length; i++)
        {
            ConsultantOption option = consultantOptions[i];

            Consultant consultant = new Consultant(
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
        GameController.OpenWindow += TryClose;
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

            if (Tutorial.Tutorialposition == 8
                && !consultant.Specializations.Contains(GameController.dataCardTypes.First(c => c.dataType == "Utility"))
                && !consultant.Specializations.Contains(GameController.dataCardTypes.First(c => c.dataType == "Traffic"))
               )
            {
                instance.GetComponentInChildren<Button>().interactable = false;
            }
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
        UpdateTabVisuals(ConsultancyButton, ActiveContractsButton, DataSpaceButton, DataSpaceContractButton);
        CreateConsultants();
    }

    public void OpenActiveContractScreen()
    {
        UpdateTabVisuals(ActiveContractsButton, ConsultancyButton, DataSpaceButton, DataSpaceContractButton);
        CreateActiveContractsUIElements();

        if (Tutorial.Tutorialposition == 9)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }
    }

    public void OpenDataSpaceScreen()
    {
        UpdateTabVisuals(DataSpaceButton, ConsultancyButton, ActiveContractsButton, DataSpaceContractButton);
    }

    public void OpenDataContractSpaceScreen()
    {
        if (Tutorial.Tutorialposition == 23)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }
        UpdateTabVisuals(DataSpaceContractButton, ConsultancyButton, ActiveContractsButton, DataSpaceButton);
    }

    public void OpenConsultantDashboard()
    {
        GameController.OpenWindow?.Invoke(gameObject);

        if (Tutorial.Tutorialposition == 7 || Tutorial.Tutorialposition == 22)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }

        ConsultancyScreen.SetActive(true);
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }

        // Refreshes the active visuals and builds the consultants grid immediately
        OpenConsultancyScreen();
        Funds.text = GameController.activePlayer.money.ToString();
    }

    public void CloseConsultDashboard()
    {
        if (Tutorial.Tutorialposition == 25)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }

        ConsultancyScreen.SetActive(false);
    }

    private void RefreshUI()
    {
        OpenConsultancyScreen();
        Funds.text = GameController.activePlayer.money.ToString();
    }

    public void TryClose(GameObject window)
    {
        if (window != gameObject)
        {
            CloseConsultDashboard();
        }
    }

    public void RefreshFunds()
    {
        Funds.text = GameController.activePlayer.money.ToString();
    }

    /// <summary>
    /// Swaps button background ColorBlock states and text coloring depending on what tab is active.
    /// </summary>
    private void UpdateTabVisuals(Button activeButton, params Button[] inactiveButtons)
    {
        // 1. Style the Selected Active Tab (#F5F5F5 Background, Black Text)
        if (activeButton != null)
        {
            ColorBlock cb = activeButton.colors;
            cb.normalColor = selectedTabColor;
            cb.selectedColor = selectedTabColor;
            cb.highlightedColor = selectedTabColor;
            activeButton.colors = cb;

            TMP_Text buttonText = activeButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null) buttonText.color = Color.black;
        }

        // 2. Style all Unselected Inactive Tabs (#2E2E2E Background, White Text)
        foreach (Button inactiveButton in inactiveButtons)
        {
            if (inactiveButton != null)
            {
                ColorBlock cb = inactiveButton.colors;
                cb.normalColor = unselectedTabColor;
                cb.selectedColor = unselectedTabColor;
                cb.highlightedColor = unselectedTabColor;
                inactiveButton.colors = cb;

                TMP_Text buttonText = inactiveButton.GetComponentInChildren<TMP_Text>();
                if (buttonText != null) buttonText.color = Color.white;
            }
        }
    }
}
