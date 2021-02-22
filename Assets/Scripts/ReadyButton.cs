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


        bool isMine;

        Player otherPlayer = other.gameObject.GetComponent<PhotonView>().Controller;
        Debug.Log("Player: " + otherPlayer.NickName + "Is on the block");
        Debug.Log("This Player is Running: " + PhotonNetwork.LocalPlayer.NickName);
        isMine = PhotonNetwork.Equals(PhotonNetwork.LocalPlayer, otherPlayer);

        lobbyManager.SetReady(true,isMine);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player is not on the button");
        bool isMine;

        Player otherPlayer = other.gameObject.GetComponent<PhotonView>().Controller;
        Debug.Log("Player: " + otherPlayer.NickName + "Is on the block");
        Debug.Log("This Player is Running: " + PhotonNetwork.LocalPlayer.NickName);
        isMine = PhotonNetwork.Equals(PhotonNetwork.LocalPlayer, otherPlayer);
        lobbyManager.SetReady(false,photonView.IsMine);
    }
}
