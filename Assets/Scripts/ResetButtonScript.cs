﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonScript : MonoBehaviour
{
    public Animator animator;
    public List<GameObject> ListOfObjectToReset;

	void Awake()
	{
        ListOfObjectToReset = new List<GameObject>();
	}

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();

        // Link ObjectsToReset from the scene to the reset button
        GameObject ResetButtonParent = gameObject.transform.parent.gameObject;
        GameObject ObjectsToReset = ResetButtonParent.transform.Find("ObjectsToReset").gameObject;
        for (int i = 0; i < ObjectsToReset.transform.childCount; i++)
        {
            ListOfObjectToReset.Add(ObjectsToReset.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D collision)
	{
        if (PhotonNetwork.IsMasterClient && (collision.CompareTag("Player") || collision.CompareTag("Objects")))
        {
            animator.SetBool("isPressed", true);

            for (int i = 0; i < ListOfObjectToReset.Count; i++)
            {
                ListOfObjectToReset[i].GetComponent<Transform>().position = ListOfObjectToReset[i].GetComponent<ObjectData>().originalPosition;
            }
        }
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        if (PhotonNetwork.IsMasterClient && (collision.CompareTag("Player") || collision.CompareTag("Objects")))
            animator.SetBool("isPressed", false);
    }
}
