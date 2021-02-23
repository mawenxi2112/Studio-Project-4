using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ReadyButton : MonoBehaviour
{
    public LobbyManager lobbyManager;
    // Start is called before the first frame update
    public PhotonView photonView;
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // There's an error currently so I'm going to change it temporary.
        // - Wen Xi

/*        bool isMine;
        Player otherPlayer = other.gameObject.GetComponent<PhotonView>().Controller;
        Debug.Log("Player: " + otherPlayer.NickName + "Is on the block");
        Debug.Log("This Player is Running: " + PhotonNetwork.LocalPlayer.NickName);
        isMine = PhotonNetwork.Equals(PhotonNetwork.LocalPlayer, otherPlayer);*/
        
        // NEED TO ADD A CHECK WHEN PLAYER LEAVES THE LOBBY

        if (other.gameObject.GetComponent<PhotonView>().IsMine)
            lobbyManager.SetReady(true, true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player is not on the button");
        bool isMine;

        Player otherPlayer = other.gameObject.GetComponent<PhotonView>().Controller;
        Debug.Log("Player: " + otherPlayer.NickName + "Is on the block");
        Debug.Log("This Player is Running: " + PhotonNetwork.LocalPlayer.NickName);
        isMine = PhotonNetwork.Equals(PhotonNetwork.LocalPlayer, otherPlayer);
        lobbyManager.SetReady(false,isMine);
    }
}
