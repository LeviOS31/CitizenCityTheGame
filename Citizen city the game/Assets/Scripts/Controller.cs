using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public enum State
{
    choosing,
    Cosultant,
    Dataspace
}

public class Controller : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;

    private bool isSwiping = false;
    private ProjectController projectController;

    [SerializeField] private float minSwipeDistance = 1f;

    private void Start()
    {
        projectController = GetComponent<ProjectController>();
    }

    void Update()
    {
        var mouse = Mouse.current;

        // Touch support
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                startPos = touch.position.ReadValue();
                isSwiping = true;
            }

            if (touch.press.wasReleasedThisFrame && isSwiping)
            {
                endPos = touch.position.ReadValue();
                DetectSwipe();
                isSwiping = false;
            }
        }
        // Mouse support for testing in editor
        else if (mouse != null)
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                startPos = mouse.position.ReadValue();
                isSwiping = true;
            }

            if (mouse.leftButton.wasReleasedThisFrame && isSwiping)
            {
                endPos = mouse.position.ReadValue();
                DetectSwipe();
                isSwiping = false;
            }
        }
    }

    void DetectSwipe()
    {
        Vector2 delta = endPos - startPos;

        if (delta.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                OnSwipeRight();
            else
                OnSwipeLeft();
        }
        else
        {
            if (delta.y > 0)
                OnSwipeUp();
            else
                OnSwipeDown();
        }
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
        projectController.swipe(true);
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
        projectController.swipe(false);
    }

    void OnSwipeUp()
    {
        Debug.Log("Swipe Up");
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

}
