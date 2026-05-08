using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject SettingsMenu;
    public void ResumeGame()
    {
        Debug.Log("Resume previous game");
        //TODO: add resume feature after save feature gets added
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("CardGamePrototype");
    }

    public void Settigns()
    {
        Debug.Log("Open settings");
        SettingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
