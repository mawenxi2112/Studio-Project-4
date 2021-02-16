using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private GameSettings gameSettings;
    
    private Slider slider;

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

        slider = GetComponent<Slider>();

        if (gameSettings !=  null)
        {
            slider.minValue = 0;
            slider.maxValue = 100;
            slider.wholeNumbers = true;

            if (gameObject.name.Contains("Main"))
            {
                slider.value = gameSettings.MasterVolume;
            }
            else if (gameObject.name.Contains("BGM"))
            {
                slider.value = gameSettings.BGMVolume;
            }
            else if (gameObject.name.Contains("FX"))
            {
                slider.value = gameSettings.SoundFXVolume;
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
                gameSettings.MasterVolume = (int)slider.value;
            }
            else if (gameObject.name.Contains("BGM"))
            {
                gameSettings.BGMVolume = (int)slider.value;
            }
            else if (gameObject.name.Contains("FX"))
            {
                gameSettings.SoundFXVolume = (int)slider.value;
            }
        }
    }
}
