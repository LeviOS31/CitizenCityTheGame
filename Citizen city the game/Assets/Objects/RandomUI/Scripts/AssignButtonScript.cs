using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssignButtonScript : MonoBehaviour
{
    [SerializeField] private Texture2D HoverCursor;
    [SerializeField] private Texture2D DefaultCursor;

    float i = 0;
    void Update()
    {

        if(i < 1)
        {
            AssignComponentToButtons();
            i = 0;
        }
        
        i += Time.deltaTime;
    }

    async Task AssignComponentToButtons()
    {
        Button[] Buttons = GameObject.FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button button in Buttons) 
        {
            if (!button.gameObject.TryGetComponent<ButtonCursorChanger>(out var a))
            {
                ButtonCursorChanger comp = button.gameObject.AddComponent<ButtonCursorChanger>();
                comp.HoverCursor = HoverCursor;
                comp.DefaultCursor = DefaultCursor;
            }
        }
    }
}
