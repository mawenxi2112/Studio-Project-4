using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneData : MonoBehaviour
{
    // Start is called before the first frame update
    public static string previousScene;
    public static string currentScene;
    public static int playerCurrency;

    private GameSettings gameSettings;

    public static bool MasterVolumeEnabled = true;
    public static int MasterVolume = 100;

    public static bool BGMEnabled = true;
    public static int BGMVolume = 100;

    public static bool SoundFXEnabled = true;
    public static int SoundFXVolume = 100;

    void Start()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("DataManager");

        for (int i = 0; i != array.Length; i++)
        {
            if (i == (array.Length - 1))
            {
                if (currentScene == null)
                {
                    currentScene = SceneManager.GetActiveScene().name;
                }

                break;

            }

            Destroy(array[i]);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}