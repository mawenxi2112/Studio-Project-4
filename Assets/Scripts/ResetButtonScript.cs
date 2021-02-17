using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonScript : MonoBehaviour
{
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
            }
        }
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        if (animator.GetFloat("TriggerChange") == 1)
        {
            animator.SetFloat("TriggerChange", 0);
        }
    }
}
