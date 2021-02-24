using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class BossDetectColliderScript : MonoBehaviour
{
    public int attackPattern = 0;

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
            if (attackPattern != 2)
            {
                gameObject.transform.parent.GetComponent<Animator>().SetBool("UseAttackOne", true);
                gameObject.transform.parent.GetComponent<BossData>().m_rechargeDuration = 2f;
                attackPattern++;
            }
			else
			{
                gameObject.transform.parent.GetComponent<Animator>().SetBool("UseAttackTwo", true);
                gameObject.transform.parent.GetComponent<BossData>().m_rechargeDuration = 3f;
                attackPattern = 0;
            }

            gameObject.GetComponent<PolygonCollider2D>().enabled = false;

        }
    }
}
