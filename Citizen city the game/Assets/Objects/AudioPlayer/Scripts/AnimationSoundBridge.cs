using UnityEngine;

public class AnimationSoundBridge : MonoBehaviour
{
    public void TriggerSoundEffect(string soundName)
    {
        AudioSignalHandler.PlaySound.Invoke(soundName);
    }
}
