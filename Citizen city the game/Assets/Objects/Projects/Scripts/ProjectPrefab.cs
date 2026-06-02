using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    // Update is called once per frame
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
            if (Tutorial.Tutorialposition == 26)
            {
                Tutorial.AdvanceTutorial?.Invoke();
            }
        }
        else if (project.IsDone)
        {
            Stamp.SetActive(true);
        }
    }

    async Task finish()
    {
        Debug.Log("Project " + project.Name + " is finished!");
        finished = true;
        GetComponent<Animator>().SetTrigger("complete");
        project.IsDone = true;
        await Task.Delay(917);
        Stamp.SetActive(true);
    }

    void Assigninfo(ProjectData _project)
    {
        project = _project;

        ProjectName.text = project.Name;
        ProjectDescription.text = project.Description;
        ProjectTypeColor1.color = project.Type == ProjectType.Personal ? new Color(0.365f, 0.6f, 1.0f) : new Color(0.918f, 0.247f, 0.247f);
        ProjectTypeColor2.color = project.Type == ProjectType.Personal ? new Color(0.365f, 0.6f, 1.0f) : new Color(0.918f, 0.247f, 0.247f);
        Points.text = "+" + project.ScoreMoney;
        ProjectImage.sprite = project.Image;

        

        foreach (Transform child in transform.Find("DataPanel"))
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (DataRequired data in project.NeededData)
        {
            GameObject instance = Instantiate(NeededDataPrefab, transform.Find("DataPanel"));
            instance.GetComponent<neededdataprefab>().SetData(data);
            instance.GetComponent<Button>().onClick.AddListener(() => onclickbutton(data));
        }
    }

    void onclickbutton(DataRequired data)
    {
        Debug.Log("Clicked on " + data.CardType);
        onclick.Invoke(data);
    }
}
