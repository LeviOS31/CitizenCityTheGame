using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider MasterAudio;
    public Slider MusicAudio;
    public Slider BackgroundAudio;
    public Slider EffectAudio;

    public AudioMixer Mixer;

    bool start = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MasterAudio.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        MusicAudio.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        BackgroundAudio.value = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        start = true;
        EffectAudio.value = PlayerPrefs.GetFloat("EffectsVolume", 1f);
    }

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

    public void TestBG()
    {
        FindObjectsByType<AudioController>(FindObjectsSortMode.None)[0].PlayBGForSecond();
    }

    public void TestEffect()
    {
        FindObjectsByType<AudioController>(FindObjectsSortMode.None)[0].PlayEffectForSecond();
    }
}
