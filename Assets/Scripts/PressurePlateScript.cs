using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

	void OnTriggerStay2D(Collider2D collision)
	{
        if (PhotonNetwork.IsMasterClient && (collision.CompareTag("Player") || collision.CompareTag("Objects")))
		{
            if (collision.CompareTag("Objects"))
			{
                if (collision.gameObject.GetComponent<ObjectData>().object_type != OBJECT_TYPE.MOVEABLEBLOCK)
                    return;
			}
            GetComponent<Animator>().SetBool("isPressed", true);
		}
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        if (PhotonNetwork.IsMasterClient && (collision.CompareTag("Player") || collision.CompareTag("Objects")))
        {
            if (collision.CompareTag("Objects"))
            {
                if (collision.gameObject.GetComponent<ObjectData>().object_type != OBJECT_TYPE.MOVEABLEBLOCK)
                    return;
            }
            GetComponent<Animator>().SetBool("isPressed", false);
        }
    }
}
