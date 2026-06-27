using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    [Header("UI Panel Reference")]
    [Tooltip("Drag your central full-screen detailed scoreboard panel here")]
    public GameObject detailedScoreboardPanel;

    /// <summary>
    /// Called when the game starts. 
    /// Ensures the large scoreboard overlay is hidden by default when gameplay begins.
    /// </summary>
    private void Start()
    {
        // Hide the big chart board when the game starts
        if (detailedScoreboardPanel != null)
        {
            detailedScoreboardPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Called once per frame. 
    /// Listens for the Tab key input to show the scoreboard when pressed, 
    /// and hides it the exact moment the key is released.
    /// </summary>
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