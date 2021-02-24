using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<PlayerData>().m_iFrame)
            {
                //Debug.Log("ENEMY HIT PLAYER");
                collision.gameObject.GetComponent<PlayerData>().SetCurrentHealth(collision.gameObject.GetComponent<PlayerData>().m_currentHealth - 1);
                collision.gameObject.GetComponent<PlayerData>().m_iFrame = true;
            }
        }
    }
}
