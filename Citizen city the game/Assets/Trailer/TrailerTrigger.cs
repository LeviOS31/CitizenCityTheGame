using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TrailerTrigger : MonoBehaviour
{
    public Animator cameraanim;
    public Animator cardsanim;
    public Animator moneyanim;
    public Animator popupanim;
    public Animator build;
    public GameController controller;
    public CardHolderUI cardholder;
    public Animator DataspaceUICompaniesAnim;
    public Animator DataspaceUIMunicipalitiesAnim;
    public GameObject DataspaceButton;
    public GameObject DataspaceSetupButton;

    bool setup = false;
    bool playing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) 
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DataspaceModelAnim();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            PopupAnim();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            BuildingAnim();
        }

    }

    async Task DataspaceModelAnim()
    {
        playing = true;
        cameraanim.SetTrigger("move1");

        await Task.Delay(1000);
        DataspaceUIMunicipalitiesAnim.SetTrigger("open");
        await Task.Delay(1000);
        DataspaceUIMunicipalitiesAnim.SetTrigger("complete");

        cardsanim.gameObject.SetActive(true);
        moneyanim.gameObject.SetActive(true);
        await Task.Delay(4000);
        cardsanim.SetTrigger("cards");
        await Task.Delay(3000);
        moneyanim.SetTrigger("coins");
        controller.activePlayer.cards.Add(new DataCard(controller.dataCardTypes.FirstOrDefault(obj => obj.dataType == "Civil"), new Color(1,0,0)));
        cardholder.CreateCards(controller.activePlayer);
        await Task.Delay(2000);
        cardsanim.gameObject.SetActive(false);
        moneyanim.gameObject.SetActive(false);
        playing = false;
    }

    async Task PopupAnim()
    {
        popupanim.SetTrigger("open");
    }

    async Task BuildingAnim()
    {
        playing = true;
        build.SetTrigger("Build");
        await Task.Delay(100);
        cameraanim.SetTrigger("move2");
        await Task.Delay(3000);
        playing = false;
    }

    public async void ConsultantsCards()
    {
        await Task.Delay(500);
        controller.activePlayer.cards.Clear();
        controller.activePlayer.cards.Add(new DataCard(controller.dataCardTypes.FirstOrDefault(obj => obj.dataType == "Civil"), controller.activePlayer.color));
        controller.activePlayer.cards.Add(new DataCard(controller.dataCardTypes.FirstOrDefault(obj => obj.dataType == "Traffic"), controller.activePlayer.color));
        controller.activePlayer.cards.Add(new DataCard(controller.dataCardTypes.FirstOrDefault(obj => obj.dataType == "Utility"), controller.activePlayer.color));
        cardholder.CreateCards(controller.activePlayer);
    }

    public async void DataSpaceSetupCity()
    {
        if (setup)
        {
            return;
        }

        await Task.Delay(200);
        cameraanim.SetTrigger("move3");
        await Task.Delay(1000);
        DataspaceUICompaniesAnim.SetTrigger("open");
        await Task.Delay(1000);
        DataspaceUICompaniesAnim.SetTrigger("complete");
        await Task.Delay(2000);
        DataspaceButton.SetActive(true);
        DataspaceSetupButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Connect municipalities";

        setup = true;

        DataspaceSetupButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ConnectMunicipalities());

    }

    public void ConnectMunicipalities()
    {
        DataspaceModelAnim();
    }
}
