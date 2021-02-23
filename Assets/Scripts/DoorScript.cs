using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;
    public GameObject LeftCollider;
    public GameObject RightCollider;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator.SetBool("IsLock", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Door_Opened"))
        {
            boxCollider.enabled = false;
            LeftCollider.GetComponent<BoxCollider2D>().enabled = true;
            RightCollider.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Door_Lock"))
        {
            boxCollider.enabled = true;
            LeftCollider.GetComponent<BoxCollider2D>().enabled = false;
            RightCollider.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
	void OnCollisionStay2D(Collision2D collision)
	{
	}

}
