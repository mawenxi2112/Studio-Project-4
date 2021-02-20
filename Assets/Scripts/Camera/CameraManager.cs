using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // Cinemachine Virtual Camera
    private GameObject virtualCameraGO;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner cameraConfiner;

    private Transform cameraFollow;

    // 
    [SerializeField]
    private float freecamSpeed = 1000.0f;

    [SerializeField]
    private bool freecam = false;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> rootGameObjects = SceneGameObjects.GetRootGameObjects();

        virtualCameraGO = SceneGameObjects.FindGameObjectWithName(ref rootGameObjects, "Cinemachine Camera");
        
        virtualCamera = virtualCameraGO.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        // Add zoom in option for the CinemachineCmaera


        if (Input.GetKeyDown(KeyCode.F))
        {
            freecam = !freecam;

            if (freecam)
            {
                cameraFollow = virtualCamera.Follow;

                virtualCamera.Follow = null;
            }
            else
            {
                virtualCamera.Follow = cameraFollow;
            }
        }

        if (freecam)
        {
/*            if (Input.GetKey(KeyCode.I))
            {
                Debug.Log("Camera Position: " + virtualCameraGO.transform.position);

                virtualCameraGO.transform.position.Set(100.0f, virtualCameraGO.transform.position.y, virtualCameraGO.transform.position.z);
            }

            if (Input.GetKey(KeyCode.K))
            {
                virtualCameraGO.transform.position.Set(virtualCameraGO.transform.position.x, virtualCameraGO.transform.position.y + freecamSpeed * dt, virtualCameraGO.transform.position.z);
            }

            if (Input.GetKey(KeyCode.J))
            {
                virtualCameraGO.transform.position.Set(virtualCameraGO.transform.position.x, virtualCameraGO.transform.position.y - freecamSpeed * dt, virtualCameraGO.transform.position.z);
            }

            if (Input.GetKey(KeyCode.L))
            {
                virtualCameraGO.transform.position.Set(virtualCameraGO.transform.position.x - freecamSpeed * dt, virtualCameraGO.transform.position.y, virtualCameraGO.transform.position.z);
            }*/
        }
    }
}
