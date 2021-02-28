using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUNDTYPE
{
    BGM,
    FX
}

public static class SoundManager
{
    public enum SoundName
    {
        DAMAGE,
        DOOR,
        SWORD,
        DASH,
        BUTTON,
        COIN,
        HEALTH,
        STRENGTH,
        MELEE,
        BOW,
        TELEPORT,
        RUNNER,
        MENUCLICK,
        MENUBACK,
        STAIRSBACK,
        FOOTSTEPS
    }

    private static Dictionary<SoundName, float> soundTimerDictionary = null;

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void Initialise()
    {
        if (soundTimerDictionary != null)
            return;


        soundTimerDictionary = new Dictionary<SoundName, float>();
        soundTimerDictionary[SoundName.DAMAGE] = 0.0f;
    }

    public static void PlaySound(SOUNDTYPE soundType, SoundName name)
    {
        if (CanPlayAudio(soundType) && CanPlaySound(name))
        {
            AudioClip audioClip = GetAudioClip(name);

            if (audioClip == null)
                return;

            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
           
            float volume;

            if (soundType == SOUNDTYPE.BGM)
                volume = (float)SceneData.BGMVolume / 100.0f;
            else
                volume = (float)SceneData.SoundFXVolume / 100.0f;

            oneShotAudioSource.volume = volume;

            oneShotAudioSource.PlayOneShot(audioClip);
      
        }
    }

    public static GameObject PlaySoundMenu(SOUNDTYPE soundType, SoundName name)
    {
        if (CanPlayAudio(soundType) && CanPlaySound(name))
        {
            AudioClip audioClip = GetAudioClipMenu(name);

            if (audioClip == null)
                return null;

            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            float volume;

            if (soundType == SOUNDTYPE.BGM)
                volume = (float)SceneData.BGMVolume / 100.0f;
            else
                volume = (float)SceneData.SoundFXVolume / 100.0f;

            oneShotAudioSource.volume = volume;

            oneShotAudioSource.PlayOneShot(audioClip);
            return oneShotGameObject;
        }
        return null;
    }

    private static bool CanPlaySound(SoundName name)
    {
        switch(name)
        {
            default:
                return true;
            case SoundName.DAMAGE:
                {
                    if (soundTimerDictionary.ContainsKey(name))
                    {
                        float lastTimePlayed = soundTimerDictionary[name];
                        float playerDamageTimerMax = 2.5f;

                        if (lastTimePlayed + playerDamageTimerMax < Time.time)
                        {
                            soundTimerDictionary[name] = Time.time;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
        }
    }

    public static AudioClip GetAudioClip(SoundName name)
    {
        if (GameSceneManager.Instance == null)
            return null;

        foreach (GameSceneManager.SoundAudioClip soundAudioClip in GameSceneManager.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.name == name)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + name + " not found!");

        return null;
    }
    public static AudioClip GetAudioClipMenu(SoundName name)
    {
        if (SceneSoundManager.Instance == null)
            return null;

        foreach (SceneSoundManager.SoundAudioClip soundAudioClip in SceneSoundManager.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.name == name)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + name + " not found!");

        return null;
    }

    public static bool CanPlayAudio(SOUNDTYPE soundType)
    {
        Debug.Log(SceneData.MasterVolume);
        if (SceneData.MasterVolume == 0)
            return false;

        if (soundType == SOUNDTYPE.BGM && SceneData.BGMVolume > 0)
        {
            return true;
        }
        else if (soundType == SOUNDTYPE.FX && SceneData.SoundFXVolume > 0)
        {
            return true;
        }

        return false;
    }
}