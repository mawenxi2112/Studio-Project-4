using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Camera camera;

    public GameObject m_weapon;
    public Vector3 ScreenToWorldPos;
    public Vector3 m_dir;

    // Start is called before the first frame update
    void Start()
    {
        // Later on for multiplayer, set the camera if the photonView is mine.
        camera = Camera.main;
        //  ScreenToWorldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_weapon.GetComponent<Transform>().position = new Vector2(transform.position.x, transform.position.y) + Vector2.ClampMagnitude(new Vector2(ScreenToWorldPos.x, ScreenToWorldPos.y), GetComponent<PlayerData>().m_weaponRadius);

    }

    void FixedUpdate()
    {
        //m_dir = (ScreenToWorldPos - transform.position).normalized;
    }

    void OnGUI()
    {
        Event currentEvent = Event.current;
        Vector3 mousePos = Input.mousePosition;

        // Get the mouse position from Event;
        // Note that the y position from Event is inverted;

        ScreenToWorldPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));



        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + camera.pixelWidth + ":" + camera.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + ScreenToWorldPos.ToString("F3"));
        GUILayout.EndArea();
    }
}
