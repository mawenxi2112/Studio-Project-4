using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;
    public bool isDoorLock;
    public GameObject StoneCollider;
    public GameObject DoorCollider;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        isDoorLock = true;
        animator.SetBool("IsLock", isDoorLock);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Door_Opened"))
        {
            boxCollider.enabled = false;
            StoneCollider.GetComponent<PolygonCollider2D>().enabled = true;
            DoorCollider.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
	void OnCollisionStay2D(Collision2D collision)
	{
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
		if (collision.gameObject.CompareTag("Hitbox"))
		{
            // Start next level
            Debug.Log("POWER UP + NEXT LEVEL");
		}
	}
}
