using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopupText : MonoBehaviour
{
    // Start is called before the first frame update
    public bool pausedOnce;
    public PlayerMenuMovement player;
    public PopUpMenu popUp;
    void Start()
    {
        pausedOnce = false;
        
        popUp.gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
  /*      if (pausedOnce)
        {
            Debug.Log("Returned");
            return;
        }
        if (!pausedOnce)
            pausedOnce = true;*/


        Debug.Log("SetAlready "  + popUp.name);
        player.isPaused = true;

        popUp.gameObject.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        pausedOnce = false;
    }
}
