using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Animator animator;
    public CircleCollider2D circleCollider;

    public bool startCountDown;
    public bool isBlowingUp;
    
    float Countdown = 0;
    double tickDuration = 0;
    double tickRequiredToChange = 0.2;

    private float regularColliderSize = 0.35f;
    private float expandedColliderSize = 0.6637768f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        startCountDown = false;
        isBlowingUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine)
		{
            return;
		}

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
                    circleCollider.enabled = !circleCollider.enabled;
                    circleCollider.radius = expandedColliderSize;
                    isBlowingUp = true;
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Bomb_Despawn"))
            {
                gameObject.SetActive(false);
                Destroy(this);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBlowingUp)
        {
            collision.gameObject.SetActive(false);
        }
    }
}
