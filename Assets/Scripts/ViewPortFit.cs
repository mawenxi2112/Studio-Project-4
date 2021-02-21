using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPortFit : MonoBehaviour
{
    public Camera camera;
    // Start is called before the first frame update

    Vector3 v3ViewPort;
    Vector3 v3BottomLeft;
    Vector3 v3TopRight;
    void Start()
    {
        if (camera == null)
            camera = Camera.main;

        /*       float distance = Vector3.Distance(this.transform.position, camera.transform.position);
               v3ViewPort.Set(0, 0, distance);
               v3BottomLeft = Camera.main.ViewportToWorldPoint(v3ViewPort);
               v3ViewPort.Set(1, 1, distance);
               v3TopRight = Camera.main.ViewportToWorldPoint(v3ViewPort);
               Vector3 scale = v3TopRight - v3BottomLeft;
               scale.z = transform.localScale.z;
               transform.localScale = scale;*/
        float distance = Vector3.Distance(this.transform.position, camera.transform.position);
        float y = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
        float x = y * Screen.width / Screen.height;
        this.transform.localScale.Set(x, y, 1.0f);
        /*

                transform.localScale = new Vector3(v3BottomLeft.x - v3TopRight.x, v3BottomLeft.y - v3TopRight.y, goDepth);*/
    }

    // Update is called once per frame

}
