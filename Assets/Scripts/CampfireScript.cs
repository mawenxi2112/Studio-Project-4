using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsLit", false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
