using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
        // These updates are for PC platform!
        // Need to add custom player interactions for mobile etc.

        // If action key is pressed while not holding the sword, it will drop the current held item if there isn't any nearby pickable/interactable gameobjects
        if (GetComponent<PlayerData>().m_actionKey && !touchingAnyWorldObject() && GetComponent<PlayerData>().m_currentEquipment != EQUIPMENT.SWORD)
        {
            EquipSword();
            GetComponent<PlayerData>().m_actionKey = false;
        }

        switch (GetComponent<PlayerData>().m_currentEquipment)
        {
            case EQUIPMENT.SWORD:
                // Rotate weapon accordingly to m_dir
                m_hand.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(m_dir.y, m_dir.x) * Mathf.Rad2Deg);

                if (!m_handAnimator)
                    m_handAnimator = m_hand.GetComponent<Animator>();

                if (GetComponent<PlayerData>().m_actionKey && !touchingAnyWorldObject()) // Add trigger mode for mobile as well.
                {
                    m_handAnimator.SetTrigger("Attack");
                    GetComponent<PlayerData>().m_actionKey = false;
                }
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

        // The gameobject that is going to be picked up next.
        switch (equipment)
        {
            case EQUIPMENT.SWORD:
                gameObject.tag = "Sword";
                break;
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

        IgnoreCollisionWithTag(m_hand, "All", true); // Ignore the collision between the held object and every other game objects with collider
        GetComponent<PlayerData>().m_currentEquipment = equipment;
    }

    public void Throw(GameObject gameObject, Vector3 startingPos, Vector3 dir, float force)
    {
        // Currently held equipment (The one that is about to be dropped) switch their tags back to objects since it's going to go back to being a world object
        if (GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.TORCH ||
            GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.KEY ||
            GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.BOMB)
            gameObject.tag = "Objects";

        IgnoreCollisionWithTag(m_hand, "All", false); ; // Re-enable the collision between the old held object and player
        gameObject.GetComponent<Rigidbody2D>().MovePosition(startingPos);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x, dir.y).normalized * force, ForceMode2D.Impulse);
        if (gameObject.GetComponent<EntityBounce>() != null)
            gameObject.GetComponent<EntityBounce>().StartBounce(6, true);

    }

    public void EquipSword()
    {
        Throw(m_hand, new Vector3(m_hand.transform.position.x, m_hand.transform.position.y, 0), m_dir, 10);
        GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
        m_hand = m_sword;
        m_sword.SetActive(true);
    }

    bool touchingAnyWorldObject()
    {
        GameObject[] worldObjects = GameObject.FindGameObjectsWithTag("Objects");

        for (int i = 0; i < worldObjects.Length; i++)
        {
            if (transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>().IsTouching(worldObjects[i].GetComponent<Collider2D>()))
                return true;
        }

        return false;
    }

    public void IgnoreCollisionWithTag(GameObject gameObject, string tag, bool ignore)
    {
        GameObject[] gameObjectArray;

        if (tag == "All")
            gameObjectArray = FindObjectsOfType<GameObject>();
        else
            gameObjectArray = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < gameObjectArray.Length; i++)
        {
            GameObject tmp = gameObjectArray[i];

            if (tmp.GetComponents<Collider2D>().Length > 0)
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>(), ignore);
        }
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        ScreenToWorldPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

        // Calculate the relative position of the mouse position to the player
        Vector2 relativePosition = new Vector2(ScreenToWorldPos.x - transform.position.x, ScreenToWorldPos.y - transform.position.y);

        // For future usages
        m_dir = relativePosition.normalized;

        // Clamp the relative position when the mouse position is outside of the weapon radius
        Vector2 clampedRelativePosition = Vector2.ClampMagnitude(relativePosition, GetComponent<PlayerData>().m_weaponRadius);

        // Update the weapon's world position by adding the newly clamped relative position back onto the player position
        m_hand.GetComponent<Transform>().position = clampedRelativePosition + new Vector2(transform.position.x, transform.position.y);
    }
}
