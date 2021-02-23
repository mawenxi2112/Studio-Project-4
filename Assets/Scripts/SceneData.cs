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

    //private static string savedFilepath;

    //public static PlayerStorage storage;

    void Start()
    {
/*        savedFilepath = Application.persistentDataPath + "/pdata.json";

        Debug.Log("is hererehagdjha");

        if (storage == null)
            storage = new PlayerStorage();

        LoadData();*/

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

/*    void LoadData()
    {
        string playerData = null;

        if (!File.Exists(savedFilepath))
        {
            SaveData();
        }
        else
        {
            playerData = File.ReadAllText(savedFilepath);

            storage = JsonConvert.DeserializeObject<PlayerStorage>(playerData);
        }
    }

    static public void SaveData()
    {
        Debug.Log("My stuff here: " + storage.maxHealth);
        Debug.Log(storage.coins);
        Debug.Log(storage.damage);
        Debug.Log(storage.speed);

        string jsonText = JsonUtility.ToJson(storage);

        if (!File.Exists(savedFilepath))
            File.Create(savedFilepath).Close();

        File.WriteAllText(savedFilepath, jsonText);
    }

    void OnApplicationQuit()
    {
        SaveData();
    }*/
}