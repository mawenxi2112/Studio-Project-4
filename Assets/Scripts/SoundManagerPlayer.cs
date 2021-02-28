using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayButtonHitSound()
    {
        SceneSoundManager.Instance.PlayMenuClickSound();
    }
    public void PlayButtonBackSound()
    {
        SceneSoundManager.Instance.PlayMenuBackSound();
    }
    public void PlayStairsBackSound()
    {
        SceneSoundManager.Instance.PlayStairsBackSound();
    }
    public void PlayFootsSteps()
    {
        SceneSoundManager.Instance.PlayFootstepsSound();
    }

}
