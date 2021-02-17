using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera camera;

    // Gameobject reference of the current held object
    public GameObject m_hand;

    // Gameobject reference of the sword
    public GameObject m_sword;

    // Animator reference of the gameobject of the currently held object
    public Animator m_handAnimator;
    
    // Pos of the mouse in world space
    public Vector3 ScreenToWorldPos;
    public Vector2 m_dir;

    // Start is called before the first frame update
    void Start()
    {
        // Later on for multiplayer, set the camera if the photonView is mine.
        camera = Camera.main;

        switch (GetComponent<PlayerData>().m_currentEquipment)
        {
            case EQUIPMENT.SWORD:
                m_handAnimator = m_hand.GetComponent<Animator>();
                break;

            case EQUIPMENT.KEY:
                break;

            case EQUIPMENT.TORCH:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the relative position of the mouse position to the player
        Vector2 relativePosition = new Vector2(ScreenToWorldPos.x - transform.position.x, ScreenToWorldPos.y - transform.position.y);

        // For future usages
        m_dir = relativePosition.normalized;

        // Clamp the relative position when the mouse position is outside of the weapon radius
        Vector2 clampedRelativePosition = Vector2.ClampMagnitude(relativePosition, GetComponent<PlayerData>().m_weaponRadius);

        // Update the weapon's world position by adding the newly clamped relative position back onto the player position
        m_hand.GetComponent<Transform>().position = clampedRelativePosition + new Vector2(transform.position.x, transform.position.y);



        switch (GetComponent<PlayerData>().m_currentEquipment)
        {
            case EQUIPMENT.SWORD:
                // Temp code to hide sword when another item is being used
                m_sword = m_hand;

                if (m_hand != m_sword)
                    m_hand = m_sword;

                // Rotate weapon accordingly to m_dir
                m_hand.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(m_dir.y, m_dir.x) * Mathf.Rad2Deg);

                if (!m_handAnimator)
                    m_handAnimator = m_hand.GetComponent<Animator>();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                    m_handAnimator.SetTrigger("Attack");
                break;

            case EQUIPMENT.KEY:
                break;

            case EQUIPMENT.TORCH:
                break;

            case EQUIPMENT.BOMB:
                break;

            case EQUIPMENT.NONE:
                break;

        }
    }

    public void PickUp(GameObject gameObject, EQUIPMENT equipment)
    {
        // Temp code to hide sword
        if (m_hand == m_sword)
            m_sword.SetActive(false);

        // Currently held equipment (The one that is about to be dropped) switch their tags back to objects since it's going to go back to being a world object
        if (GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.TORCH ||
            GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.KEY ||
            GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.BOMB)
            m_hand.tag = "Objects";

        // The gameobject that is going to be picked up next.
        switch (equipment)
        {
            case EQUIPMENT.TORCH:
                gameObject.tag = "Torch";
                break;
            case EQUIPMENT.KEY:
                gameObject.tag = "Key";
                break;
            case EQUIPMENT.BOMB:
                gameObject.tag = "Bomb";
                break;
        }
    
        Throw(m_hand, m_hand.transform.position, new Vector3(0, 0, 0), 0);
        m_hand = gameObject;
        GetComponent<PlayerData>().m_currentEquipment = equipment;
    }

    public void Throw(GameObject gameObject, Vector3 startingPos, Vector3 dir, float force)
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(startingPos);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x, dir.y).normalized * force);
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        ScreenToWorldPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
    }
}
