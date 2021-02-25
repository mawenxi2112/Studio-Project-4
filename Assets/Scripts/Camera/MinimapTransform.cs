using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapTransform : MonoBehaviour
{
    public PlayerData player;

    private Transform playerTransform;

    void Start()
    {
        playerTransform = null;
    }

    void LateUpdate()
    {
        if (playerTransform == null)
        {
            playerTransform = player.gameObject.GetComponent<Transform>();
        }
        else
        {
            Vector3 newPosition = playerTransform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }
}