using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
namespace Photon.Pun
{
    public class CreateRoom : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        public TMP_InputField playerName;
        public TMP_InputField serverID;

        private Dictionary<string, RoomInfo> cachedRoomList;
        #region UNITY
        void Start()
        {

        }
        void Update()
        {
            if(PhotonNetwork.InRoom)
            {
                Debug.Log("In Room: " + PhotonNetwork.CurrentRoom.Name);
                Debug.Log("PlayerName: " + PhotonNetwork.LocalPlayer.NickName);
            }
        }
        #endregion
        // Update is called once per frame
        #region PUN CALLBACKS
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            UpdateCachedRoomList(roomList);
        }
        public void CreateLobby()
        {
            if(!PhotonNetwork.IsConnected)
            {
                Debug.Log("Not Connected yet!");
                return;
            }
            if (cachedRoomList.ContainsKey(serverID.text))
            {
                Debug.Log("Room Name Taken!");
                return;
            }
            PhotonNetwork.LocalPlayer.NickName = playerName.text;
            byte maxPlayers = 2;
            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };
            PhotonNetwork.CreateRoom(serverID.text, options, null);
            Debug.Log("RoomCreated With: " + serverID.text);
        }
        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }
        #endregion
    }
}