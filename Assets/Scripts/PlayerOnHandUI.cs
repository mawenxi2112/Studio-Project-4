using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerOnHandUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] sprites;
    public PlayerData player;
    public Image currImage;
    void Start()
    {
      
  
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        if ((int)player.m_currentEquipment == 0)
            currImage.enabled = false;
        else
            currImage.enabled = true;
        currImage.sprite = sprites[(int)player.m_currentEquipment];
           
    }
}
