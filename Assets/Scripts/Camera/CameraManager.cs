using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // Cinemachine Virtual Camera
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner cameraConfiner;

    // Get the Polygon Collider for the Camera
    private PolygonCollider2D cameraBounds;

    public Vector2[] cameraPath = new Vector2[4];

    // Start is called before the first frame update
    void Start()
    {

        List<GameObject> rootGameObjects = SceneGameObjects.GetRootGameObjects();

        GameObject virtualCameraGO = null;

        for (int i = 0; i < rootGameObjects.Count; i++)
        {
            if (rootGameObjects[i].name.Equals("Cinemachine Camera"))
            {
                virtualCameraGO = rootGameObjects[i];
            }

            if (rootGameObjects[i].name.Equals("Grid"))
            {
                cameraBounds = rootGameObjects[i].GetComponent<PolygonCollider2D>();
            }
        }

        Debug.Log(cameraBounds.offset.x + " x " + cameraBounds.offset.y);

        cameraBounds.SetPath(0, cameraPath);

        if (virtualCameraGO != null)
            Debug.Log(virtualCameraGO.name);
        
        virtualCamera = virtualCameraGO.GetComponent<CinemachineVirtualCamera>();

        // Add the Confiner Extention for the Camera
        virtualCamera.AddExtension(cameraConfiner);
    }

    // Update is called once per frame
    void Update()
    {
        // Add zoom in option for the CinemachineCmaera
    }
}
