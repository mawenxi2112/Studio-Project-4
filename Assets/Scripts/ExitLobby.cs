using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLobby : MonoBehaviour
{
    // Start is called before the first frame update
    public LobbyManager lobbyManager;
    void OnTriggerEnter2D()
    {
        lobbyManager.PlayerLeaveRoom();
    }
}
