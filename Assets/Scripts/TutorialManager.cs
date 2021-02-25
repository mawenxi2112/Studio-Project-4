using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Joystick movementJoystick;
    public Joystick attackJoystick;
    public Button dashButton;
    public GameObject sword;
    void Start()
    {
       
        player.GetComponent<PlayerInteraction>().m_sword = player.GetComponent<PlayerInteraction>().m_hand;
        player.GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
        player.GetComponent<PlayerInteraction>().m_hand = sword;
        player.GetComponent<PlayerInteraction>().m_sword = player.GetComponent<PlayerInteraction>().m_hand;
        player.GetComponent<PlayerData>().m_movementJoystick = movementJoystick;
        player.GetComponent<PlayerData>().m_attackJoystick = attackJoystick;
        player.GetComponent<PlayerData>().m_dashButton = dashButton;
        player.GetComponent<PlayerData>().m_isPaused = false;
        player.GetComponent<PlayerData>().m_currentEquipment = EQUIPMENT.SWORD;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
