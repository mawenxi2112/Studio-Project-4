﻿// --------------------------------------------------------------------------------------------------------------------
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

    public GameObject playerPrefab;

    public GameObject coinPrefab;
    public GameObject keyPrefab;
    public GameObject healthpackPrefab;
    public GameObject spikePrefab;
    public GameObject moveableblockPrefab;
    public GameObject torchPrefab;
    public GameObject campfirePrefab;
    public GameObject bombPrefab;
    public GameObject breakableblockPrefab;
    public GameObject surprisetrapblockPrefab;
    public GameObject chestPrefab;
    public GameObject pressureplatePrefab;
    public GameObject resetbuttonPrefab;
    public GameObject doorPrefab;
    public GameObject gatePrefab;
    public GameObject meeleEnemyPrefab;
    public GameObject rangeEnemyPrefab;
    public GameObject runnerEnemyPrefab;
    public GameObject swordPrefab;

    public GameObject objectplacementManager;
    public GameObject NavMesh2DReference;

    public CinemachineVirtualCamera camera;

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
                // not all players loaded yet. wait:
                Debug.Log("setting text waiting for players! ", this.InfoText);
                InfoText.text = "Waiting for other players...";
            }
        }

    }

    #endregion


    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {

        // REMOVE THESE WHEN TESTING MULTIPLAYER, ONLY USE THIS FOR LOCAL TESTING
        /*GameObject tmpPlayer = Instantiate(playerPrefab);
		tmpPlayer.transform.position = new Vector3(10, 2, 0);*/
        //Instantiate(coinPrefab).transform.position = new Vector3(1, 0, 0);
        //Instantiate(keyPrefab).transform.position = new Vector3(2, 0, 0);
        //Instantiate(healthpackPrefab).transform.position = new Vector3(3, 0, 0);
        //Instantiate(spikePrefab).transform.position = new Vector3(5, 0, 0);
        //Instantiate(moveableblockPrefab).transform.position = new Vector3(-2, 0, 0);
        //Instantiate(torchPrefab).transform.position = new Vector3(7, 0, 0);
        //Instantiate(campfirePrefab).transform.position = new Vector3(9, 0, 0);
        //Instantiate(bombPrefab).transform.position = new Vector3(-5, 0, 0);
        //Instantiate(breakableblockPrefab).transform.position = new Vector3(-5, 2, 0);
        //Instantiate(surprisetrapblockPrefab).transform.position = new Vector3(-5, 4, 0);
        //Instantiate(chestPrefab).transform.position = new Vector3(-7, 0, 0);
        //Instantiate(pressureplatePrefab).transform.position = new Vector3(0, -1, 0);
        //Instantiate(resetbuttonPrefab).transform.position = new Vector3(-2, -1, 0);
        //Instantiate(doorPrefab).transform.position = new Vector3(-10, 0, 0);

        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(10, 2, 0), Quaternion.identity, 0);
        player.GetComponent<PlayerInteraction>().m_hand = PhotonNetwork.Instantiate("Sword", new Vector3(11, 2, 0), Quaternion.identity, 0);
        player.GetComponent<PlayerInteraction>().m_sword = player.GetComponent<PlayerInteraction>().m_hand;
        player.GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
        player.GetComponent<PhotonView>().RPC("SetSwordReference", RpcTarget.AllBuffered, player.GetComponent<PlayerInteraction>().m_hand.GetComponent<PhotonView>().ViewID);
        camera.Follow = player.transform;

        if (PhotonNetwork.IsMasterClient)
        {
            // For Spawning of enemies, NavMesh2D needs to be disable first, then reenabled after all enemies are spawned
            NavMesh2DReference.SetActive(false);
            for (int i = 0; i < EnemyManager.GetInstance().EnemyWaypointHolder.Count; i++)
            {
                if (EnemyManager.GetInstance().EnemyWaypointHolder[i].CompareTag("MeleeWaypoint"))
                {
                    GameObject enemy;

                    enemy = PhotonNetwork.InstantiateRoomObject("MeleeEnemy", new Vector3(0, 0, 0), Quaternion.identity, 0);

                    enemy.GetComponent<EnemyData>().m_ID = i;
                    enemy.GetComponent<EnemyData>().m_wayPoint = EnemyManager.GetInstance().EnemyWaypointList[enemy.GetComponent<EnemyData>().m_ID];
                    enemy.GetComponent<Transform>().position = enemy.GetComponent<EnemyData>().m_wayPoint[0].position;
                    enemy.GetComponent<NavMeshAgentScript>().enabled = true;
                    enemy.GetComponent<NavMeshAgent>().enabled = true;
                    enemy.GetComponent<NavMeshAgent>().updateRotation = false;
                    enemy.GetComponent<NavMeshAgent>().updateUpAxis = false;
                }
                else if (EnemyManager.GetInstance().EnemyWaypointHolder[i].CompareTag("RangeWaypoint"))
                {
                    GameObject enemy;

                    enemy = PhotonNetwork.InstantiateRoomObject("RangedEnemy", new Vector3(0, 0, 0), Quaternion.identity, 0);

                    enemy.GetComponent<EnemyData>().m_ID = i;
                    enemy.GetComponent<EnemyData>().m_wayPoint = EnemyManager.GetInstance().EnemyWaypointList[enemy.GetComponent<EnemyData>().m_ID];
                    enemy.GetComponent<Transform>().position = enemy.GetComponent<EnemyData>().m_wayPoint[0].position;
                    enemy.GetComponent<NavMeshAgentScript>().enabled = true;
                    enemy.GetComponent<NavMeshAgent>().enabled = true;
                    enemy.GetComponent<NavMeshAgent>().updateRotation = false;
                    enemy.GetComponent<NavMeshAgent>().updateUpAxis = false;
                }
                else if (EnemyManager.GetInstance().EnemyWaypointHolder[i].CompareTag("RunnerWaypoint"))
                {
                    GameObject enemy;

                    enemy = PhotonNetwork.InstantiateRoomObject("RunnerEnemy", new Vector3(0, 0, 0), Quaternion.identity, 0);

                    enemy.GetComponent<EnemyData>().m_ID = i;
                    enemy.GetComponent<EnemyData>().m_wayPoint = EnemyManager.GetInstance().EnemyWaypointList[enemy.GetComponent<EnemyData>().m_ID];
                    enemy.GetComponent<Transform>().position = enemy.GetComponent<EnemyData>().m_wayPoint[0].position;
                    enemy.GetComponent<NavMeshAgentScript>().enabled = true;
                    enemy.GetComponent<NavMeshAgent>().enabled = true;
                    enemy.GetComponent<NavMeshAgent>().updateRotation = false;
                    enemy.GetComponent<NavMeshAgent>().updateUpAxis = false;
                }
            }
            NavMesh2DReference.SetActive(true);
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

}