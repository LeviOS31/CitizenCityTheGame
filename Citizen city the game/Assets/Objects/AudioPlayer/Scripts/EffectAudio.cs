using System.Threading.Tasks;
using UnityEngine;

public class EffectAudio : MonoBehaviour
{
    public AudioSource Player;

    public async Task Playaudio(AudioClip audio)
    {
        Player.clip = audio;
        Player.Play();

        int durationMs = Mathf.CeilToInt(audio.length * 1000 + 1000);
        await Task.Delay(durationMs);

        if (this == null) return;

        Destroy(gameObject);
    }
}
