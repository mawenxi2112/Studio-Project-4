using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public SoundAudioClip[] soundAudioClipArray;
    public static SceneSoundManager Instance;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.SoundName name;
        public AudioClip audioClip;
    }

    // Update is called once per frame
    void Awake()
    {

        if (SceneData.currentScene != "Level1Scene")
        {
            SoundManager.Initialise();
        }
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
        DontDestroyOnLoad(this);
    }
    public void PlayDashSound()
    {
        Debug.Log("PlayingSound");
        DontDestroyOnLoad(SoundManager.PlaySoundMenu(SOUNDTYPE.FX, SoundManager.SoundName.DASH));
    }
}
