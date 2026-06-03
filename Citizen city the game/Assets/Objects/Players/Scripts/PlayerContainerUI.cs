using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerContainerUI : MonoBehaviour
{
    [SerializeField] GameObject playerCardPrefab;
    private List<PlayerCardUI> instantiatedCards = new List<PlayerCardUI>();

    public List<PlayerCardUI> Initialize(List<Player> players)
    {
        instantiatedCards.Clear();

        foreach (Player player in players)
        {
            PlayerCardUI playerCardUI = Instantiate(playerCardPrefab).GetComponent<PlayerCardUI>();
            playerCardUI.gameObject.transform.SetParent(transform, false);
            instantiatedCards.Add(playerCardUI);
            playerCardUI.Initialize(player);

            player.UIChange += SortAndRankLeaderboard;
        }

        SortAndRankLeaderboard();
        return instantiatedCards;
    }

    private void Update()
    {
        // Keeps values updating smoothly during gameplay simulations
        SortAndRankLeaderboard();
    }

    public void SortAndRankLeaderboard()
    {
        // Verify cross-references are hooked up if names changed during runtime generation
        foreach (var card in instantiatedCards)
        {
            if (card.GetScoreData() == null)
            {
                card.LinkToScoreData();
            }
        }

        // Sort descending by general calculated match points
        List<PlayerCardUI> sortedList = instantiatedCards
            .OrderByDescending(card => card.GetScoreData() != null ? card.GetScoreData().algemeneScore : 0)
            .ToList();

        // Update layout ordering hierarchy and text components
        for (int i = 0; i < sortedList.Count; i++)
        {
            sortedList[i].transform.SetSiblingIndex(i);
            sortedList[i].SetVisualRank(i + 1);
            sortedList[i].UpdateUI(); // Safely updates strings without generating massive garbage allocations
        }
    }

    private void OnDestroy()
    {
        foreach (var card in instantiatedCards)
        {
            if (card != null && card.GetPlayerModel() != null)
            {
                card.GetPlayerModel().UIChange -= SortAndRankLeaderboard;
            }
        }
    }
}