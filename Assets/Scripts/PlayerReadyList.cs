using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class PlayerReadyList : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text m_textComponent;
    public int textType;
    void Start()
    {
        m_textComponent = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textType == PhotonNetwork.PlayerList.Length)
        {
            m_textComponent.text = "";
            return;
        }

        Player player = PhotonNetwork.PlayerList[textType];
        m_textComponent.text = "P" + textType+1 + ": "+player.NickName;
        object bValue;
        if (player.CustomProperties.TryGetValue(GameData.PLAYER_READY,out bValue))
        {
            if((bool)bValue)
            {
                m_textComponent.color = new Color32(94, 241, 138, 255);
            }
            else
            {
                m_textComponent.color = new Color32(255, 53, 62, 255);
            }
        }
        else
        {
            m_textComponent.text = "Error Value not Set";
        }
        
    }
}
