using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    public Animator animator;

    private GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsLit", false);

        particle = transform.GetChild(0).gameObject;

        particle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("IsLit") == true)
        {
            particle.SetActive(true);
        }
        else if (animator.GetBool("IsLit") == false)
        {
            particle.SetActive(false);
        }
    }
}
