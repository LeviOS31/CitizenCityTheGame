using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EnableStampForContracts : MonoBehaviour
{
    [SerializeField] private GameObject stamp;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject dataCard1stSet;
    [SerializeField] private GameObject dataCard2ndSet;
    [SerializeField] private GameObject firstSignedStamp;
    [SerializeField] private GameObject secondSignedStamp;
    public static Action<DataSpaceData> enableStamp;
    public static Action<DataSpaceData> enableSignedStamp;
    private Player player = GameController.activePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(stamp == null)
        {
            Debug.Log("Stamp wasnt assigned");
            return;
        }
        enableStamp += EnableStamp;
        enableSignedStamp += EnableSignedStamp;
        stamp.SetActive(false);
    }

    public void EnableStamp(DataSpaceData data)
    {
        if(text == null)
        {
            Debug.Log("text isnt there");
            return;
        }
        
        string id = text.GetComponent<TextMeshProUGUI>().text;
        if(data.id.ToString() == id)
        {
            if (data.isEnabled)
            {
                stamp.SetActive(true);
            }            
        }       
    }

    public void EnableSignedStamp(DataSpaceData dataSpaceData)
    {
        Color dataCard1stSetColor = dataCard1stSet.GetComponent<Image>().color;
        Color dataCard2ndSetColor = dataCard2ndSet.GetComponent<Image>().color;
        bool otherPlayerInvested = CheckIfOtherPlayerHasInvested(dataSpaceData);
        bool activePlayerInvested = CheckIfActivePlayerHasInvested(dataSpaceData);
        
        if(dataCard1stSetColor == player.color)
        {
            if (otherPlayerInvested)
            {
                firstSignedStamp.SetActive(true);
            }
        }
        else
        {
            if (activePlayerInvested)
            {
                firstSignedStamp.SetActive(true);
            }
        }

        if(dataCard2ndSetColor == player.color)
        {
            if (otherPlayerInvested)
            {
                secondSignedStamp.SetActive(true);
            }
        }
        else
        {
            if (activePlayerInvested)
            {
                secondSignedStamp.SetActive(true);
            }
        }
    }

    private bool CheckIfOtherPlayerHasInvested(DataSpaceData dataSpaceData)
    {
        int areAllDataInvetsed = 0;
        bool hasinvested = false;
        
        foreach(DataSpaceDataRequired data in dataSpaceData.neededData)
        {
            if(data.color == player.color && data.isMet)
            {
                areAllDataInvetsed++;
            }

            if(dataSpaceData.player1HasInvested || dataSpaceData.player2HasInvested)
            {
                hasinvested = true;
            }
        }

        if(areAllDataInvetsed == 3 && hasinvested)
        {
            return true;
        }

        return false;
    }

    private bool CheckIfActivePlayerHasInvested(DataSpaceData dataSpaceData)
    {
        int areAllDataInvetsed = 0;
        bool hasinvested = false;
        
        foreach(DataSpaceDataRequired data in dataSpaceData.neededData)
        {
            if(data.color != player.color && data.isMet)
            {
                areAllDataInvetsed++;
            }

            if(dataSpaceData.player1HasInvested || dataSpaceData.player2HasInvested)
            {
                hasinvested = true;
            }
        }

        if(areAllDataInvetsed == 3 && hasinvested)
        {
            return true;
        }

        return false;
    }
}
