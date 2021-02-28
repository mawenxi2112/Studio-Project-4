using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public LoadScene sceneManager;
    public GameObject player;
    public PopUpMenu popupMenu;
    public int ID;
    public bool pausedOnce;
    public ShopData shopValue;
    public SoundManagerPlayer soundPlayer;
 
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

  
   
        switch (ID)
        {
            case -1:
                soundPlayer.PlayFootsSteps();
                player.GetComponent<PlayerData>().m_isPaused = true;

                if (popupMenu)
                    popupMenu.PopUp();
                if (!popupMenu.gameObject.activeSelf)
                {
                    popupMenu.gameObject.SetActive(true);
                }
                break;
            case 0:
               
                sceneManager.LoadMainMenu();
           
                break;
            case 1:
                soundPlayer.PlayStairsBackSound();
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
                player.GetComponent<PlayerData>().m_isPaused = true;
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
