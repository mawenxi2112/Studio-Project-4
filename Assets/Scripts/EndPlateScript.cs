using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlateScript : MonoBehaviour
{

    public int NoOfPlayerIn;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        //Debug.Log("NoOfPlayers: " + NoOfPlayerIn);
        //Debug.Log("Players in Network: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (NoOfPlayerIn == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            GameObject.Find("GameManager").GetComponent<GameSceneManager>().ChangeScene();
        }
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
        {
            NoOfPlayerIn++;
        }
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
        {
            NoOfPlayerIn--;
        }
    }
}
