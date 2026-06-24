using UnityEngine;


// This class is responsible for bridging the gap between animation events and sound effects.
// attach this script to a gameobject that also plays an animation, and use the TriggerSoundEffect method to play a sound effect when an animation event is triggered.
public class AnimationSoundBridge : MonoBehaviour
{
    public void TriggerSoundEffect(string soundName)
    {
        AudioSignalHandler.PlaySound.Invoke(soundName);
    }
}
