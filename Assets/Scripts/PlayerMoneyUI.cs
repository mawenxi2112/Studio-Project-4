using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerMoneyUI : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerData player;
    public TMP_Text m_textComponent;
    void Start()
    {
        m_textComponent = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        m_textComponent.text = player.m_currency.ToString();
    }
}
