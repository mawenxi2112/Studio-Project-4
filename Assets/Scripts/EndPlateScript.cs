using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlateScript : MonoBehaviour
{

    public int NoOfPlayerIn = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
        {
            NoOfPlayerIn++;

            if (!PhotonNetwork.IsMasterClient)
                return;

            if (NoOfPlayerIn == PhotonNetwork.CountOfPlayers)
            {
                GameObject.Find("GameManager").GetComponent<GameSceneManager>().ChangeScene();
            }
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
