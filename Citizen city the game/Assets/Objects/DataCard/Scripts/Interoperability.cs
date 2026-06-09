using UnityEngine;

public class Interoperability : MonoBehaviour
{
    private int MoneyCost = 100;
    private int TimeCost = 1;

    public void OnClick()
    {
        if (GameController.activePlayer.money >= MoneyCost)
        {
            GameController.activePlayer.money -= MoneyCost;
        }
    }
}
