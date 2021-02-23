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
        PlayerPrefs.SetInt("PMaxHealth", maxHealth);
        PlayerPrefs.SetInt("PMaxSpeed", speed);
        PlayerPrefs.SetInt("PDamage", damage);
        PlayerPrefs.SetInt("PCoins", coins);
        PlayerPrefs.Save();
    }
}