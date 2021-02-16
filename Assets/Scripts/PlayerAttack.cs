using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Camera camera;

    public GameObject m_weapon;
    public Animator weaponAnimator;
    public Vector3 ScreenToWorldPos;
    public Vector2 m_dir;

    // Start is called before the first frame update
    void Start()
    {
        // Later on for multiplayer, set the camera if the photonView is mine.
        camera = Camera.main;
        
        if (GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.SWORD)
        {
            weaponAnimator = m_weapon.GetComponent<Animator>();
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
        m_weapon.GetComponent<Transform>().position = clampedRelativePosition + new Vector2(transform.position.x, transform.position.y);

        // Rotate weapon accordingly to m_dir
        m_weapon.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, Mathf.Atan2(m_dir.y, m_dir.x) * Mathf.Rad2Deg);

        if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sword_Swing"))
        {
            m_weapon.GetComponent<PolygonCollider2D>().enabled = true;
        }
        else
        {
            m_weapon.GetComponent<PolygonCollider2D>().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.SWORD) // Add attack speed
        {
            weaponAnimator.SetTrigger("Attack");    
            // Attack checking enemies code.
        }
        
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        ScreenToWorldPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
    }
}
