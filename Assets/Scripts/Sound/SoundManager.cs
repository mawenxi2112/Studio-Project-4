using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum SoundAudio
    {

    }

    public static void PlaySound(SOUNDTYPE soundType)
    {
        if (GameSettings.instance.CanPlayAudio(soundType))
        {
            // Play sound
        }
    }
}
