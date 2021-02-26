using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneBackground : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerData player;
    public SpriteRenderer spriteRenderer;
    public Sprite mobileSprite;
    public Sprite PCSprite;
    void Start()
    {
 
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            spriteRenderer.sprite = PCSprite;
        else if (Application.platform == RuntimePlatform.Android)
            spriteRenderer.sprite = mobileSprite;
    }


}
