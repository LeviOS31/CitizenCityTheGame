using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AudioController : MonoBehaviour
{
    public AudioSource BackgroundAudio;
    public GameObject EffectAudio;
    public bool playBGAudioOnStart;

    List<AudioClip> BackgroundAudioClips;
    List<AudioClip> EffectAudioClips;

    private void Start()
    {
        BackgroundAudioClips = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sound/Background"));
        EffectAudioClips = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sound/Effects"));

        if (playBGAudioOnStart)
        {
            PlayBackground();
        }

        AudioSignalHandler.PlaySound += PlayEffect;
    }

    private void OnDisable()
    {
        AudioSignalHandler.PlaySound -= PlayEffect;
    }

    private void OnDestroy()
    {
        AudioSignalHandler.PlaySound -= PlayEffect;
    }

    public void PlayEffect(string audioname)
    {
        Debug.Log(audioname);

        foreach (AudioClip clip in EffectAudioClips)
        {
            if (clip.name == audioname)
            {
                GameObject instance = Instantiate(EffectAudio, transform);
                EffectAudio audio = instance.GetComponent<EffectAudio>();
                audio.Playaudio(clip);
                return;
            }
        }
    }

    public async Task PlayBackground()
    {
        int random = Random.Range(0, BackgroundAudioClips.Count - 1);
        AudioClip clip = BackgroundAudioClips[random];
        BackgroundAudio.clip = clip;
        BackgroundAudio.Play();

        int durationMs = Mathf.CeilToInt(clip.length * 1000);
        await Task.Delay(durationMs);

        if (this == null) return;

        PlayBackground();
    }

    public async Task PlayBGForSecond()
    {
        if (BackgroundAudio.isPlaying)
        {
            return;
        }

        int random = Random.Range(0, BackgroundAudioClips.Count - 1);
        AudioClip clip = BackgroundAudioClips[random];
        BackgroundAudio.clip = clip;
        BackgroundAudio.Play();
        await Task.Delay(1000);

        if (this == null) return;

        BackgroundAudio.Stop();   
    }

    public async Task PlayEffectForSecond()
    {
        int random = Random.Range(0, EffectAudioClips.Count - 1);
        AudioClip clip = EffectAudioClips[random];

        GameObject instance = Instantiate(EffectAudio, transform);
        EffectAudio audio = instance.GetComponent<EffectAudio>();
        audio.Playaudio(clip);
    }

}
