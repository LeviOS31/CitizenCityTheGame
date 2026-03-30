using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] public Image background;
    [SerializeField] public Image icon;
    [SerializeField] public TMP_Text _name;
    [SerializeField] public TMP_Text score;
    [SerializeField] public TMP_Text scorePerTurn;
    [SerializeField] public TMP_Text multiplier;
    [SerializeField] public Button button;

    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable()
    {
        button.enabled = true;
        background.color = Color.white;
    }

    public void Disable() 
    { 
        button.enabled= false;
        background.color = Color.lightGray;
    }

    public void UpdateScore()
    {
        player.UpdateScore();
        UpdateUI();
    }

    private void UpdateUI()
    {
        icon.color = player.playerColor;
        _name.text = player.name;
        score.text = player.playerScore.ToString();
        scorePerTurn.text = player.scorePerTurn.ToString();
        multiplier.text = player.playerMultiplier.ToString();
    }
}
