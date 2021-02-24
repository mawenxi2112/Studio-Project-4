using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION
{ 
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

public class DirectionPadScript : MonoBehaviour
{
    public DIRECTION direction;
    public Vector2 resultantForce;
    public float force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case DIRECTION.UP:
                resultantForce = force * new Vector2(0, 1);
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case DIRECTION.DOWN:
                resultantForce = force * new Vector2(0, -1);
                transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case DIRECTION.LEFT:
                resultantForce = force * new Vector2(-1, 0);
                transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case DIRECTION.RIGHT:
                resultantForce = force * new Vector2(1, 0);
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // When the player comes into contact with the directional pad, we check if the player belongs to their client
        if (collision.CompareTag("Player") && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            // Set Position>??
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(resultantForce, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position);
        }
    }
}
