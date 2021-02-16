using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    private GameSettings gameSettings;

    private Toggle toggle;

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

        toggle = GetComponent<Toggle>();

        if (gameSettings != null)
        {
            if (gameObject.name.Contains("Main"))
            {
                toggle.isOn = gameSettings.MasterVolumeEnabled;
            }
            else if (gameObject.name.Contains("BGM"))
            {
                toggle.isOn = gameSettings.BGMEnabled;
            }
            else if (gameObject.name.Contains("FX"))
            {
                toggle.isOn = gameSettings.SoundFXEnabled;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSettings != null)
        {
            if (gameObject.name.Contains("Main"))
            {
                gameSettings.MasterVolumeEnabled = toggle.isOn;
            }
            else if (gameObject.name.Contains("BGM"))
            {
                gameSettings.BGMEnabled = toggle.isOn;
            }
            else if (gameObject.name.Contains("FX"))
            {
                gameSettings.SoundFXEnabled = toggle.isOn;
            }
        }
    }
}
