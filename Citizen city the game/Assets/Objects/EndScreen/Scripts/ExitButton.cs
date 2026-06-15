using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
