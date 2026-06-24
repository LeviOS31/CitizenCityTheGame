using Assets.Objects.Projects.Scripts;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;


// this script is responsible for managing the UI of the projects in the game. It handles opening and closing the project folder, navigating between projects, and passing along a click for the needed data.
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

    // this method reloads the UI for the projects, creating new project papers for each local and regional project, and setting up their positions and click events.
    public void ReloadProjectsUI(List<ProjectData> LocalProjects, List<ProjectData> RegionalProjects, ProjectData OpenProject)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ProjectPaper");
        foreach (GameObject obj in objects) {
            Destroy(obj);
        }

        foreach (ProjectData project in LocalProjects)
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

        foreach (ProjectData project in RegionalProjects)
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


    // this method opens the project folder UI, triggering animations and enabling buttons for interaction. It also checks the tutorial position to advance the tutorial if necessary.
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


    // this method closes the project folder UI, triggering animations and disabling buttons for interaction. It also plays a closing sound.
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

    // this method navigates to the next project in the UI, triggering animations and playing a sound effect.
    public void NextProject()
    {
        CurProject.GetComponent<Animator>().SetTrigger("Next");

        AudioSignalHandler.PlaySound.Invoke("FolderOpen");

        int curIndex = CurProject.transform.GetSiblingIndex();
        CurProject = PaperParent.GetChild(curIndex - 1).gameObject;
    }

    // this method navigates to the previous project in the UI, triggering animations and playing a sound effect.
    public void PrevProject()
    {
        int curindex = CurProject.transform.GetSiblingIndex();
        CurProject = PaperParent.GetChild(curindex + 1).gameObject;
        CurProject.GetComponent<Animator>().SetTrigger("Previous");

        AudioSignalHandler.PlaySound.Invoke("FolderClose");
    }

    // this method navigates to the local project in the UI, triggering animations for all projects.
    public void GotToLocalProject()
    {
        foreach (Transform child in PaperParent)
        {
            child.GetComponent<Animator>().SetTrigger("Previous");
        }
        CurProject = PaperParent.GetChild(PaperParent.childCount - 1).gameObject;
    }

    // this method navigates to the regional project in the UI, triggering animations for local projects and setting the current project to the regional project.
    public void GoToRegionalProject()
    {
        foreach (Transform child in PaperParent)
        {
            if (child.GetComponent<ProjectPrefab>().project.Type == ProjectType.Local)
            {
                child.GetComponent<Animator>().SetTrigger("Next");

            }

            if (child.GetComponent<ProjectPrefab>().project.Type == ProjectType.Regional)
            {
                CurProject = child.gameObject;
            }
        }
    }

    // this method is called when a needed data checkbox is clicked in the project UI. It invokes the CheckCards event to notify other parts of the game that a card has been selected.
    public void OnNeededDataClick(DataRequired data)
    {
        CheckCards?.Invoke(data);
    }

    // this method attempts to close the project folder UI if the specified window is not the current game object. It is used to ensure that only one window is open at a time.
    public void TryClose(GameObject window)
    {
        if (window != gameObject)
        {
            Close();
        }
    }
}
