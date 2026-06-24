using System.Threading.Tasks;
using UnityEngine;

// This class is responsible for playing sound effects in the game.
// It uses an AudioSource component to play the audio clip and destroys the game object after the audio has finished playing.
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
