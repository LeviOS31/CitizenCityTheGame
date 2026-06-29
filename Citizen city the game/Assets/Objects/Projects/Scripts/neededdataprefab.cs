using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Script for the visual representation of the data required for a project. It is used in the project view to show the data required for a project and whether it has been met or not.
public class neededdataprefab : MonoBehaviour
{
    DataRequired data;
    bool changed;
    bool ismet = false;

    public Image DataColor;
    public Image DataIcon;
    public Image Checked;

    void Start()
    {
        
    }

    void Update()
    {
        if (changed)
        {
            reload();
            changed = false;
        }

        if (data != null && data.IsMet && !ismet)
        {
            GetComponent<Button>().interactable = false;
            changed = true;
            ismet = true;
        }
    }

    // Sets the data for the prefab and updates the visual representation of the data required for a project.
    public void SetData(DataRequired _data)
    {
        data = _data;

        DataColor.color = data.Color;
        DataIcon.sprite = data.CardType.dataIcon;
        Checked.color = data.IsMet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
    }
    
    // Reloads the visual representation of the data required for a project.
    public void reload()
    {
        DataColor.color = data.Color;
        DataIcon.sprite = data.CardType.dataIcon;
        Checked.color = data.IsMet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
    }
}
