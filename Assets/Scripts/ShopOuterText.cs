using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopOuterText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text m_TextComponent;
    public int objectType;
    public ShopData shopData;
    void Start()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    
    }

    // Update is called once per frame
    void Update()
    {
        int value;
        int multiplyer =0;
        int offset=0;
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
        value = shopData.playerUpgrades[objectType] * shopData.playerUpgrades[objectType] * multiplyer + offset;
        m_TextComponent.text = value.ToString();
    }
}
