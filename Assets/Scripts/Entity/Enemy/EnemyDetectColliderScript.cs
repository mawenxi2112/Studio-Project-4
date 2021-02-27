using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectColliderScript : MonoBehaviour
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

            if (collision.gameObject.GetComponent<PlayerData>().m_currentHealth <= 0)
                return;

            gameObject.transform.parent.GetComponent<Animator>().SetBool("IsAttacking", true);
            
        }
    }
}
