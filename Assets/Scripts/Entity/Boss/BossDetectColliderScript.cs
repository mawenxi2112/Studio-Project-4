using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class BossDetectColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check whether is target within collider

            if (!PhotonNetwork.IsMasterClient)
                return;

            // Determine what type of attack is going to be use here (Create pattern, 1,1,2)

            gameObject.transform.parent.GetComponent<Animator>().SetBool("UseAttackOne", true);
            gameObject.transform.parent.GetComponent<BossData>().m_rechargeDuration = 1.5f;

        }
    }
}
