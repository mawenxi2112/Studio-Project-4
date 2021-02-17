using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;
    public CircleCollider2D circleCollider;

    public bool startCountDown;
    
    float Countdown = 0;
    double tickDuration = 0;
    double tickRequiredToChange = 0.2;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        startCountDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountDown)
        {
            tickDuration += Time.deltaTime;

            if (tickDuration >= tickRequiredToChange)
            {
                tickDuration = 0;
                Countdown++;
                animator.SetFloat("Countdown", Countdown);

                if (Countdown >= 10)
                {
                    boxCollider.enabled = !boxCollider.enabled;
                    circleCollider.enabled = !circleCollider.enabled;
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Bomb_Despawn"))
            {
                gameObject.SetActive(false);
            }
        }
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
        // This function is use to do damaging of other objects around the surroundings
        collision.gameObject.SetActive(false);
    }

	void OnTriggerStay2D(Collider2D collider)
	{
		// This function use to do picking of bomb
	}
}
