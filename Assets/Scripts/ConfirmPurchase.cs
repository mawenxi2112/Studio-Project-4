using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPurchase : MonoBehaviour
{
    // Start is called before the first frame update
    public int objectType;
    public PlayerData playerData;
    public ShopItemText statIncrease;
    public ShopItemText cost;
    public ShopItemText wallet;
    public ShopData shopData;
    public void Confirm()
    {
     
        int iCost = int.Parse(cost.m_TextComponent.text);
        int iWallet = int.Parse(wallet.m_TextComponent.text);
        if (playerData.m_maxHealth >= 10 && objectType == 1)
            return;
        if(iCost > iWallet)
        {
            return;
        }
        iWallet = iWallet - iCost;
        wallet.m_TextComponent.text = iWallet.ToString();
        shopData.playerUpgrades[objectType]++;
        int iPlayerStat = 0;


        int iIncrease = 0;
        switch (objectType)
        {
            case 0:
                iPlayerStat = playerData.m_currentAttack;
                iIncrease = 1;
                break;
            case 1:
                iPlayerStat = playerData.m_maxHealth;
                iIncrease = 1;
                break;
            case 2:
                iPlayerStat = (int)playerData.m_maxMoveSpeed;
                iIncrease = 5;
                break;
        }
      
        iPlayerStat = iIncrease + iPlayerStat;

        playerData.m_currency = iWallet;
     
        switch (objectType)
        {
            case 0:
                playerData.m_currentAttack = iPlayerStat;
        
                break;
            case 1:
                playerData.m_maxHealth = iPlayerStat;
               
                break;
            case 2:
                playerData.m_maxMoveSpeed = iPlayerStat;
              
                break;
        }



    }
}
