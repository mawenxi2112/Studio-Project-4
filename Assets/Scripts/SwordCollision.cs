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
        collider.gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
    }
}
