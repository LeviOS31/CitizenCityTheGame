using System.Collections.Generic;
using UnityEngine;

public class PlayerContainerUI : MonoBehaviour
{
    [SerializeField] GameObject playerCardPrefab;
    private List<PlayerCardUI> players = new List<PlayerCardUI>();

    public List<PlayerCardUI> Initialize(List<Player> players)
    {
        List<PlayerCardUI> playerCards = new List<PlayerCardUI>();

        foreach (Player player in players) 
        {
            PlayerCardUI playerCardUI = Instantiate(playerCardPrefab).GetComponent<PlayerCardUI>();
            playerCardUI.gameObject.transform.SetParent(transform, false);
            playerCards.Add(playerCardUI);
            playerCardUI.Initialize(player);
        }

        return playerCards;
    }
}
