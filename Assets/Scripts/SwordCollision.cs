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

        collider.gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
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
