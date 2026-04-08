using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Globalization;

public class ProjectController : MonoBehaviour
{
    public State currentState = State.choosing;
    public TextMeshProUGUI TotalMoneyText;
    public int totalmoney = 20000000;
    public List<Project> projects = new List<Project>();
    public TextMeshProUGUI projectDescription;
    public TextMeshProUGUI Budget;
    public GameObject DataPrefab;
    public GameObject DataList;
    public Texture2D metIcon;
    public Texture2D notmetIcon;
    public Animator animator;
    public GameObject popUp;
    public TextMeshProUGUI popUpText;


    public void Start()
    {
        Project project = projects.First();

        foreach (DataRequirement data in project.DataList)
        {
            data.isMet = false;
        }

        project.DataList.First().isMet = true;

        UpdateUI(project);
    }

    private void UpdateUI(Project curproject)
    {
        TotalMoneyText.text = "€" + totalmoney.ToString("N0", CultureInfo.GetCultureInfo("nl-NL"));

        projectDescription.text = curproject.Projectdescription;
        Budget.text = "€ " + curproject.ProjectBudget.ToString();

        foreach (Transform child in DataList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (DataRequirement data in curproject.DataList)
        {
            Debug.Log("data" + data.requirementName);
            GameObject dataitem = Instantiate(DataPrefab, DataList.transform);
            TextMeshProUGUI dataText = dataitem.GetComponentInChildren<TextMeshProUGUI>();
            RawImage dataImage = dataitem.GetComponentInChildren<RawImage>();

            dataText.text = data.requirementName;

            if (data.isMet)
            {
                dataImage.texture = metIcon;
            }
            else
            {
                dataImage.texture = notmetIcon;
            }
        }
    }

    public void swipe(bool right)
    {
        if (currentState != State.choosing) return;

        if (right)
        {
            currentState = State.Cosultant;
            popUp.SetActive(true);
            popUpText.text = "Hiring Consultancy\r\n\r\nHiring a consultancy to find and visualize the data needed usually costs 30% to 40% of your total budget.\r\n\r\nAre you sure you want to hire a consultancy?";
        }
        else
        {
            currentState = State.Dataspace;
            popUp.SetActive(true);
            popUpText.text = "Data Space Terms of Service\r\n\r\nBy accessing or using the data space, you agree to be bound by these Terms of Service. If you do not agree with any part of these terms, you should not use the service.";
        }
    }

    public void choicebuttons(bool Yes)
    {
        popUp.SetActive(false);
        if (!Yes)
        {
            currentState = State.choosing;
        }
        else
        {
            if (currentState == State.Cosultant)
            {
                animator.SetTrigger("Swipe-Left");
            }
            else if (currentState == State.Dataspace)
            {
                animator.SetTrigger("Swipe-Right");
            }
        }
    }

    public void ConsultantDone()
    {
        totalmoney = totalmoney - (int)(projects.First().ProjectBudget * 1.35f);
        projects.First().DataList.ForEach(data => data.isMet = true);

        UpdateUI(projects.First());
    }
}

    //public void DataSpaceDone(List<DataSpaceOption> dataSpaceOption)
    //{
    //    foreach (DataSpaceOption option in dataSpaceOption)
    //    {
    //        DataRequirement? req = projects.First().DataList.Find(item => item.type == option.dataType);
    //        if (req != null)
    //        {
    //            req.isMet = true;
    //            continue;
    //        } 
    //    }

    //    UpdateUI(projects.First());
    //}}
