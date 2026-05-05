using Assets.Objects.Projects.Scripts;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class ProjectsUI : MonoBehaviour
{
    public GameObject PrefabProjectPaper;
    public Transform PaperParent;
    public Transform FolderFront;
    public GameObject NextButton;
    public GameObject PrevButton;

    private GameObject CurProject;

    public event Action<DataRequired> CheckCards;

    public void ReloadProjectsUI(List<ProjectData> personalProjects, List<ProjectData> provincialProjects, ProjectData OpenProject)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ProjectPaper");
        foreach (GameObject obj in objects) {
            Destroy(obj);
        }

        foreach (ProjectData project in personalProjects)
        {
            GameObject projectPaper = Instantiate(PrefabProjectPaper, PaperParent);
            RectTransform rectTransform = projectPaper.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(-306.4f, -276.9f);
            rectTransform.sizeDelta = new Vector2(400, 482);

            projectPaper.GetComponent<ProjectPrefab>().project = project;
            projectPaper.GetComponent<ProjectPrefab>().Initialize();

            projectPaper.transform.SetAsFirstSibling();

            projectPaper.GetComponent<ProjectPrefab>().onclick += OnNeededDataClick;
        }

        foreach (ProjectData project in provincialProjects)
        {
            GameObject projectPaper = Instantiate(PrefabProjectPaper, PaperParent);
            RectTransform rectTransform = projectPaper.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(-306.4f, -276.9f);
            rectTransform.sizeDelta = new Vector2(400, 482);

            projectPaper.GetComponent<ProjectPrefab>().project = project;
            projectPaper.GetComponent<ProjectPrefab>().Initialize();

            projectPaper.transform.SetAsFirstSibling();

            projectPaper.GetComponent<ProjectPrefab>().onclick += OnNeededDataClick;
        }

        //TODO: load in open project


    }

    private void Update()
    {
        if (CurProject != null)
        {
            int curIndex = CurProject.transform.GetSiblingIndex();
            PrevButton.SetActive(curIndex < PaperParent.childCount - 1);
            NextButton.SetActive(curIndex > 0);
        }
    }

    public async void Open()
    {
        GetComponent<Animator>().SetTrigger("open");
        await Task.Delay(1000);
        FolderFront.SetAsFirstSibling();
        CurProject = PaperParent.GetChild(PaperParent.childCount - 1).gameObject;
    }

    public void Close()
    {
        FolderFront.SetSiblingIndex(transform.childCount - 3);
        GetComponent<Animator>().SetTrigger("close");
    }

    public void NextProject()
    {
        CurProject.GetComponent<Animator>().SetTrigger("Next");
        int curIndex = CurProject.transform.GetSiblingIndex();
        CurProject = PaperParent.GetChild(curIndex - 1).gameObject;
    }
    public void PrevProject()
    {
        int curindex = CurProject.transform.GetSiblingIndex();
        CurProject = PaperParent.GetChild(curindex + 1).gameObject;
        CurProject.GetComponent<Animator>().SetTrigger("Previous");
    }
    public void GoToPersonalProject()
    {
        foreach (Transform child in PaperParent)
        {
            child.GetComponent<Animator>().SetTrigger("Previous");
        }
        CurProject = PaperParent.GetChild(PaperParent.childCount - 1).gameObject;
    }
    public void GoToGroupProject()
    {
        foreach (Transform child in PaperParent)
        {
            if (child.GetComponent<ProjectPrefab>().project.Type == ProjectType.Personal)
            {
                child.GetComponent<Animator>().SetTrigger("Next");

            }

            if (child.GetComponent<ProjectPrefab>().project.Type == ProjectType.Provincial)
            {
                CurProject = child.gameObject;
            }
        }
    }
    public void GoToOpenProject()
    {
        foreach (Transform child in PaperParent)
        {
            if (child.GetComponent<ProjectPrefab>().project.Type != ProjectType.Open)
            {
                child.GetComponent<Animator>().SetTrigger("Next");
            }

        }
        CurProject = PaperParent.GetChild(0).gameObject;
    }

    public void OnNeededDataClick(DataRequired data)
    {
        Debug.Log("sending to controller " + data.CardType);
        CheckCards?.Invoke(data);
    }
}
