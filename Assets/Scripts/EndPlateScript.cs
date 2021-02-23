using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlateScript : MonoBehaviour
{
    public int number = 0;
    public bool once = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // We check isMasterClient as we only one person to do the change scene call, and only when number of people on the plate is the same as the amount on the network.
        if (PhotonNetwork.IsMasterClient && number == PhotonNetwork.PlayerList.Length && !once)
        {
            once = true;
            GameObject.Find("GameManager").GetComponent<GameSceneManager>().ChangeScene();
        }
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
            number++;
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
            number--;
    }
}
