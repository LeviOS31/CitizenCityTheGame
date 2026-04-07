using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectPrefac : MonoBehaviour
{
    ProjectData project;

    public TextMeshProUGUI ProjectName;
    public TextMeshProUGUI ProjectDescription;
    public TextMeshProUGUI Points;
    public Image ProjectTypeColor1;
    public Image ProjectTypeColor2;
    public Image ProjectImage;
    public GameObject Datapanel;

    public GameObject NeededDataPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Assinginfo(ProjectData _project)
    {
        project = _project;

        ProjectName.text = project.name;
        ProjectDescription.text = project.Description;
        ProjectTypeColor1.color = project.Type == ProjectType.Personal ? new Color(0.365f, 0.6f, 1.0f) : new Color(0.918f, 0.247f, 0.247f);
        ProjectTypeColor2.color = project.Type == ProjectType.Personal ? new Color(0.365f, 0.6f, 1.0f) : new Color(0.918f, 0.247f, 0.247f);
        Points.text = "+" + project.ScoreValue;
        ProjectImage.sprite = project.Image;

        foreach (DataRequired data in project.NeededData)
        {
            GameObject instance = Instantiate(NeededDataPrefab, this.transform);
            instance.GetComponent<neededdataprefab>().SetData(data);
        }
    }
}
