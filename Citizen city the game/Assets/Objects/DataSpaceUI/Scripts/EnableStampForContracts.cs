using System;
using UnityEngine;
using TMPro;
public class EnableStampForContracts : MonoBehaviour
{
    [SerializeField] private GameObject stamp;
    [SerializeField] private GameObject text;
    public static Action<DataSpaceData> enableStamp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(stamp == null)
        {
            Debug.Log("Stamp wasnt assigned");
            return;
        }
        enableStamp += EnableStamp;
        stamp.SetActive(false);
    }

    public void EnableStamp(DataSpaceData data)
    {
        string id = text.GetComponent<TextMeshProUGUI>().text;
        if(data.id.ToString() == id)
        {
            stamp.SetActive(true);
        }        
    }
}
