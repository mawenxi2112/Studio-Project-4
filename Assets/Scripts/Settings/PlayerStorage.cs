using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerStorage
{
    public static PlayerStorage instance = null;

    private string savedFilepath;

    public int maxHealth = 10;

    public int speed = 350;

    public int damage = 1;

    public int coins = 0;

    private PlayerStorage()
    {
        Init();
    }

    public static PlayerStorage GetInstance()
    {
        if (instance == null)
            instance = new PlayerStorage();

        return instance;
    }

    private void Init()
    {
        savedFilepath = Application.persistentDataPath + "/pdata.json";
    }

    void LoadData()
    {
        string playerData = null;

        if (!File.Exists(savedFilepath))
        {
            File.Create(savedFilepath).Close();
        }
        else
        {
            playerData = File.ReadAllText(savedFilepath);

            PlayerStorage newPlayerData = JsonConvert.DeserializeObject<PlayerStorage>(playerData);

            this.maxHealth = newPlayerData.maxHealth;
            this.speed = newPlayerData.speed;
            this.damage = newPlayerData.damage;
            this.coins = newPlayerData.coins;
        }
    }

    void SaveData()
    {
        string jsonText = JsonUtility.ToJson(this);

        if (!File.Exists(savedFilepath))
            File.Create(savedFilepath).Close();

        File.WriteAllText(savedFilepath, jsonText);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationExit()
    {
        SaveData();
    }
}
