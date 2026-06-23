using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AlphaCutout : MonoBehaviour, ICanvasRaycastFilter
{
    public Material highlightMaterial;
    public int[] numbers = { 1, 2, 3, 5, 6, 10, 13, 15, 20, 21, 24, 28, 29};

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        // Skips the raycast check if the current tutorial position is in the numbers array
        if (numbers.Contains(Tutorial.Tutorialposition))
        {
            return true;
        }

        // If we don't have the material, block everything by default
        if (highlightMaterial == null) return true;

        // Get the cutout center and size from the shader
        Vector4 cutoutPos = highlightMaterial.GetVector("_CutoutPos");
        Vector4 cutoutSize = highlightMaterial.GetVector("_Size");

        // Convert the incoming click position (pixels) to normalized screen coordinates (0 to 1)
        float clickX = sp.x / Screen.width;
        float clickY = sp.y / Screen.height;

        // Calculate the boundaries of the rectangular cutout
        float minX = cutoutPos.x - (cutoutSize.x * 0.5f);
        float maxX = cutoutPos.x + (cutoutSize.x * 0.5f);
        float minY = cutoutPos.y - (cutoutSize.y * 0.5f);
        float maxY = cutoutPos.y + (cutoutSize.y * 0.5f);

        // Check if the click happened INSIDE the cutout area
        if (clickX >= minX && clickX <= maxX && clickY >= minY && clickY <= maxY)
        {
            //Return false so the raycast passes through
            return false;
        }

        // The click is on the dark overlay, block it
        return true;
    }
}
