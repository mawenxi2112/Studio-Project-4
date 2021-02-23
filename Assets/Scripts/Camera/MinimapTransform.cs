using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapTransform : MonoBehaviour
{
    public Transform playersTransform;

    // Start is called before the first frame update
    void Start()
    {
        playersTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        // If playersTransform is not assigned
        if (playersTransform == null)
        {
            GameObject[] playerGO = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < playerGO.Length; i++)
            {
                Debug.Log(playerGO[i].name);


                if (playerGO[i].GetComponent<PhotonView>().IsMine)
                {
                    //gameObject.transform = playerGO[i].GetComponent<Transform>();
                }
            }
        }
    }
}
