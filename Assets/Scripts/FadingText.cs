using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FadingText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text text;
    bool m_Fading;
    float timer;
    // Update is called once per frame
    void Start()
    {
        text = GetComponent<TMP_Text>();
        timer = 2.0f;
        m_Fading = false;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 2.0f;
            m_Fading = !m_Fading;
        }
        if (m_Fading == true)
        {
            //Fully fade in Image (1) with the duration of 2
            text.CrossFadeAlpha(1, 1.0f, false);
        }
        //If the toggle is false, fade out to nothing (0) the Image with a duration of 2
        if (m_Fading == false)
        {
           text.CrossFadeAlpha(0, 1.0f, false);
        }
  
    }

/*    void OnGUI()
    {
        //Fetch the Toggle's state
        if (text.alpha == 0)
            m_Fading = false;
        if (text.alpha == 1)
            m_Fading = true;
      
    }*/
}
