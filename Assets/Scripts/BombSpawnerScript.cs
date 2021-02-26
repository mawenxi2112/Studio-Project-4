using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BombSpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bomb;

    void Start()
    {
        bomb = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (bomb == null)
		{
            bomb = PhotonNetwork.Instantiate("Bomb", gameObject.transform.position, Quaternion.identity);
		}
    }
}
