using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneData : MonoBehaviour
{
    // Start is called before the first frame update
   public static string previousScene;
    public static string currentScene;
    public static int playerCurrency;
    void Start()
    {
      
        GameObject[] array = GameObject.FindGameObjectsWithTag("DataManager");
        for(int i = 0;i != array.Length;i++)
        {
            if(i == (array.Length -1))
            {
                if (currentScene == null)
                {
                    currentScene = SceneManager.GetActiveScene().name;
                }
               
                break;

            }
            Destroy(array[i]);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
       
    }

}
