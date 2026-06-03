using UnityEngine;
using UnityEngine.UIElements;

public class CardSelectionUI : MonoBehaviour
{
    [SerializeField] RectTransform container;
    [SerializeField] float scrollSensitivity = 0.5f;
    [SerializeField] float maxClamp = -0;
    [SerializeField] float minClamp = -1000;
    private bool isHovering;

    private void Update()
    {
        ScrollMenu();
    }

    private void ScrollMenu()
    {
        float mouseScrollDelta = Input.mouseScrollDelta.y * -scrollSensitivity;
        if (Mathf.Abs(mouseScrollDelta) == 0) return;
        Debug.Log("scroll");

        float newYPosition = Mathf.Clamp(container.anchoredPosition.y, minClamp, maxClamp);

        container.anchoredPosition = new Vector2(0, newYPosition);
    }
}
