using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpExit : MonoBehaviour
{
    // Start is called before the first frame update
    public PopUpMenu popupMenu;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void DisablePopup()
    {
        
        Debug.Log("Settings Inactive");
        popupMenu.gameObject.SetActive(false);
        player.GetComponent<PlayerData>().m_isPaused = false;
    }
}
