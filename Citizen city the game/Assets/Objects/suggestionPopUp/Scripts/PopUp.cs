using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite Closebutton;
    public Sprite ClosebuttonClicked;
    public GameObject Button;
    Animator animator;

    async Task Start()
    {
        animator = GetComponent<Animator>();
        await Task.Delay(10000);
        animator.SetTrigger("open");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Button.GetComponent<Image>().sprite = ClosebuttonClicked;
        Button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 94.73684210526316f);
        Button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 93.10344827586207f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Button.GetComponent<Image>().sprite = Closebutton;
        Button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
        Button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
    }

    public void onclick()
    {
        animator.SetTrigger("close");
    }
}
