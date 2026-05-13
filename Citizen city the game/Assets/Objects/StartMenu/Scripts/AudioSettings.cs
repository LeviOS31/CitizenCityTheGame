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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MasterAudio.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        MusicAudio.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        BackgroundAudio.value = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        EffectAudio.value = PlayerPrefs.GetFloat("EffectsVolume", 1f);
    }

    public void change1()
    {
        float masterdB = Mathf.Log10(Mathf.Max(MasterAudio.value, 0.0001f)) * 20;

        PlayerPrefs.SetFloat("MasterVolume", MasterAudio.value);

        Mixer.SetFloat("MasterVolume", masterdB);

    }

    public void change2()
    {
        float musicdB = Mathf.Log10(Mathf.Max(MusicAudio.value, 0.0001f)) * 20;

        PlayerPrefs.SetFloat("MusicVolume", MusicAudio.value);

        Mixer.SetFloat("MusicVolume", musicdB);
    }

    public void change3()
    {
        float backgrounddB = Mathf.Log10(Mathf.Max(BackgroundAudio.value, 0.0001f)) * 20;

        PlayerPrefs.SetFloat("BackgroundVolume", BackgroundAudio.value);

        Mixer.SetFloat("BackgroundVolume", backgrounddB);
    }

    public void change4()
    {
        float effectdB = Mathf.Log10(Mathf.Max(EffectAudio.value, 0.0001f)) * 20;

        PlayerPrefs.SetFloat("EffectsVolume", EffectAudio.value);

        Mixer.SetFloat("EffectsVolume", effectdB);
    }
}
