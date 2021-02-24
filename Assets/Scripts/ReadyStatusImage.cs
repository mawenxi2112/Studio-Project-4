using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ReadyStatusImage : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] playerStatus;
    public Sprite readyTick;
    public Sprite notReadyCross;

    // Update is called once per frame
    void Update()
    {

        if (PhotonNetwork.PlayerList.Length == 0)
            return;

        for(int playerType = 0;playerType < playerStatus.Length;playerType++)
        {
            if (PhotonNetwork.PlayerList.Length == playerType)
            {
                playerStatus[playerType].enabled = false;
                break;
            }
            Player player = PhotonNetwork.PlayerList[playerType];
            playerStatus[playerType].enabled = true;
            object bValue;
            if (player.CustomProperties.TryGetValue(GameData.PLAYER_READY, out bValue))
            {
                if ((bool)bValue)
                {
                    playerStatus[playerType].sprite = readyTick;
                }
                else
                {
                    playerStatus[playerType].sprite = notReadyCross;
                }
            }

        }
    }
}
