using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// this class is responsible for displaying the information of a project in the UI.
public class ProjectPrefab : MonoBehaviour
{
    public ProjectData project;

    public TextMeshProUGUI ProjectName;
    public TextMeshProUGUI ProjectDescription;
    public TextMeshProUGUI Points;
    public Image ProjectTypeColor1;
    public Image ProjectTypeColor2;
    public Image ProjectImage;
    public GameObject Stamp;
    public GameObject Datapanel;
    bool finished;

    public GameObject NeededDataPrefab;

    public event Action<DataRequired> onclick;

    public void Initialize()
    {
        Assigninfo(project);
    }

    // This method checks if the project is finished and updates the UI accordingly. If all required data for the project is met, it triggers the finish process and updates the tutorial if necessary.
    void Update()
    {
        if (project == null) return;

        if (finished) return;
        bool alldone = true;
        foreach (DataRequired data in project.NeededData)
        {
            if (!data.IsMet)
            {
                alldone = false;
            }
        }

        if (alldone && !project.IsDone)
        {
            finish();
            if (Tutorial.Tutorialposition == 27)
            {
                Tutorial.AdvanceTutorial?.Invoke();
            }
        }
        else if (project.IsDone)
        {
            Stamp.SetActive(true);
        }
    }

    // This method handles the completion of the project. It updates the UI to reflect the project's completion, and invokes any necessary events.
    async Task finish()
    {
        finished = true;
        GetComponent<Animator>().SetTrigger("complete");
        project.IsDone = true;
        await Task.Delay(917);
        Stamp.SetActive(true);
        ProjectController.ProjectDone?.Invoke();
    }

    // This method assigns the project data to the UI elements, updating the display with the project's name, description, type, score, and required data. It also sets up button click listeners for each required data item.
    void Assigninfo(ProjectData _project)
    {
        project = _project;

        ProjectName.text = project.Name;
        ProjectDescription.text = project.Description;
        ProjectTypeColor1.color = project.Type == ProjectType.Local ? new Color(0.365f, 0.6f, 1.0f) : new Color(0.918f, 0.247f, 0.247f);
        ProjectTypeColor2.color = project.Type == ProjectType.Local ? new Color(0.365f, 0.6f, 1.0f) : new Color(0.918f, 0.247f, 0.247f);
        Points.text = "+" + project.ScoreMoney;
        ProjectImage.sprite = project.Image;

        

        foreach (Transform child in transform.Find("DataPanel"))
        {
            GameObject.Destroy(child.gameObject);
        }


        // This loop instantiates a prefab for each required data item in the project, sets its data, and adds a click listener to handle user interaction.
        foreach (DataRequired data in project.NeededData)
        {
            GameObject instance = Instantiate(NeededDataPrefab, transform.Find("DataPanel"));
            instance.GetComponent<neededdataprefab>().SetData(data);
            instance.GetComponent<Button>().onClick.AddListener(() => onclickbutton(data));
        }
    }

    // This method is called when a required data button is clicked. It invokes the onclick event with the associated data.
    void onclickbutton(DataRequired data)
    {
        onclick.Invoke(data);
    }
}
