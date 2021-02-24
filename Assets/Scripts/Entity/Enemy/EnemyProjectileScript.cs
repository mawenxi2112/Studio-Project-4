using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EnemyProjectileScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
                collision.gameObject.GetComponent<PlayerData>().SetCurrentHealth(collision.gameObject.GetComponent<PlayerData>().m_currentHealth - 1);
                collision.gameObject.GetComponent<PlayerData>().m_iFrame = true;
            }
            PhotonNetwork.Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Objects"))
		{
            if (collision.gameObject.GetComponent<ObjectData>().object_type == OBJECT_TYPE.BREAKABLEBLOCK ||
                 collision.gameObject.GetComponent<ObjectData>().object_type == OBJECT_TYPE.CHEST ||
                 collision.gameObject.GetComponent<ObjectData>().object_type == OBJECT_TYPE.DOOR ||
                 collision.gameObject.GetComponent<ObjectData>().object_type == OBJECT_TYPE.GATE ||
                 collision.gameObject.GetComponent<ObjectData>().object_type == OBJECT_TYPE.MOVEABLEBLOCK ||
                 collision.gameObject.GetComponent<ObjectData>().object_type == OBJECT_TYPE.SURPRISETRAPBLOCK)
			{
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.name == "WallTiles")
		{
            PhotonNetwork.Destroy(gameObject);
        }
	}
}
