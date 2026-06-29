using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// This class manages the audio settings in the game, allowing users to adjust master, music, background, and effect volumes through UI sliders.
// It saves user preferences using PlayerPrefs and applies them to the audio mixer.
public class AudioSettings : MonoBehaviour
{
    public Slider MasterAudio;
    public Slider MusicAudio;
    public Slider BackgroundAudio;
    public Slider EffectAudio;

    public AudioMixer Mixer;

    bool start = false;

    // This method initializes the audio sliders with values stored in PlayerPrefs when the game starts.
    // It sets the sliders to the saved volume levels for master, music, background, and effects.
    void Start()
    {
        MasterAudio.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        MusicAudio.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        BackgroundAudio.value = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        start = true;
        EffectAudio.value = PlayerPrefs.GetFloat("EffectsVolume", 1f);
    }

    // This method is called when the user changes any of the audio sliders.
    // It updates the corresponding volume levels in the audio mixer and saves the new values to PlayerPrefs.
    public void change()
    {
        if (!start) return;

        float masterdB = Mathf.Log10(Mathf.Max(MasterAudio.value, 0.0001f)) * 20;
        float musicdB = Mathf.Log10(Mathf.Max(MusicAudio.value, 0.0001f)) * 20;
        float backgrounddB = Mathf.Log10(Mathf.Max(BackgroundAudio.value, 0.0001f)) * 20;
        float effectdB = Mathf.Log10(Mathf.Max(EffectAudio.value, 0.0001f)) * 20;

        PlayerPrefs.SetFloat("MasterVolume", MasterAudio.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicAudio.value);
        PlayerPrefs.SetFloat("BackgroundVolume", BackgroundAudio.value);
        PlayerPrefs.SetFloat("EffectsVolume", EffectAudio.value);

        Mixer.SetFloat("MasterVolume", masterdB);
        Mixer.SetFloat("MusicVolume", musicdB);
        Mixer.SetFloat("BackgroundVolume", backgrounddB);
        Mixer.SetFloat("EffectsVolume", effectdB);
    }
    
    // This method plays the background audio for a short duration to test the current volume settings.
    public void TestBG()
    {
        FindObjectsByType<AudioController>(FindObjectsSortMode.None)[0].PlayBGForSecond();
    }

    // This method plays the effect audio for a short duration to test the current volume settings.
    public void TestEffect()
    {
        FindObjectsByType<AudioController>(FindObjectsSortMode.None)[0].PlayEffectForSecond();
    }
}
