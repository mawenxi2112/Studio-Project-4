using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
 
    void Start()
    {
        SceneData.previousScene = SceneData.currentScene;
        SceneData.currentScene = SceneManager.GetActiveScene().name;
        
    }
    public IEnumerator LoadMainLevelAsync(AsyncOperation async)
    {
      
        async = SceneManager.LoadSceneAsync("Level1Scene", LoadSceneMode.Single);
        while(!async.isDone)
        {
        yield return null;

        }
    }

    // Update is called once per frame

    public void LoadMainMenu()
    {
    
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "MainMenu";
        SceneManager.LoadScene("MainMenu");
        
     
    }
    public void LoadPlayMenu()
    {
        PlayerStorage.Save();
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "PlayMenu";
        SceneManager.LoadScene("PlayMenu");
    }
    public void LoadSettingsMenu()
    {
   
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "Settings";
        SceneManager.LoadScene("Settings");
    }
    public void LoadSingleplayerMenu()
    {
  
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "SingleplayerMenu";
        SceneManager.LoadScene("SingleplayerMenu");
    }
    public void LoadMultiplayerMenu()
    {
      
        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "MultiplayerMenu";
        SceneManager.LoadScene("MultiplayerMenu");
    }
    public void LoadShopMenu()
    {

        SceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneData.currentScene = "Shop";
        SceneManager.LoadScene("Shop");
    }
}
