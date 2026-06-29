using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Action AdvanceTutorial;
    public static int Tutorialposition = -1;

    public Material highlightMaterial;
    public float padding = 10f;
    public GameObject MainPanel;

    void Start()
    {
        AdvanceTutorial += advanceTutorial;
    }

    // This method is called when the tutorial should advance to the next step.
    public void advanceTutorial()
    {
        Tutorialposition++;

        Debug.Log("Tutorial Position: " + Tutorialposition);

        RectTransform targetUIElement = new RectTransform();
        bool found = false;

        // Search for the UI element with the matching Tutorialposition
        foreach (GameObject TutorialHelper in GameObject.FindGameObjectsWithTag("Tutorial"))
        {
            if (TutorialHelper.TryGetComponent<TutorialHelperObject>(out TutorialHelperObject helperObject))
            {
                if (helperObject.Position == Tutorialposition)
                {
                    targetUIElement = helperObject.GetComponent<RectTransform>();
                    found = true;
                    break;
                }
            }
        }

        // If a matching UI element is found, highlight it and show the corresponding tutorial panel.
        if (found)
        {
            if (Tutorialposition - 1 != -1)
            {
                MainPanel.transform.GetChild(Tutorialposition - 1).gameObject.SetActive(false);
            }

            MainPanel.transform.GetChild(Tutorialposition).gameObject.SetActive(true);
            HighlightElement(targetUIElement);
        }

        // If the tutorial position reaches 30, hide the main panel.
        if (Tutorialposition == 30)
        {
            MainPanel.SetActive(false);
        }
    }

    public void HighlightElement(RectTransform target)
    {
        if (target == null || highlightMaterial == null) return;

        // 1. Get the 4 corners of the UI element in world space
        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        // 2. Convert world corners to screen pixel coordinates
        Vector2 screenMin = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 screenMax = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        // Add padding in pixels
        screenMin -= new Vector2(padding, padding);
        screenMax += new Vector2(padding, padding);

        // 3. Calculate center position and normalize it (0 to 1)
        Vector2 centerScreen = (screenMin + screenMax) * 0.5f;
        Vector4 normalizedCenter = new Vector4(centerScreen.x / Screen.width, centerScreen.y / Screen.height, 0, 0);

        // 4. Calculate dimensions and normalize them (0 to 1)
        float normalizedWidth = (screenMax.x - screenMin.x) / Screen.width;
        float normalizedHeight = (screenMax.y - screenMin.y) / Screen.height;
        Vector4 normalizedSize = new Vector4(normalizedWidth, normalizedHeight, 0, 0);

        // 5. Send data to the shader
        highlightMaterial.SetVector("_CutoutPos", normalizedCenter);
        highlightMaterial.SetVector("_Size", normalizedSize);
    }
}
