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
        Debug.Log("StartGame!");

        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

		// REMOVE THESE WHEN TESTING MULTIPLAYER, ONLY USE THIS FOR LOCAL TESTING
		GameObject tmpPlayer = Instantiate(playerPrefab);
		tmpPlayer.transform.position = new Vector3(10, 2, 0);
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

		tmpPlayer.GetComponent<PlayerInteraction>().m_hand = Instantiate(swordPrefab);
		tmpPlayer.GetComponent<PlayerInteraction>().m_sword = tmpPlayer.GetComponent<PlayerInteraction>().m_hand;
		tmpPlayer.GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
		tmpPlayer.GetComponent<PlayerInteraction>().transform.position = new Vector3(10, 2, 0);

        for (int i = 0; i < objectplacementManager.transform.childCount; i++)
		{
            List<GameObject> TmpStorage = new List<GameObject>();

            if (objectplacementManager.transform.GetChild(i).gameObject.CompareTag("DoorPlacement"))
			{
                for (int j = 0; j < objectplacementManager.transform.GetChild(i).childCount; j++)
				{
                    GameObject doortmp = objectplacementManager.transform.GetChild(i).GetChild(j).gameObject;
                    GameObject networkDoor = Instantiate(doorPrefab);
                    networkDoor.transform.position = new Vector3(doortmp.transform.position.x, doortmp.transform.position.y , 0);
                    TmpStorage.Add(networkDoor);
                    
                    Destroy(doortmp);
                }
            }
            else if (objectplacementManager.transform.GetChild(i).gameObject.CompareTag("CoinPlacement"))
			{
                for (int j = 0; j < objectplacementManager.transform.GetChild(i).childCount; j++)
                {
                    GameObject cointmp = objectplacementManager.transform.GetChild(i).GetChild(j).gameObject;
                    GameObject networkCoin = Instantiate(coinPrefab);
                    networkCoin.transform.position = new Vector3(cointmp.transform.position.x, cointmp.transform.position.y, 0);
                    TmpStorage.Add(networkCoin);

                    Destroy(cointmp);
                }
            }
            else if (objectplacementManager.transform.GetChild(i).gameObject.CompareTag("HealthpackPlacement"))
            {
                for (int j = 0; j < objectplacementManager.transform.GetChild(i).childCount; j++)
                {
                    GameObject healthpacktmp = objectplacementManager.transform.GetChild(i).GetChild(j).gameObject;
                    GameObject networkHealthpack = Instantiate(healthpackPrefab);
                    networkHealthpack.transform.position = new Vector3(healthpacktmp.transform.position.x, healthpacktmp.transform.position.y, 0);
                    TmpStorage.Add(networkHealthpack);

                    Destroy(healthpacktmp);
                }
            }
            else if (objectplacementManager.transform.GetChild(i).gameObject.CompareTag("ChestPlacement"))
            {
                for (int j = 0; j < objectplacementManager.transform.GetChild(i).childCount; j++)
                {
                    GameObject chesttmp = objectplacementManager.transform.GetChild(i).GetChild(j).gameObject;
                    GameObject networkchest = Instantiate(chestPrefab);
                    networkchest.transform.position = new Vector3(chesttmp.transform.position.x, chesttmp.transform.position.y, 0);
                    TmpStorage.Add(networkchest);

                    Destroy(chesttmp);
                }
            }
            else if (objectplacementManager.transform.GetChild(i).gameObject.CompareTag("TorchPlacement"))
            {
                for (int j = 0; j < objectplacementManager.transform.GetChild(i).childCount; j++)
                {
                    GameObject torchtmp = objectplacementManager.transform.GetChild(i).GetChild(j).gameObject;
                    GameObject networktorch = Instantiate(torchPrefab);
                    networktorch.transform.position = new Vector3(torchtmp.transform.position.x, torchtmp.transform.position.y, 0);
                    TmpStorage.Add(networktorch);

                    Destroy(torchtmp);
                }
            }
            else if (objectplacementManager.transform.GetChild(i).gameObject.CompareTag("KeyPlacement"))
            {
                for (int j = 0; j < objectplacementManager.transform.GetChild(i).childCount; j++)
                {
                    GameObject keytmp = objectplacementManager.transform.GetChild(i).GetChild(j).gameObject;
                    GameObject networkkey = Instantiate(keyPrefab);
                    networkkey.transform.position = new Vector3(keytmp.transform.position.x, keytmp.transform.position.y, 0);
                    TmpStorage.Add(networkkey);

                    Destroy(keytmp);
                }
            }
            else if (objectplacementManager.transform.GetChild(i).gameObject.CompareTag("PuzzlePlacement"))
            {
                // First for-loop loops through the number of puzzle sets there are
                for (int j = 0; j < objectplacementManager.transform.GetChild(i).childCount; j++)
                {
                    GameObject PuzzleSettmp = objectplacementManager.transform.GetChild(i).GetChild(j).gameObject;
                    List<GameObject> OTUTmpStorage = new List<GameObject>();
                    List<GameObject> ROTmpStorage = new List<GameObject>();

                    // Second for-loop loops through the ObjectToUnlockGate, RegularObject, Gate in the puzzle set
                    for (int k = 0; k < PuzzleSettmp.transform.childCount; k++)
                    {
                        if (PuzzleSettmp.transform.GetChild(k).gameObject.name != "Gate")
                        {
                            // Third for-loop loops through the child in ObjectToUnlock and RegularObject
                            for (int l = 0; l < PuzzleSettmp.transform.GetChild(k).childCount; l++)
                            {
                                GameObject OTUorROchild = PuzzleSettmp.transform.GetChild(k).GetChild(l).gameObject;
                                switch (OTUorROchild.GetComponent<ObjectData>().object_type)
                                {
                                    case OBJECT_TYPE.CAMPFIRE:
                                        GameObject networkcampfire = Instantiate(campfirePrefab);
                                        networkcampfire.transform.position = new Vector3(OTUorROchild.transform.position.x, OTUorROchild.transform.position.y, 0);

                                        if (PuzzleSettmp.transform.GetChild(k).gameObject.name == "ObjectsToUnlockGate")
                                            OTUTmpStorage.Add(networkcampfire);
                                        else
                                            ROTmpStorage.Add(networkcampfire);

                                        Destroy(OTUorROchild);
                                        break;

                                    case OBJECT_TYPE.PRESSUREPLATE:
                                        GameObject networkpressureplate = Instantiate(pressureplatePrefab);
                                        networkpressureplate.transform.position = new Vector3(OTUorROchild.transform.position.x, OTUorROchild.transform.position.y, 0);

                                        if (PuzzleSettmp.transform.GetChild(k).gameObject.name == "ObjectsToUnlockGate")
                                            OTUTmpStorage.Add(networkpressureplate);
                                        else
                                            ROTmpStorage.Add(networkpressureplate);

                                        Destroy(OTUorROchild);
                                        break;

                                    case OBJECT_TYPE.RESETBUTTON:
                                        break;
                                };

                            }
                        }
                    }

                    for (int k = 0; k < PuzzleSettmp.transform.childCount; k++)
                    {
                        if (PuzzleSettmp.transform.GetChild(k).gameObject.name == "ObjectsToUnlockGate")
                        {
                            for (int x = 0; x < OTUTmpStorage.Count; x++)
                            {
                                OTUTmpStorage[x].transform.SetParent(PuzzleSettmp.transform.GetChild(k), false);
                            }
                        }
                        else if (PuzzleSettmp.transform.GetChild(k).gameObject.name == "RegularObjects")
                        {
                            for (int x = 0; x < ROTmpStorage.Count; x++)
                            {
                                ROTmpStorage[x].transform.SetParent(PuzzleSettmp.transform.GetChild(k), false);
                            }
                        }
                        else if (PuzzleSettmp.transform.GetChild(k).gameObject.name == "Gate")
                        {
                            GameObject networkgate = Instantiate(gatePrefab);
                            networkgate.transform.position = new Vector3(PuzzleSettmp.transform.GetChild(k).position.x, PuzzleSettmp.transform.GetChild(k).position.y, 0);
                            for (int x = 0; x < OTUTmpStorage.Count; x++)
                            {
                                networkgate.GetComponent<GateScript>().ListOfObjectRequiredToOpenGate.Add(OTUTmpStorage[x].gameObject);
                            }
                            networkgate.transform.SetParent(PuzzleSettmp.transform);
                            Destroy(PuzzleSettmp.transform.GetChild(k).gameObject);
                        }
                    }
                }
            }

            for (int j = 0; j < TmpStorage.Count; j++)
			{
                TmpStorage[j].transform.SetParent(objectplacementManager.transform.GetChild(i), false);
			}
        }

        // For Spawning of enemies, NavMesh2D needs to be disable first, then reenabled after all enemies are spawned
        NavMesh2DReference.SetActive(false);
        for (int i = 0; i < EnemyManager.GetInstance().EnemyWaypointHolder.Count; i++)
        {
            if (EnemyManager.GetInstance().EnemyWaypointHolder[i].CompareTag("MeleeWaypoint"))
            {
                GameObject enemy = Instantiate(meeleEnemyPrefab);
                enemy.GetComponent<EnemyData>().m_ID = i;
                enemy.GetComponent<EnemyData>().m_wayPoint = EnemyManager.GetInstance().EnemyWaypointList[enemy.GetComponent<EnemyData>().m_ID];
                enemy.GetComponent<Transform>().position = enemy.GetComponent<EnemyData>().m_wayPoint[0].position;
            }
            else if (EnemyManager.GetInstance().EnemyWaypointHolder[i].CompareTag("RangeWaypoint"))
            {
                GameObject enemy = Instantiate(rangeEnemyPrefab);
                enemy.GetComponent<EnemyData>().m_ID = i;
                enemy.GetComponent<EnemyData>().m_wayPoint = EnemyManager.GetInstance().EnemyWaypointList[enemy.GetComponent<EnemyData>().m_ID];
                enemy.GetComponent<Transform>().position = enemy.GetComponent<EnemyData>().m_wayPoint[0].position;
            }
            else if (EnemyManager.GetInstance().EnemyWaypointHolder[i].CompareTag("RunnerWaypoint"))
            {
                GameObject enemy = Instantiate(runnerEnemyPrefab);
                enemy.GetComponent<EnemyData>().m_ID = i;
                enemy.GetComponent<EnemyData>().m_wayPoint = EnemyManager.GetInstance().EnemyWaypointList[enemy.GetComponent<EnemyData>().m_ID];
                enemy.GetComponent<Transform>().position = enemy.GetComponent<EnemyData>().m_wayPoint[0].position;
            }
        }
        NavMesh2DReference.SetActive(true);

        camera.Follow = tmpPlayer.transform;

        GameObject player = PhotonNetwork.Instantiate("Player", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)


        if (player.GetComponent<PhotonView>().IsMine)
        {
            // Do targetting of camera here.
        }

        if (PhotonNetwork.IsMasterClient)
        {
            
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