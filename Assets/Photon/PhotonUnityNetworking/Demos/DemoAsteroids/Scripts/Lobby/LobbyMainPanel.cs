using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Asteroids
{
    public class LobbyMainPanel : MonoBehaviourPunCallbacks
    {
      
        public InputField PlayerNameInput;
        public InputField RoomNameInputField;

        public GameObject DataManager;
        
       


        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;

        #region UNITY

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
            
            /*PlayerNameInput.text = "Player " + Random.Range(1000, 10000);*/
        }
        void Update()
        {
/*            if (PhotonNetwork.InRoom)
            {
                Debug.Log("You are in Room:" + PhotonNetwork.CurrentRoom.Name);
            }*/
        }
        #endregion

        #region PUN CALLBACKS

        public override void OnConnectedToMaster()
        {
            
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();

            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Create Room Failed");

        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
           
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            string roomName = "Room " + Random.Range(1000, 10000);

            RoomOptions options = new RoomOptions {MaxPlayers = 8};

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
                return;
            Hashtable props = new Hashtable
            {
                {AsteroidsGame.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
       
            PhotonNetwork.LoadLevel("GameLobbyScene");

        }
        public override void OnCreatedRoom()
        {
            Debug.Log("CREATED A ROOM!");
            Hashtable props = new Hashtable
            {
                {AsteroidsGame.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            PhotonNetwork.LoadLevel("GameLobbyScene");
        }

        public override void OnLeftRoom()
        {


            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("EnteredRoom!");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);

        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {

            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                object isPlayerReady;
                if (changedProps.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
                }
            }

        }

        #endregion

        #region UI CALLBACKS

        public void OnBackButtonClicked()
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

  
        }

        public void OnCreateRoomButtonClicked()
        {
            string roomName = RoomNameInputField.text;
     
            roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;
            if(cachedRoomList.ContainsKey(roomName))
            {
                Debug.Log("Room Name Taken");
                return;
            }
            byte maxPlayers;

            maxPlayers = 2;

            RoomOptions options = new RoomOptions {MaxPlayers = maxPlayers, PlayerTtl = 10000 };
            PhotonNetwork.LocalPlayer.NickName = PlayerNameInput.text;
            PhotonNetwork.CreateRoom(roomName, options, null);
            Debug.Log("Trying to join Scene");
   
 
        }

        public void OnJoinRandomRoomButtonClicked()
        {

            PhotonNetwork.JoinRandomRoom();
        }

        public void OnLeaveGameButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void OnLoginButtonClicked()
        {
            string playerName = PlayerNameInput.text;

            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                Debug.LogError("Player Name is invalid.");
            }
        }

        public void OnJoinButtonClicked()
        {
            if(!PhotonNetwork.IsConnected)
            {
                Debug.Log("Player is not yet connected");
            }
            if(RoomNameInputField.text == "")
            {
                PhotonNetwork.LocalPlayer.NickName = PlayerNameInput.text;
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("JoiningRandomRoom: ");
                return;
            }
            Debug.Log("Joining: " + RoomNameInputField.text);
            if (!cachedRoomList.ContainsKey(RoomNameInputField.text))
                Debug.Log("This Room has yet to exist");

            PhotonNetwork.LocalPlayer.NickName = PlayerNameInput.text;
            PhotonNetwork.JoinRoom(RoomNameInputField.text);
   
        }
        public void OnRoomListButtonClicked()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }

   
        }

        public void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel("Level1Scene");
        }

        #endregion

        private bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
                {
                    if (!(bool) isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        
        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }

        public void LocalPlayerPropertiesUpdated()
        {
        }

        private void SetActivePanel(string activePanel)
        {
            
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

        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in cachedRoomList.Values)
            {
            }
        }
    }
}