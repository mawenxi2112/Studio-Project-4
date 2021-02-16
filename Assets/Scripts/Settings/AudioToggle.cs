using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToggle : MonoBehaviour
{
    private GameSettings gameSettings;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> rootGameObjects = SceneGameObjects.GetRootGameObjects();

        for (int i = 0; i < rootGameObjects.Count; i++)
        {
            if (rootGameObjects[i].name.Equals("Settings Manager"))
            {
                gameSettings = rootGameObjects[i].GetComponent<GameSettings>();
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
