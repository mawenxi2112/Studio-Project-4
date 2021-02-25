using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class PlayerStorage
{
    public static int maxHealth = 10;

    public static int speed = 350;

    public static int damage = 1;

    public static int coins = 10000;

    public static void Load()
    {
        maxHealth = PlayerPrefs.GetInt("PMaxHealth", 10);
        speed = PlayerPrefs.GetInt("PMaxSpeed", 350);
        damage = PlayerPrefs.GetInt("PDamage", 1);
        coins = PlayerPrefs.GetInt("PCoins", 10000);
    }

    public static void Save()
    {

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            PlayerPrefs.SetInt("PMaxHealth", GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().m_maxHealth);
            PlayerPrefs.SetInt("PMaxSpeed", (int)GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().m_maxMoveSpeed);
            PlayerPrefs.SetInt("PDamage", GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().m_currentAttack);
            PlayerPrefs.SetInt("PCoins", GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().m_currency);
            PlayerPrefs.Save();
        }
    }

    public static void Save(int maxHealthV, int moveSpeedV, int damageV, int coinsV)
    {
        PlayerPrefs.SetInt("PMaxHealth", maxHealthV);
        PlayerPrefs.SetInt("PMaxSpeed", moveSpeedV);
        PlayerPrefs.SetInt("PDamage", damageV);
        PlayerPrefs.SetInt("PCoins", coinsV);
        PlayerPrefs.Save();
    }
}