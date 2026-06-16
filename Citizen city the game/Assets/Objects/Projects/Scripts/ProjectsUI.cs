using Assets.Objects.Projects.Scripts;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ProjectsUI : MonoBehaviour
{
    public GameObject PrefabProjectPaper;
    public Transform PaperParent;
    public Transform FolderFront;
    public GameObject NextButton;
    public GameObject PrevButton;

    private GameObject CurProject;
    private bool open;

    public event Action<DataRequired> CheckCards;

    private void Start()
    {
        GameController.NewTurn += (Player) => Close();
        GameController.OpenWindow += TryClose;
    }

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
        GameController.OpenWindow?.Invoke(gameObject);

        if (Tutorial.Tutorialposition == 4 || Tutorial.Tutorialposition == 14 || Tutorial.Tutorialposition == 26)
        {
            Tutorial.AdvanceTutorial?.Invoke();
        }

        if (open) return;
        open = true;
        GetComponent<Animator>().SetTrigger("open");

        AudioSignalHandler.PlaySound.Invoke("FolderOpen");

        foreach (Transform child in PaperParent)
        {
            child.GetComponent<Animator>().ResetTrigger("Next");
            child.GetComponent<Animator>().ResetTrigger("Previous");
        }

        await Task.Delay(1000);
        FolderFront.SetAsFirstSibling();
        CurProject = PaperParent.GetChild(PaperParent.childCount - 1).gameObject;

        Button[] buttons = transform.GetComponentsInChildren<Button>();

        foreach (Button btn in buttons)
        {
            btn.interactable = true;
        }

    }

    public async void Close()
    {
        if (!open) return;
        open = false;
        Button[] buttons = transform.GetComponentsInChildren<Button>();

        foreach (Button btn in buttons)
        {
            btn.interactable = false;
        }

        foreach (Transform child in PaperParent)
        {
            child.GetComponent<Animator>().SetTrigger("Previous");
        }


        await Task.Delay(500);

        FolderFront.SetSiblingIndex(transform.childCount - 6);
        GetComponent<Animator>().SetTrigger("close");


        AudioSignalHandler.PlaySound.Invoke("FolderClose");
    }

    public void NextProject()
    {
        CurProject.GetComponent<Animator>().SetTrigger("Next");

        AudioSignalHandler.PlaySound.Invoke("FolderOpen");

        int curIndex = CurProject.transform.GetSiblingIndex();
        CurProject = PaperParent.GetChild(curIndex - 1).gameObject;
    }
    public void PrevProject()
    {
        int curindex = CurProject.transform.GetSiblingIndex();
        CurProject = PaperParent.GetChild(curindex + 1).gameObject;
        CurProject.GetComponent<Animator>().SetTrigger("Previous");

        AudioSignalHandler.PlaySound.Invoke("FolderClose");
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

    public void OnNeededDataClick(DataRequired data)
    {
        Debug.Log("sending to controller " + data.CardType);
        CheckCards?.Invoke(data);
    }

    public void TryClose(GameObject window)
    {
        if (window != gameObject)
        {
            Close();
        }
    }
}
