﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D collider;

    double maxDurationTillNextChange = 0.2;
    public double maxUpDurationToStayFor = 2;
    public double maxDownDurationToStayFor = 2;
    double durationTillNextChange = 0;
    double durationToStayFor = 0;
    bool goingDown = false;
    bool isStaying = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isStaying)
            {
                durationTillNextChange += Time.deltaTime;

                if (durationTillNextChange >= maxDurationTillNextChange)
                {
                    durationTillNextChange = 0;

                    if (goingDown)
                        animator.SetFloat("State", animator.GetFloat("State") - 1);
                    else if (!goingDown)
                        animator.SetFloat("State", animator.GetFloat("State") + 1);


                    if ((int)animator.GetFloat("State") <= 0 || (int)animator.GetFloat("State") == 2)
                    {
                        isStaying = true;
                        goingDown = !goingDown;

                        if ((int)animator.GetFloat("State") == 2)
                            collider.enabled = !collider.enabled;
                    }
                }
            }
            else if (isStaying)
            {
                durationToStayFor += Time.deltaTime;

                if (!goingDown)
                {
                    if (durationToStayFor >= maxDownDurationToStayFor)
                    {
                        isStaying = false;
                        durationToStayFor = 0;

                        if ((int)animator.GetFloat("State") == 2)
                            collider.enabled = !collider.enabled;
                    }
                }
                else if (goingDown)
                {
                    if (durationToStayFor >= maxUpDurationToStayFor)
                    {
                        isStaying = false;
                        durationToStayFor = 0;

                        if ((int)animator.GetFloat("State") == 2)
                            collider.enabled = !collider.enabled;
                    }
                }
            }
        }
        else
        {
            if ((int)animator.GetFloat("State") == 2)
                collider.enabled = true;
            else
                collider.enabled = false;
        }

    }
}
