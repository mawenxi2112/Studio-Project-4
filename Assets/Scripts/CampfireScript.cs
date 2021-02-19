using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    public Animator animator;
    public bool IsLit;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        IsLit = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsLit", IsLit);
    }
}
