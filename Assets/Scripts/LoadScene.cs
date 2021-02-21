using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
 
    void Start()
    {
        
    }

    // Update is called once per frame
   
    public void LoadMainMenu()
    {
    
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "MainMenu";
        SceneManager.LoadScene(0);
     
    }
    public void LoadPlayMenu()
    {
       
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "PlayMenu";
        SceneManager.LoadScene(1);
    }
    public void LoadSettingsMenu()
    {
   
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "Settings";
        SceneManager.LoadScene(2);
    }
    public void LoadSingleplayerMenu()
    {
  
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "SingleplayerMenu";
        SceneManager.LoadScene(3);
    }
    public void LoadMultiplayerMenu()
    {
      
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "MultiplayerMenu";
        SceneManager.LoadScene(4);
    }
    public void LoadShopMenu()
    {

        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "Shop";
        SceneManager.LoadScene(5);
    }
}
