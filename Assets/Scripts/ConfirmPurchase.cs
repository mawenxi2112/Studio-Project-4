using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPurchase : MonoBehaviour
{
    // Start is called before the first frame update
    public int objectType;
    public ShopItemText playerStat;
    public PlayerData playerData;
    public ShopItemText statIncrease;
    public ShopItemText cost;
    public ShopItemText wallet;
    public ShopData shopData;
    public void Confirm()
    {
        Debug.Log("Confirmed");
        int iCost = int.Parse(cost.m_TextComponent.text);
        int iWallet = int.Parse(wallet.m_TextComponent.text);
/*        Debug.Log("Cost: " + iCost);
        Debug.Log("Wallet: " + iWallet);*/
        if(iCost > iWallet)
        {
            Debug.Log("Not Enough Money");
            return;
        }
        iWallet = iWallet - iCost;
        wallet.m_TextComponent.text = iWallet.ToString();
        shopData.playerUpgrades[objectType]++;
        int iPlayerStat = int.Parse(playerStat.m_TextComponent.text);
        Debug.Log("Testing: " + int.Parse(playerStat.m_TextComponent.text));
        int iIncrease = 0;
        switch (objectType)
        {
            case 0:
                iIncrease = 3;
                break;
            case 1:
                iIncrease = 2;
                break;
            case 2:
                iIncrease = 5;
                break;
        }
      
        iPlayerStat = iIncrease + iPlayerStat;
        
        Debug.Log("iIncrease: " + iIncrease);

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
        if(objectType == 2)
        {
            Debug.Log(playerStat.m_TextComponent.text);
        }
        playerStat.Restart();
        cost.Restart();
        wallet.Restart();


    }
}
