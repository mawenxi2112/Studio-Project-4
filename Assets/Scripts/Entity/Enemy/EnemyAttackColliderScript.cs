using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackColliderScript : MonoBehaviour
{
    PolygonCollider2D polygoncollider;

    // Start is called before the first frame update
    void Start()
    {
        switch (gameObject.transform.parent.GetComponent<EnemyData>().m_type)
		{
            case ENEMY_TYPE.MELEE:
                polygoncollider = gameObject.GetComponent<PolygonCollider2D>();
                break;

            case ENEMY_TYPE.RANGED:
                break;

            case ENEMY_TYPE.RUNNER:
                break;
        };
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
