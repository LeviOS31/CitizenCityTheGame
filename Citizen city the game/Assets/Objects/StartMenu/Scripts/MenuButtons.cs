using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject SettingsMenu;
    public AudioMixer Mixer;

    private void Start()
    {
        float masterdB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("MasterVolume", 0.5f), 0.0001f)) * 20;
        float musicdB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("MusicVolume", 0.5f), 0.0001f)) * 20;
        float backgrounddB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("BackgroundVolume", 0.5f), 0.0001f)) * 20;
        float effectdB = Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("EffectsVolume", 0.5f), 0.0001f)) * 20;


        Mixer.SetFloat("MasterVolume", masterdB);
        Mixer.SetFloat("MusicVolume", musicdB);
        Mixer.SetFloat("BackgroundVolume", backgrounddB);
        Mixer.SetFloat("EffectsVolume", effectdB);
    }


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
