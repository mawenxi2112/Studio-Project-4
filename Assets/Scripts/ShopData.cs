using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    // Start is called before the first frame update
    public enum UPGRADETYPES
    {
        DAMAGE,
        HEALTH,
        SPEED,
        TOTAL
    }
    public int[] playerUpgrades = new int[(int)UPGRADETYPES.TOTAL];
    void Start()
    {
        SceneData.playerCurrency = 105;
    }
    void OnDisable()
    {
        for(int i=(int)UPGRADETYPES.DAMAGE;i<(int)UPGRADETYPES.TOTAL;i++)
        {
            //Makeshift array storing
            PlayerPrefs.SetInt("value" + i.ToString(), playerUpgrades[i]);
        }
    }

    void OnEnable()
    {
       
        for (int i = (int)UPGRADETYPES.DAMAGE; i < (int)UPGRADETYPES.TOTAL; i++)
        {
            //Makeshift array storing
            playerUpgrades[i] = PlayerPrefs.GetInt("value" + i.ToString());
            
        }
        /*        value = PlayerPrefs.GetInt("value");*/
    }

    // Update is called once per frame

}
