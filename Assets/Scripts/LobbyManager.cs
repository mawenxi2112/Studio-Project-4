
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Cinemachine;
using TMPro;
using UnityEngine.AI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance = null;


    public Text InfoText;
    public GameObject playerPrefab;
    public Transform readyButton;
    public TMP_Text playerInfo;
    public TMP_Text serverInfo;
    
    public Camera camera;
    public PhotonView photonView;
    public Joystick joystick;

    public LoadScene SceneManager;
    public int playerReady;
    //public GameObject[] AsteroidPrefabs;

    #region UNITY

    public void Awake()
    {
        Instance = this;
    }
    void Update()
    {
      /*  if (PhotonNetwork.IsMasterClient)
            Debug.Log("PlayerAmount: " + PhotonNetwork.PlayerList.Length);*/
    }
    public override void OnEnable()
    {
        base.OnEnable();

    }

    public void Start()
    {
        Hashtable props = new Hashtable
            {
                {GameData.PLAYER_READY,false}
            };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        StartGame();
    }

    public void SetReady(bool value,bool isMine)
    {
        if (isMine)
        {
            Hashtable props = new Hashtable
            {
                {GameData.PLAYER_READY,value}
            };
            Debug.Log("Setting Who?" + PhotonNetwork.LocalPlayer.NickName);
            Debug.Log("Setting to? " + value.ToString());
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }

    #endregion

    #region COROUTINES

    /*    private IEnumerator SpawnAsteroid()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(GameDataa.ASTEROIDS_MIN_SPAWN_TIME, AsteroidsGame.ASTEROIDS_MAX_SPAWN_TIME));

                Vector2 direction = Random.insideUnitCircle;
                Vector3 position = Vector3.zero;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Make it appear on the left/right side
                    position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
                }
                else
                {
                    // Make it appear on the top/bottom
                    position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
                }

                // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
                position -= position.normalized * 0.1f;


                Vector3 force = -position.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };

                PhotonNetwork.InstantiateRoomObject("BigAsteroid", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
            }
        }
    */
    private IEnumerator EndOfGame(string winner, int score)
    {
        float timer = 5.0f;

        while (timer > 0.0f)
        {
            InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DemoAsteroids-LobbyScene");
    }
        public override void OnLeftRoom()
    {
      
    }
    public void PlayerLeaveRoom()
    {
        StartCoroutine(LeaveRoomAndLoad());
    }

    IEnumerator LeaveRoomAndLoad()
    {
        PhotonNetwork.LeaveRoom();

        while(PhotonNetwork.InRoom)
        yield return null;

        SceneManager.LoadPlayMenu();
    }
    
    public override void OnJoinedRoom()
    {
/*        object isPlayerReady;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady);
        Debug.Log("Starting Value: " + (bool)isPlayerReady);*/
        base.OnJoinedRoom();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //StartCoroutine(SpawnAsteroid());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Force the game to quit/end

        //CheckEndOfGame();
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("Running Into Game");

    
        if(CheckPlayersReady())
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            Debug.Log("Loading Game Scene!!!!!!!!!!!!!");
            PhotonNetwork.LoadLevel("Level1Scene");
        }

    }

    #endregion


    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {

        GameObject player;
        GameObject world = GameObject.Find("World");

         player = PhotonNetwork.Instantiate("Player", new Vector3(-4, -4, 0), Quaternion.identity, 0);
      /*  player.GetComponent<PlayerMovement>().joystick = joystick;*/
        player.transform.SetParent(world.transform);
        player.GetComponent<PlayerData>().platform = 1;
        playerInfo.text = "Player Name: " +PhotonNetwork.LocalPlayer.NickName;
        serverInfo.text = "Server: " +PhotonNetwork.CurrentRoom.Name;

        /*        Hashtable props = new Hashtable
                    {
                        {GameData.PLAYER_LOADED_LEVEL, true}
                    };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);*/ // SETting player Properties

/*        photonView = player.GetComponent<PhotonView>();
        photonView.RPC("UpdateReady", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName, false);
        playerReadyList.Add(PhotonNetwork.LocalPlayer.NickName, false);*/
        Physics.IgnoreLayerCollision(0, 9);
        Debug.Log("PlayerReady: " + CheckPlayersReady().ToString());
        Debug.Log("Size of PlayerList: " + PhotonNetwork.PlayerList.Length.ToString());
    }
    public void SetPlayerReady(bool State)
    {
        Hashtable props = new Hashtable
            {
                {GameData.PLAYER_READY, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
  
    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }
      
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Debug.Log("PlayerName: " + p.NickName);
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
            {
                Debug.Log("PlayerReady: " + (bool)isPlayerReady);
                if (!(bool)isPlayerReady)
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
}
