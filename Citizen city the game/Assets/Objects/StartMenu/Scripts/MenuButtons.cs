using UnityEngine;
using UnityEngine.SceneManagement;

// This class is responsible for handling the main menu buttons and their associated actions such as starting a new game, resuming a game, opening settings, and quitting the application.
// It also manages the visibility of the settings menu and player selector UI elements.
public class MenuButtons : MonoBehaviour
{
    public GameObject SettingsMenu;
    public bool isingamesettings = false;
    public GameObject playerSelector;
    public GameObject MainMenuButtons;
    private void Update()
    {
        if (!isingamesettings) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (transform.GetChild(0).gameObject.activeSelf)
            {
                Continue();
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void Continue()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        Debug.Log("Resume previous game");
        //TODO: add resume feature after save feature gets added
    }

    public void StartNewGame()
    {
        playerSelector.SetActive(true);
        MainMenuButtons.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("CardGamePrototype");
    }

    public void SaveGame()
    {
        Debug.Log("Save current game");
        //TODO: add save feature and UI
    }

    public void LoadGame()
    {
        Debug.Log("Load a save game");
        //TODO: add load feature and UI
    }

    public void Settigns()
    {
        Debug.Log("Open settings");
        SettingsMenu.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
