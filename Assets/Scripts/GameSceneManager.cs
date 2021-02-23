// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsteroidsGameManager.cs" company="Exit Games GmbH">
//   Part of: Asteroid demo
// </copyright>
// <summary>
//  Game Manager for the Asteroid Demo
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Cinemachine;
using UnityEngine.AI;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public static GameSceneManager Instance = null;

    public Text InfoText;

    public GameObject objectplacementManager;
    public GameObject[] NavMesh2DReference;
    public GameObject[] LevelReference;

    public CinemachineVirtualCamera camera;
    public Joystick movementJoystick;
    public Joystick attackJoystick;
    public Button dashButton;

    public int levelCount;

    //public GameObject[] AsteroidPrefabs;

    #region UNITY

    public void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    public void Start()
    {
        Hashtable props = new Hashtable
            {
                {GameData.PLAYER_LOADED_LEVEL, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        StartGame();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
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
        PhotonNetwork.Disconnect();
    }
    public override void OnJoinedRoom()
    {
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
        if (changedProps.ContainsKey(GameData.PLAYER_LIVES))
        {
            CheckEndOfGame();
            return;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }


        // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
        int startTimestamp;
        bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(GameData.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                if (!startTimeIsSet)
                {
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {
/*                // not all players loaded yet. wait:
                Debug.Log("setting text waiting for players! ", this.InfoText);
                InfoText.text = "Waiting for other players...";*/
            }
        }

    }

    #endregion


    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {
        levelCount = 1;

        Debug.Log("Starting the game");
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(LevelReference[levelCount - 1].transform.Find("SpawnPoint").position.x, LevelReference[levelCount - 1].transform.Find("SpawnPoint").position.y, 0), Quaternion.identity, 0);
        player.GetComponent<PlayerInteraction>().m_hand = PhotonNetwork.Instantiate("Sword", new Vector3(LevelReference[levelCount - 1].transform.Find("SpawnPoint").position.x + 1, LevelReference[levelCount - 1].transform.Find("SpawnPoint").position.y, 0), Quaternion.identity, 0);
        player.GetComponent<PlayerInteraction>().m_sword = player.GetComponent<PlayerInteraction>().m_hand;
        player.GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
        player.GetComponent<PhotonView>().RPC("SetSwordReference", RpcTarget.AllBuffered, player.GetComponent<PlayerInteraction>().m_hand.GetComponent<PhotonView>().ViewID);
        player.GetComponent<PlayerData>().m_movementJoystick = movementJoystick;
        player.GetComponent<PlayerData>().m_attackJoystick = attackJoystick;
        player.GetComponent<PlayerData>().m_dashButton = dashButton;
        camera.Follow = player.transform;

        if (PhotonNetwork.IsMasterClient)
        {
            // For Spawning of enemies, NavMesh2D needs to be disable first, then reenabled after all enemies are spawned

            // Looping through EnemyWaypointHolder
            for (int x = 0; x < EnemyManager.GetInstance().EnemyWaypointHolder.Length; x++)
            {
                NavMesh2DReference[x].SetActive(false);
                GameObject LevelSet = null;
                switch (x)
				{
                    case 0:
                        LevelSet = GameObject.Find("Level1");
                        break;

                    case 1:
                        LevelSet = GameObject.Find("Level2");
                        break;
				}
               
                for (int i = 0, j = 0; i < LevelSet.transform.childCount; ++i)
				{
                    if (LevelSet.transform.GetChild(i).gameObject.CompareTag("Enemy"))
					{
                        GameObject enemy = LevelSet.transform.GetChild(i).gameObject;
                        enemy.GetComponent<EnemyData>().m_wayPoint = EnemyManager.GetInstance().EnemyWaypointList[x][j];
                        enemy.GetComponent<EnemyData>().m_ID = j;
                        enemy.GetComponent<Transform>().position = enemy.GetComponent<EnemyData>().m_wayPoint[0].position;
                        enemy.GetComponent<NavMeshAgentScript>().enabled = true;
                        enemy.GetComponent<NavMeshAgent>().enabled = true;
                        enemy.GetComponent<NavMeshAgent>().updateRotation = false;
                        enemy.GetComponent<NavMeshAgent>().updateUpAxis = false;
                        enemy.GetComponent<Animator>().SetInteger("Health", enemy.GetComponent<EnemyData>().m_maxHealth);
                        j++;
                    }
                }

                NavMesh2DReference[x].SetActive(true);
            }

            GetComponent<PhotonView>().RPC("SetCurrentLevel", RpcTarget.All, levelCount);
        }
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(GameData.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    private void CheckEndOfGame()
    {

        bool allDestroyed = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object lives;
            if (p.CustomProperties.TryGetValue(GameData.PLAYER_LIVES, out lives))
            {
                if ((int)lives > 0)
                {
                    allDestroyed = false;
                    break;
                }
            }
        }

        if (allDestroyed)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int score = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                    score = p.GetScore();
                }
            }

            StartCoroutine(EndOfGame(winner, score));
        }
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }
    public void ChangeScene()
	{
        levelCount++;
        GetComponent<PhotonView>().RPC("SetCurrentLevel", RpcTarget.AllViaServer, levelCount);
    }

    [PunRPC]
	public void SortEnemyReferences(int[] EnemyViewID, string level, bool setParentActive)
	{
		Transform levelGameObjectParent = GameObject.Find(level).transform;

		for (int i = 0; i < EnemyViewID.Length; i++)
			PhotonView.Find(EnemyViewID[i]).transform.SetParent(levelGameObjectParent, false);

		levelGameObjectParent.gameObject.SetActive(setParentActive);
	}

    [PunRPC]
    public void SetCurrentLevel(int LevelToActivate)
	{
        for (int i = 0; i < LevelReference.Length; i++)
        {
            if (i == LevelToActivate - 1)
            {
                LevelReference[i].SetActive(true);
                for (int j = 0; j < LevelReference[i].transform.childCount; ++j)
                {
                    if (LevelReference[i].transform.GetChild(j).gameObject.CompareTag("Enemy"))
                    {
                        GameObject enemy = LevelReference[i].transform.GetChild(j).gameObject;
                        enemy.GetComponent<Animator>().SetInteger("Health", enemy.GetComponent<EnemyData>().m_maxHealth);
                    }
                }

            }
            else if (LevelReference[i].activeSelf != false)
                LevelReference[i].SetActive(false);
        }
    }
}