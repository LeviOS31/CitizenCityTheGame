using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ConnectorNode : MonoBehaviour
{
    public bool isConnected = false;
    public ConnectorNode nodePair;
    [SerializeField] public Player playerOne;
    [SerializeField] public Player playerTwo;

    [SerializeField] Image playerOneColor;
    [SerializeField] Image playerTwoColor;

    [SerializeField] Image playerOneCar;
    [SerializeField] Image playerOnePerson;
    [SerializeField] Image playerOneUtil;
    [SerializeField] Image playerTwoCar;
    [SerializeField] Image playerTwoPerson;
    [SerializeField] Image playerTwoUtil;

    private GameController controller;
}

    //List<RequiredDataCard> playerOneRequirement = new List<RequiredDataCard>();
    //List<RequiredDataCard> playerTwoRequirement = new List<RequiredDataCard>();

//    public void Start()
//    {
//        playerOneRequirement.Add(new RequiredDataCard(playerTwo.playerColor, DataTypes.Traffic));
//        playerOneRequirement.Add(new RequiredDataCard(playerTwo.playerColor, DataTypes.Citizen));
//        playerOneRequirement.Add(new RequiredDataCard(playerTwo.playerColor, DataTypes.Utility));

//        playerTwoRequirement.Add(new RequiredDataCard(playerOne.playerColor, DataTypes.Traffic));
//        playerTwoRequirement.Add(new RequiredDataCard(playerOne.playerColor, DataTypes.Citizen));
//        playerTwoRequirement.Add(new RequiredDataCard(playerOne.playerColor, DataTypes.Utility));

//        playerOneColor.color = playerTwo.playerColor;
//        playerTwoColor.color = playerOne.playerColor;
//    }


//    public void ClickNode()
//    {
//        if (controller == null) 
//        { 
//            controller = FindAnyObjectByType<GameController>();
//        }

//        if (controller.activePlayer == playerOne)
//        {
//            List<DataCard> playerHand = playerOne.cards;

//            List<RequiredDataCard> cardsToRemove = new List<RequiredDataCard>();

//            foreach (RequiredDataCard card in playerOneRequirement)
//            {
//                DataCard datacard = playerHand.Find(pc => pc.Type == card.type && pc.Color == card.color);

//                if (datacard != null) 
//                { 
//                    DataCard target = playerOne.cards.Find(pc => pc.Type == datacard.Type && pc.Color == datacard.Color);
//                    playerOne.cards.Remove(target);

//                    Debug.Log(card.type + " " + card.color);

//                    //switch (card.type) 
//                    //{
//                    //    case(DataTypes.Traffic):
//                    //        playerOneCar.color = Color.green;
//                    //        break;
//                    //    case(DataTypes.Citizen):
//                    //        playerOnePerson.color = Color.green;
//                    //        break;
//                    //    case(DataTypes.Utility):
//                    //        playerOneUtil.color = Color.green;
//                    //        break;
//                    //}

//                    cardsToRemove.Add(card);
//                }
//            }

//            foreach (RequiredDataCard card in cardsToRemove)
//            {
//                playerOneRequirement.Remove(card);
//            }
//        }

//        if (controller.activePlayer == playerTwo)
//        {
//            List<DataCard> playerHand = playerTwo.cards;

//            List<RequiredDataCard> cardsToRemove = new List<RequiredDataCard>();

//            foreach (RequiredDataCard card in playerTwoRequirement)
//            {
//                DataCard datacard = playerHand.Find(pc => pc.Type == card.type && pc.Color == card.color);

//                if (datacard != null)
//                {
//                    DataCard target = playerTwo.cards.Find(pc => pc.Type == datacard.Type && pc.Color == datacard.Color);
//                    playerTwo.cards.Remove(target);

//                    Debug.Log(card.type + " " + card.color + " player 2");

//                    //switch (card.type)
//                    //{
//                    //    case (DataTypes.Traffic):
//                    //        playerTwoCar.color = Color.green;
//                    //        break;
//                    //    case (DataTypes.Citizen):
//                    //        playerTwoPerson.color = Color.green;
//                    //        break;
//                    //    case (DataTypes.Utility):
//                    //        playerTwoUtil.color = Color.green;
//                    //        break;
//                    //}

//                    cardsToRemove.Add(card);
//                }
//            }

//            foreach (RequiredDataCard card in cardsToRemove)
//            {
//                playerTwoRequirement.Remove(card);
//            }
//        }

//        if (playerOneRequirement.Count == 0 && playerTwoRequirement.Count == 0)
//        {
//            isConnected = true;
//        }

//        FindAnyObjectByType<CardHolderUI>().Render(controller.activePlayer);
//    }

//    public void DrawDataSpaceCard()
//    {
//        if (!isConnected) return;

//        playerOne.cards.Add(playerTwo.DrawDataspaceCard());
//        playerTwo.cards.Add(playerOne.DrawDataspaceCard());
//    }
//}

//public class RequiredDataCard
//{
//    public bool isDelivered = false;
//    public Color color;
//    //public DataTypes type;

//    //public RequiredDataCard(Color color, DataTypes type)
//    //{
//    //    this.color = color;
//    //    this.type = type;
//    //}
//}

