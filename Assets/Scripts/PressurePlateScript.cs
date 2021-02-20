using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public Animator animator;
    public bool isSteppedOn;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isSteppedOn = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

	void OnTriggerStay2D(Collider2D collision)
	{
        if (collision.CompareTag("Player") || collision.CompareTag("Objects"))
        {
            if (animator.GetFloat("TriggerChange") == 0)
            {
                animator.SetFloat("TriggerChange", 1);
                isSteppedOn = true;
            }
        }
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        if (animator.GetFloat("TriggerChange") == 1)
        {
            animator.SetFloat("TriggerChange", 0);
            isSteppedOn = false;
        }
    }
}
