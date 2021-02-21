using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    public LobbyManager lobbyManager;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player is on the button");
        lobbyManager.SetReady(true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player is not on the button");
        lobbyManager.SetReady(false);
    }
}
