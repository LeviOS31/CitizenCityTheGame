using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Action AdvanceTutorial;
    public static int Tutorialposition = -1;

    void Start()
    {
        AdvanceTutorial += advanceTutorial;
    }

    public void advanceTutorial()
    {
        Tutorialposition++;

        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }

        transform.GetChild(Tutorialposition).gameObject.SetActive(true);
    }
}
