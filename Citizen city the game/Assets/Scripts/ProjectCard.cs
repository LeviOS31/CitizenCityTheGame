using System;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;

public class ProjectCard : MonoBehaviour
{
    [SerializeField] GameObject requirementPrefab;
    [SerializeField] GameObject container;
    [SerializeField] List<Player> players;
    [SerializeField] TMP_Text rewardText;

    public int reward = 5;

    //public List<RequiredDataCard> requirements = new List<RequiredDataCard>();

    //void Start()
    //{
    //    reward = UnityEngine.Random.Range(2, 11);
    //    rewardText.text = reward.ToString();

    //    for (int i = 0; i < 3; i++) 
    //    { 
    //        int rndPlayer = UnityEngine.Random.Range(0 , players.Count);
    //        int rndType = UnityEngine.Random.Range(0, 3);

    //        RequiredDataCard card = new RequiredDataCard(players[rndPlayer].playerColor, (DataTypes)rndType);
    //        requirements.Add(card);
    //    }

    //    foreach (RequiredDataCard card in requirements)
    //    {
    //        Debug.Log(card);
    //        GameObject instance = Instantiate(requirementPrefab);
    //        instance.GetComponent<RequirementUI>().Setup(card);
    //        instance.transform.SetParent(container.transform, false);
    //    }
    //}

    //public void CompleteCard()
    //{
    //    GameController controller = FindAnyObjectByType<GameController>();

    //    int totalCards = 0;

    //    foreach (RequiredDataCard card in requirements) 
    //    {
    //        List<DataCard> playerHand = controller.activePlayer.cards;
    //        playerHand.AddRange(controller.activePlayer.tradableCards);

    //        DataCard playerCard = playerHand.Find(pc => pc.Color == card.color && pc.Type == card.type);

    //        if (playerCard != null)
    //        {
    //            totalCards++;
    //        }
    //    }

    //    if (totalCards == requirements.Count)
    //    {
    //        controller.activePlayer.scorePerTurn += reward;
    //        Destroy(gameObject);
    //    }
    //}
}
