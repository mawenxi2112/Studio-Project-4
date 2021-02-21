using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpExit : MonoBehaviour
{
    // Start is called before the first frame update
    public PopUpMenu popupMenu;
    public PlayerMenuMovement player;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void DisablePopup()
    {
        
        Debug.Log("Settings Inactive");
        popupMenu.gameObject.SetActive(false);
        player.isPaused = false;
    }
}
