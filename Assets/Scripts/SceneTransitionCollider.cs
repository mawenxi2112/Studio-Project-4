using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public LoadScene sceneManager;
    public PlayerMenuMovement player;
    public PopUpMenu popupMenu;
    public int ID;
    public bool pausedOnce;
    public ShopData shopValue;
    void Start()
    {
        pausedOnce = false;
    }

    // Update is called once per frame
    void OnTriggerExit2D(Collider2D other)
    {
        pausedOnce = false;
    }
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (pausedOnce)
            return;
        if (!pausedOnce)
            pausedOnce = true;

  
        Debug.Log("Colliding with?" + this.name);
        switch (ID)
        {
            case -1:
                player.isPaused = true;
                Debug.Log("Active: " + popupMenu.gameObject.activeSelf);
                if (popupMenu)
                    popupMenu.PopUp();
                if (!popupMenu.gameObject.activeSelf)
                {
                    popupMenu.gameObject.SetActive(true);
                    Debug.Log("Settings to Active");
                }
                Debug.Log("ActiveTest: " + popupMenu.gameObject.activeSelf);
                Debug.Log("Pausing");
                break;
            case 0:
                
                sceneManager.LoadMainMenu();
           
                Debug.Log("Moving to Main Menu");
                break;
            case 1:
            
                sceneManager.LoadPlayMenu();
           
                break;
            case 2:
                sceneManager.LoadSettingsMenu();
                break;
            case 3:
                sceneManager.LoadSingleplayerMenu();
                break;
            case 4:
                sceneManager.LoadMultiplayerMenu();
                break;
            case 5:
                sceneManager.LoadShopMenu();
                player.isPaused = true;
                break;
        }
    }
    void Update()
    {

    }

  /*  public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colliding");
        switch(ID)
        {
            case 0:
                sceneManager.LoadMainMenu();
                Debug.Log("Moving to Main Menu");
                break;
            case 1:
                sceneManager.LoadPlayMenu();
                break;
            case 2:
                sceneManager.LoadSettingsMenu();
                break;
            case 3:
                sceneManager.LoadSingleplayerMenu();
                break;
            case 4:
                sceneManager.LoadMultiplayerMenu();
                break;
            case 5:
                sceneManager.LoadShopMenu();
                break;
        }
    }*/
}
