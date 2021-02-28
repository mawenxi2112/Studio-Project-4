using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopItemText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text m_TextComponent;
    public int textType;
    public int objectType;
    public ShopData shopData;
    public PlayerData player;
    //0: Cost
    //1: Wallet
    //2: Purchase Name
    //3: Atk Stat
    //4: Health Stat
    //5: MovementSpeed
    void Start()
    {
        
        m_TextComponent = GetComponent<TMP_Text>();
        int value;
        int offset=0;
        int multiplyer =0;
        switch(objectType)
        {
            case 0:
                multiplyer = 5;
                offset = 10;

                break;
            case 1:
                multiplyer = 12;
                offset = 5;
                break;
            case 2:
                multiplyer = 20;
                offset = 20;
                break;
        }
        switch(textType)
        {
       
            case 0:
                
                value = shopData.playerUpgrades[objectType] * shopData.playerUpgrades[objectType] * multiplyer + offset;
                m_TextComponent.text = value.ToString();
                break;
           
            case 1:
                value = player.m_currency;
                m_TextComponent.text = value.ToString();
                break;
            case 2:
                if(objectType == 0)
                {
                    m_TextComponent.text = "Attack +1";
                }

                else if (objectType == 1)
                {
                    m_TextComponent.text = "Health +1";
                }

                else if (objectType == 2)
                {
                    m_TextComponent.text = "Speed +5";
                }
                break;
            case 3:
                m_TextComponent.text = player.m_currentAttack.ToString();
                break;
            case 4:
                m_TextComponent.text = player.m_maxHealth.ToString();
                break;
            case 5:
                m_TextComponent.text = player.m_maxMoveSpeed.ToString();
                break;

        }
     
    }
    void OnEnable()
    {
        Restart();
    }
  
    // Update is called once per frame
    public void Update()
    {
       
         m_TextComponent = GetComponent<TMP_Text>();
        int value;
        int offset = 0;
        int multiplyer = 0;
        switch (objectType)
        {
            case 0:
                multiplyer = 5;
                offset = 10;

                break;
            case 1:
                multiplyer = 12;
                offset = 5;
                break;
            case 2:
                multiplyer = 20;
                offset = 20;
                break;
        }
        switch (textType)
        {

            case 0:

                value = shopData.playerUpgrades[objectType] * shopData.playerUpgrades[objectType] * multiplyer + offset;
                //Debug.Log("Value: " + value);
                m_TextComponent.text = value.ToString();
                break;

            case 1:
                value = player.m_currency;
                m_TextComponent.text = value.ToString();
                break;
            case 2:
                if (objectType == 0)
                {
                    m_TextComponent.text = "Attack +1";
                }

                else if (objectType == 1)
                {
                    m_TextComponent.text = "Health +1";
                }

                else if (objectType == 2)
                {
                    m_TextComponent.text = "Speed +5";
                }
                break;
            case 3:
                if(objectType == 1)
                {
                    //Debug.Log("PlayerAttack: " +player.m_currentAttack.ToString());
                }
                else if(objectType == 2)
                {
                    //Debug.Log("PlayerAttackSpeed: " + player.m_currentAttack.ToString());
                }
                m_TextComponent.text = player.m_currentAttack.ToString();

                break;
            case 4:
                m_TextComponent.text = player.m_maxHealth.ToString();
                if (player.m_maxHealth >= 10)
                {
                    m_TextComponent.text = "Maxed";
                    player.m_maxHealth = 10;
                }
                break;
            case 5:
                m_TextComponent.text = player.m_maxMoveSpeed.ToString();
                break;

        }
    }
    public void Restart()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        int value;
        int offset = 0;
        int multiplyer = 0;
        switch (objectType)
        {
            case 0:
                multiplyer = 5;
                offset = 10;

                break;
            case 1:
                multiplyer = 12;
                offset = 5;
                break;
            case 2:
                multiplyer = 20;
                offset = 20;
                break;
        }
        switch (textType)
        {

            case 0:

                value = shopData.playerUpgrades[objectType] * shopData.playerUpgrades[objectType] * multiplyer + offset;
                //Debug.Log("Value: " + value);
                m_TextComponent.text = value.ToString();
                break;

            case 1:
                value = player.m_currency;
                m_TextComponent.text = value.ToString();
                break;
            case 2:
                if (objectType == 0)
                {
                    m_TextComponent.text = "Attack +1";
                }

                else if (objectType == 1)
                {
                    m_TextComponent.text = "Health +1";
                }

                else if (objectType == 2)
                {
                    m_TextComponent.text = "Speed +5";
                }
                break;
            case 3:
                if(objectType == 1)
                {
                    //Debug.Log("PlayerAttack: " +player.m_currentAttack.ToString());
                }
                else if(objectType == 2)
                {
                    //Debug.Log("PlayerAttackSpeed: " + player.m_currentAttack.ToString());
                }
                m_TextComponent.text = player.m_currentAttack.ToString();

                break;
            case 4:
                m_TextComponent.text = player.m_maxHealth.ToString();
                break;
            case 5:
                m_TextComponent.text = player.m_maxMoveSpeed.ToString();
                break;

        }
    }
}
