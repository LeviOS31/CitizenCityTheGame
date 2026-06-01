using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    [Header("UI Panel Reference")]
    [Tooltip("Drag your central full-screen detailed scoreboard panel here")]
    public GameObject detailedScoreboardPanel;

    private void Start()
    {
        // Hide the big chart board when the game starts
        if (detailedScoreboardPanel != null)
        {
            detailedScoreboardPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Hold TAB to view detailed stats chart
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            detailedScoreboardPanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            detailedScoreboardPanel.SetActive(false);
        }
    }
}