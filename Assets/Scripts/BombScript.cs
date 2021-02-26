using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Animator animator;
    public CircleCollider2D circleCollider;

    public bool startCountDown;
    double tickDuration = 0;
    double tickRequiredToChange = 0.2;

    private float regularColliderSize = 0.35f;
    private float expandedColliderSize = 0.7537768f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        startCountDown = false;
        animator.SetFloat("Countdown", 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Since we're gonna do collision detection locally, we have to get the synced countdown value and then check when to increase the explosive radius
        if (animator.GetFloat("Countdown") >= 10)
        {
            circleCollider.enabled = !circleCollider.enabled;
            circleCollider.radius = expandedColliderSize;
            circleCollider.isTrigger = true;
        }

        if (!GetComponent<PhotonView>().IsMine)
            return;

        if (startCountDown)
        {
            tickDuration += Time.deltaTime;

            if (tickDuration >= tickRequiredToChange)
            {
                tickDuration = 0;
                animator.SetFloat("Countdown", animator.GetFloat("Countdown") + 1);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Bomb_Despawn"))
                PhotonNetwork.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        // Only during the explosion state, we will check for explosion
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion_Idle"))
        {
            // If the gameobject is a networked object and we are able to control the gameobject that's inside the explosion radius, we will then destroy it
            if (collision.gameObject.GetComponent<PhotonView>() && collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                if (collision.gameObject.CompareTag("Objects"))
                {
                    if (collision.gameObject.GetComponent<ObjectData>().object_type == OBJECT_TYPE.BREAKABLEBLOCK)
                        PhotonNetwork.Destroy(collision.gameObject);
                }
            }
        }
    }
}
