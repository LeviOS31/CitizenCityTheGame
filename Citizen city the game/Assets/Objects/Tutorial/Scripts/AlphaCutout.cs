using UnityEngine;
using UnityEngine.UI;

public class AlphaCutout : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("Any pixel with an alpha below this value will be clickable-through.")]
    public float alphaThreshold = 0.1f;

    void Start()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.alphaHitTestMinimumThreshold = alphaThreshold;
        }
    }

    public void click()
    {
        Debug.Log("clicked " + name);
    }
}
