using UnityEngine;
using UnityEngine.UI;

public class neededdataprefab : MonoBehaviour
{
    DataRequired data;

    public Image DataColor;
    public Image DataIcon;
    public Image Checked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(DataRequired _data)
    {
        data = _data;

        DataColor.color = data.Color;
        DataIcon.sprite = data.CardType.dataIcon;
        Checked.color = data.IsMet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
    }

    public void Clicked()
    {
        //TODO: add some sort of event call or something to request if the player has the right card 
        Debug.Log("clicked on data");

        data.IsMet = true;
    }
}
