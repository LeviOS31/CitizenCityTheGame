using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ButtonCursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D HoverCursor;
    public Texture2D DefaultCursor;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Button button = GetComponent<Button>();

        if (HoverCursor != null && button.interactable)
        {
            Cursor.SetCursor(HoverCursor, hotSpot, CursorMode.Auto);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DefaultCursor != null)
        {
            Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private void OnDisable()
    {
        if (DefaultCursor != null)
        {
            Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}