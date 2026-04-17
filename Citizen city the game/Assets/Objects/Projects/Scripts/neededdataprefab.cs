using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public void SetData(DataRequired _data)
    {
        data = _data;

        DataColor.color = data.Color;
        DataIcon.sprite = data.CardType.dataIcon;
        Checked.color = data.IsMet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
    }

    public void reload()
    {
        DataColor.color = data.Color;
        DataIcon.sprite = data.CardType.dataIcon;
        Checked.color = data.IsMet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
    }
}
