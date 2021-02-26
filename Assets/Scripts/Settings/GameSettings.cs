﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance = null;

    private string settingsFilepath;

    // Resolution related Settings
    public bool fullscreen = true;

    // Volume Related Settings
    public bool MasterVolumeEnabled = true;
    public int MasterVolume = 100;

    public bool BGMEnabled = true;
    public int BGMVolume = 100;

    public bool SoundFXEnabled = true;
    public int SoundFXVolume = 100;

    public enum SOUNDTYPE
    {
        BGM,
        FX
    }

    void Awake()
    {
        instance = GetInstance();

        settingsFilepath = Application.persistentDataPath + "/settings.json";

        string settingsJson = null;

        if (File.Exists(settingsFilepath))
        {
            settingsJson = File.ReadAllText(settingsFilepath);
        }

        if (settingsJson == null)
        {
            Debug.Log("File does not exist. Generating new file with default values.");

            UpdateTextFile();
        }
        else
        {
            GameSettings newSettings = JsonConvert.DeserializeObject<GameSettings>(settingsJson);

            fullscreen = newSettings.fullscreen;

            MasterVolumeEnabled = newSettings.MasterVolumeEnabled;
            MasterVolume = newSettings.MasterVolume;

            BGMEnabled = newSettings.BGMEnabled;
            BGMVolume = newSettings.BGMVolume;

            SoundFXEnabled = newSettings.SoundFXEnabled;
            SoundFXVolume = newSettings.SoundFXVolume;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            MasterVolume++;

        Debug.Log("Master Volume: " + MasterVolume);
        Debug.Log("BGM Volume: " + BGMVolume);
        Debug.Log("Sound FX Volume: " + SoundFXVolume);
    }

    public void UpdateTextFile()
    {
        string jsonText = JsonUtility.ToJson(this);

        if (!File.Exists(settingsFilepath))
            File.Create(settingsFilepath).Close();

        File.WriteAllText(settingsFilepath, jsonText);
    }

    public void CanPlayAudio()
    {

    }

    public static GameSettings GetInstance()
    {
        if (instance == null)
            instance = new GameSettings();


        return instance;
    }

    void OnApplicationQuit()
    {
        UpdateTextFile();
    }
}