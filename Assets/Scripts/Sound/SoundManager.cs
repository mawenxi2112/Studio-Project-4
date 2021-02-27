﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum SoundName
    {
        DAMAGE,
        DOOR,
        SWORD
    }

    private static Dictionary<SoundName, float> soundTimerDictionary;

    public static void Initialise()
    {
        soundTimerDictionary = new Dictionary<SoundName, float>();
        soundTimerDictionary[SoundName.DAMAGE] = 0.0f;
    }

    public static void PlaySound(SOUNDTYPE soundType, SoundName name)
    {
        if (GameSettings.GetInstance().CanPlayAudio(soundType) && CanPlaySound(name))
        {
            // Play sound
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            if (soundType == SOUNDTYPE.BGM)
                audioSource.volume = GameSettings.GetInstance().BGMVolume;
            else
                audioSource.volume = GameSettings.GetInstance().SoundFXVolume;

            audioSource.PlayOneShot(GetAudioClip(name));
        }
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
}