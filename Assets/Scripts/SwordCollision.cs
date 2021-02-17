using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("TRIGGER ENTER!");

        if (collider.gameObject.CompareTag("Player"))
            return;

        if (collider.gameObject.CompareTag("Objects"))
		{
            switch(collider.gameObject.GetComponent<ObjectData>().object_type)
			{
                case OBJECT_TYPE.COIN:
                    break;

                case OBJECT_TYPE.HEALTHPACK:
                    break;

                case OBJECT_TYPE.KEY:
                    break;

                case OBJECT_TYPE.TORCH:
                    break;

                case OBJECT_TYPE.BOMB:
                    break;

                case OBJECT_TYPE.CHEST:
                    collider.gameObject.GetComponent<ObjectData>().blockHealth--;
                    break;

                case OBJECT_TYPE.SPIKE:
                    break;

                case OBJECT_TYPE.MOVEABLEBLOCK:
                    break;

                case OBJECT_TYPE.CAMPFIRE:
                    break;

                case OBJECT_TYPE.BREAKABLEBLOCK:
                    collider.gameObject.GetComponent<ObjectData>().blockHealth--;
                    break;

                case OBJECT_TYPE.SURPRISETRAPBLOCK:
                    collider.gameObject.GetComponent<ObjectData>().blockHealth--;
                    break;
            }
		}
    }

    void OnTriggerStay2D(Collider2D collider)
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;
    }

    public void EnableSwordCollision()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void DisableSwordCollision()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}
