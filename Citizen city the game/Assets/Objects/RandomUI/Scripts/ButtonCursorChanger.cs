using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class ButtonCursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D HoverCursor;
    public Texture2D DefaultCursor;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HoverCursor != null)
        {
            Debug.Log("WHYYYYY");
            Cursor.SetCursor(HoverCursor, hotSpot, CursorMode.Auto);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DefaultCursor != null)
        {
            Debug.Log("NOOOOOTTTT");
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