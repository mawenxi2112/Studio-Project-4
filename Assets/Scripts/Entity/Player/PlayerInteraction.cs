using Photon.Pun;
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

        if (!GetComponent<PhotonView>().IsMine)
            return;

        // If action key is pressed while not holding the sword, it will drop the current held item if there isn't any nearby pickable/interactable gameobjects
        // Throwaway current item mechanic
        if (GetComponent<PlayerData>().m_actionKey && !areTherePossibleMechanics() && GetComponent<PlayerData>().m_currentEquipment != EQUIPMENT.SWORD && GetComponent<PlayerData>().m_currentEquipment != EQUIPMENT.NONE)
        {
            //EquipSword();
            GetComponent<PhotonView>().RPC("EquipSword", RpcTarget.All);
            GetComponent<PlayerData>().m_actionKey = false;
        }

        switch (GetComponent<PlayerData>().m_currentEquipment)
        {
            case EQUIPMENT.SWORD:
                // Rotate weapon accordingly to m_dir
                m_hand.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(m_dir.y, m_dir.x) * Mathf.Rad2Deg);
                if (!m_handAnimator)
                    m_handAnimator = m_hand.GetComponent<Animator>();

                if (GetComponent<PlayerData>().m_actionKey && !areTherePossibleMechanics()) // Add trigger mode for mobile as well.
                {
                    m_handAnimator.SetBool("Attack", true);
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

    public void Throw(GameObject gameObject, Vector3 dir, float force)
    {
        // Currently held equipment (The one that is about to be dropped) switch their tags back to objects since it's going to go back to being a world object
        if (GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.TORCH ||
            GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.KEY ||
            GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.BOMB)
            gameObject.tag = "Objects";

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 99;

        if (GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.BOMB)
            gameObject.GetComponent<BombScript>().startCountDown = true;

        // Re-enable the collision between the old held object and player, by using coroutines we can delay the action
        StartCoroutine(IgnoreCollisionWithTag(m_hand, "All", false, 0f));
        StartCoroutine(IgnoreCollisionWithTag(m_hand, "Player", false, 0.1f));

        if (GetComponent<PhotonView>().IsMine)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x, dir.y).normalized * force, ForceMode2D.Impulse);
            if (gameObject.GetComponent<EntityBounce>() != null)
                gameObject.GetComponent<EntityBounce>().StartBounce(6, true);
        }
    }

    public void EquipSword()
    {
        Throw(m_hand, m_dir, 10);
        GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
        m_hand = m_sword;
        m_sword.SetActive(true);
    }

    bool areTherePossibleMechanics()
    {
        GameObject[] worldObjects = GameObject.FindGameObjectsWithTag("Objects");

        for (int i = 0; i < worldObjects.Length; i++)
        {
            if (transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>().IsTouching(worldObjects[i].GetComponent<Collider2D>()))
            {
                // If touching objects that are interactable/pickable
                if (worldObjects[i].GetComponent<ObjectData>().object_type == OBJECT_TYPE.BOMB ||
                    worldObjects[i].GetComponent<ObjectData>().object_type == OBJECT_TYPE.KEY ||
                    worldObjects[i].GetComponent<ObjectData>().object_type == OBJECT_TYPE.TORCH)
                    return true;

                if (GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.TORCH && worldObjects[i].GetComponent<ObjectData>().object_type == OBJECT_TYPE.CAMPFIRE)
                    return true;

                if (GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.KEY && worldObjects[i].GetComponent<ObjectData>().object_type == OBJECT_TYPE.DOOR)
                    return true;
            }
        }

        return false;
    }

    IEnumerator IgnoreCollisionWithTag(GameObject gameObject, string tag, bool ignore, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

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
        if (!GetComponent<PhotonView>().IsMine || GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.NONE)
            return;

        if (GetComponent<PlayerData>().platform == 0)
        {
            // These are for PC control!
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
        else if (GetComponent<PlayerData>().platform == 1)
        {
            // Get the direction of the joystick
            m_dir = GetComponent<PlayerData>().m_attackJoystick.Direction;
            // Possibly rework this! 
            Vector2 scaledRelativePosition = new Vector2(m_dir.x, m_dir.y) * GetComponent<PlayerData>().m_weaponRadius;

            // Update the weapon's world position by adding the newly calculated relative position back onto the player position
            m_hand.GetComponent<Transform>().position = scaledRelativePosition + new Vector2(transform.position.x, transform.position.y);
        }
    }

    [PunRPC]
    public void PickUp(int pickUpGameObjectViewID, EQUIPMENT equipmentType, PhotonMessageInfo info)
    {
        // If the gameobject isn't the one who is throwing the pickup event
        if (GetComponent<PhotonView>().ViewID != info.photonView.ViewID)
            return;

        GameObject pickUpGameObject = PhotonView.Find(pickUpGameObjectViewID).gameObject;

        // If the owner of the gameobject is not the same as the pickup player, then we change the ownership
        PlayerData.TransferOwner(gameObject, pickUpGameObject);

        // Temp code to hide sword
        if (m_hand == m_sword)
            m_sword.SetActive(false);

        // Setting the tag for the gameobject that is going to be picked up next.

        switch (equipmentType)
        {
            case EQUIPMENT.SWORD:
                pickUpGameObject.tag = "Sword";
                break;
            case EQUIPMENT.TORCH:
                pickUpGameObject.tag = "Torch";
                break;
            case EQUIPMENT.KEY:
                pickUpGameObject.tag = "Key";
                break;
            case EQUIPMENT.BOMB:
                pickUpGameObject.tag = "Bomb";
                break;
        }
        pickUpGameObject.GetComponent<SpriteRenderer>().sortingOrder = 101;

        GetComponent<PlayerData>().m_currentEquipment = equipmentType;

        // Only auto throw current held item if the player isn't holding a sword.
        if (m_hand != m_sword)
            Throw(m_hand, new Vector3(0, 0, 0), 0);
        m_hand = pickUpGameObject;
        StartCoroutine(IgnoreCollisionWithTag(m_hand, "All", true, 0f));
        //IgnoreCollisionWithTag(m_hand, "All", true); // Ignore the collision between the held object and every other game objects with collider
        GetComponent<PlayerData>().m_currentEquipment = equipmentType;
    }

    [PunRPC]
    public void EquipSword(PhotonMessageInfo info)
    {
        if (GetComponent<PhotonView>().ViewID != info.photonView.ViewID)
            return;

        Throw(m_hand, m_dir, 10);
        GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
        m_hand = m_sword;
        m_sword.SetActive(true);
    }
}


