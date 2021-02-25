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
       
        if (player.platform == 0)
            spriteRenderer.sprite = PCSprite;
        else if (player.platform == 1)
            spriteRenderer.sprite = mobileSprite;
    }


}
