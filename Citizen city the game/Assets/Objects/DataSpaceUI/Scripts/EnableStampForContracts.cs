using System;
using UnityEngine;
using UnityEngine.UI;
public class EnableStampForContracts : MonoBehaviour
{
    [SerializeField] private GameObject stamp;
    [SerializeField] private GameObject dataCard1stSet; //This card must be on the same side that the first stamp is located
    [SerializeField] private GameObject dataCard2ndSet; //This card must be on the same side that the second stamp is located
    [SerializeField] private GameObject firstSignedStamp; //This stamp must be on the same side that the card of the 1st set is located
    [SerializeField] private GameObject secondSignedStamp;//This stamp must be on the same side that the card of the 2nd set is located
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
    }

    private void OnDestroy()
    {
        enableStamp -= EnableStamp;
        enableSignedStamp -= EnableSignedStamp;
    }
    //Enables the stamp that tell if the data space is actiavated when the player enables it through the contracts
    public void EnableStamp(DataSpaceData data)
    {
        if (data.isEnabled)
        {            
            stamp.SetActive(true);
        }     
    }
    //Enables the stamp that tells if a player has invested in a regional contract
    public void EnableSignedStamp(DataSpaceData dataSpaceData)
    {
        if (dataCard1stSet == null || dataCard2ndSet == null) return;

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
    //Checks if other Player has invested in the regional contract
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
    //Checks if the active Player has invested in the regional contract
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
