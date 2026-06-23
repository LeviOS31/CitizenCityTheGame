using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class ChangeRegionalContractNames : MonoBehaviour
{
    [SerializeField] private GameObject dataCard1stSet;
    [SerializeField] private GameObject dataCard2ndSet;
    [SerializeField] private GameObject player1Text;
    [SerializeField] private GameObject player2Text;
    //This method is used to add the player name on the section of the regional contracts that the player has to fill in with data
    public void ChageText(List<Player> players)
    {
        Color dataColor1 = dataCard1stSet.GetComponent<Image>().color;
        Color dataColor2 = dataCard2ndSet.GetComponent<Image>().color;

        foreach (Player player in players)
        {
            if (player.color == dataColor1)
            {
                TextMeshProUGUI playerText = player2Text.GetComponent<TextMeshProUGUI>();
                playerText.text = $"{player.name}:";
            }

            if (player.color == dataColor2)
            {
                TextMeshProUGUI playerText = player1Text.GetComponent<TextMeshProUGUI>();
                playerText.text = $"{player.name}:";
            }
        }
    }
}
