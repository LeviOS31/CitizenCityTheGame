using UnityEngine;

public class GameFieldUI : MonoBehaviour
{
    [SerializeField] GameObject field;
    public void Enable()
    {
        field.SetActive(true);
    }

    public void Disable()
    {
        field.SetActive(false);
    }
}
