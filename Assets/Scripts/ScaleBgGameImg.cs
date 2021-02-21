using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBgGameImg : MonoBehaviour
{
    
    public Camera mainCam;
    public GameObject worldScale;
    // Start is called before the first frame update
    void Start()
    {
        scaleBackgroundImageToFit();
    }
    private void scaleBackgroundImageToFit()
    {
        Vector2 deviceScreenResolution = new Vector2(Screen.width, Screen.height);
        mainCam.transform.localPosition = new Vector3(0,0,-8.5f);
        float srcHeight = Screen.height;
        float srcWidth = Screen.width;

        float AspectRatio = srcWidth / srcHeight;

        mainCam.aspect = AspectRatio;

        float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
        float camWidth = camHeight * AspectRatio;
   
        Debug.Log("CamWidth: " + camWidth.ToString());
        Debug.Log("CamHeight: " + camHeight.ToString());
        Debug.Log("AspectRatio: " + AspectRatio.ToString());
        SpriteRenderer ImageSR = this.GetComponent<SpriteRenderer>();
        float bgImgH = ImageSR.sprite.rect.height;
        float bgImgW = ImageSR.sprite.rect.width;
        Debug.Log("bgWidth: " + bgImgW.ToString());
        Debug.Log("bgHeight: " + bgImgH.ToString());
        float bgImg_scale_ratio_height = camHeight / bgImgH;
        float bgImg_scale_ratio_width = camWidth / bgImgW;
        worldScale.transform.localScale =new Vector3(bgImg_scale_ratio_width, bgImg_scale_ratio_height, 1);
        // Debug.Log("BGWidth: " + bgImg_scale_ratio_width.ToString());
    }
    // Update is called once per frame

}
