using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoorScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;
    public bool isDoorLock;
    public GameObject LeftCollider;
    public GameObject RightCollider;


    // Start is called before the first frame update
    void Start()
    { 
        if (!PhotonNetwork.IsMasterClient)
            return;

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        isDoorLock = true;
        animator.SetBool("IsLock", isDoorLock);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Door_Opened"))
        {
            boxCollider.enabled = false;
            LeftCollider.GetComponent<BoxCollider2D>().enabled = true;
            RightCollider.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
	void OnCollisionStay2D(Collision2D collision)
	{
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (collision.gameObject.CompareTag("Player"))
		{
            if (collision.gameObject.GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.KEY && Input.GetKeyDown(KeyCode.Mouse0) && isDoorLock)
			{
                isDoorLock = false;
                animator.SetBool("IsLock", isDoorLock);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Hitbox") && !isDoorLock)
		{
            // Start next level
            //Debug.Log("POWER UP + NEXT LEVEL");
		}
	}
}
