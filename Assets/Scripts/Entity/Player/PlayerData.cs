using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EQUIPMENT
{
    NONE,
    SWORD,
    TORCH,
    KEY,
    BOMB
}

public class PlayerData : MonoBehaviourPunCallbacks, IPunObservable
{
    public int m_currentHealth;
    public  int m_maxHealth;
    public float m_currentMoveSpeed;
    public float m_maxMoveSpeed;
    public bool m_iFrame;
    public double m_iFrameCounter;
    public double m_iFrameThreshold;
    public float m_weaponRadius;
    public int m_currency;
    public float m_dashSpeed;
    public EQUIPMENT m_currentEquipment;
    public bool m_actionKey;
    public double m_actionKeyTimer;
    public double m_actionKeyReset;
    public int m_currentAttack;
    public bool m_isPaused;
    // Used in mobile platform
    public Button m_dashButton;
    public Joystick m_movementJoystick;
    public Joystick m_attackJoystick;
    public bool m_OnAttackJoystickDown;
    // Temporary using this to seperate the different platforms.
    public int platform = 0; // 0 = PC, 1 = ANDROID

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PhotonView>()) // Online
        {
            if (!GetComponent<PhotonView>().IsMine)
                return;

            PlayerStorage.Load();
            m_isPaused = false;
            m_currency = PlayerStorage.coins;
            m_maxHealth = PlayerStorage.maxHealth;
            m_maxMoveSpeed = PlayerStorage.speed;
            m_currentAttack = PlayerStorage.damage;

            if (!m_movementJoystick)
                m_movementJoystick = GameObject.Find("Movement Joystick").GetComponent<Joystick>();

            if (!m_attackJoystick)
                m_attackJoystick = GameObject.Find("Attack Joystick").GetComponent<Joystick>();

            if (!m_dashButton)
                m_dashButton = GameObject.Find("Dash").GetComponent<Button>();

            if (platform == 0) // If on pc platform, hide the joystick
            {
                m_movementJoystick.gameObject.SetActive(false);
                m_attackJoystick.gameObject.SetActive(false);
                m_dashButton.gameObject.SetActive(false);
            }
        }
        else // Offline
        {
            PlayerStorage.Load();

            m_currency = PlayerStorage.coins;
            m_maxHealth = PlayerStorage.maxHealth;
            m_maxMoveSpeed = PlayerStorage.speed;
            m_currentAttack = PlayerStorage.damage;

            if (!m_movementJoystick)
                m_movementJoystick = GameObject.Find("Movement Joystick").GetComponent<Joystick>();

            if (!m_attackJoystick)
                m_attackJoystick = GameObject.Find("Attack Joystick").GetComponent<Joystick>();

            if (!m_dashButton)
                m_dashButton = GameObject.Find("Dash").GetComponent<Button>();

            if (platform == 0) // If on pc platform, hide the joystick
            {
                m_movementJoystick.gameObject.SetActive(false);
                m_attackJoystick.gameObject.SetActive(false);
                m_dashButton.gameObject.SetActive(false);
            }
        }

        m_currentHealth = m_maxHealth;
        m_currentMoveSpeed = m_maxMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>()) // Online
        {
            if (!GetComponent<PhotonView>().IsMine)
                return;

            // If player dies
            if (m_currentHealth <= 0)
            {
                GetComponent<Animator>().SetBool("isDead", true);

                if (m_currentEquipment != EQUIPMENT.NONE)
                    GetComponent<PhotonView>().RPC("DeadHand", RpcTarget.All);

                return;
            }
            else
            {
                GetComponent<Animator>().SetBool("isDead", false);
            }

            m_actionKeyTimer += Time.deltaTime;

            if (m_iFrame)
            {
                // Render a different colour during iFrame
                m_actionKey = false;
                m_iFrameCounter += Time.deltaTime;

                if (m_iFrameCounter >= m_iFrameThreshold)
                {
                    m_iFrame = false;
                    m_iFrameCounter = 0f;
                }
            }

            if (platform == 0)
            {
                if (Input.GetKey(KeyCode.Mouse0) && m_actionKeyTimer >= m_actionKeyReset)
                {
                    m_actionKeyTimer = 0;
                    m_actionKey = true;
                }

                if (!Input.GetKey(KeyCode.Mouse0))
                {
                    m_actionKey = false;
                }
            }
            else if (platform == 1)
            {
                Vector2 attackDir = m_attackJoystick.Direction;
                if (Input.GetKey(KeyCode.Escape))
                {
                    m_isPaused = !m_isPaused;
                }
                if (m_currentEquipment == EQUIPMENT.SWORD)
                {
                    if (attackDir.magnitude > 0 && m_actionKeyTimer >= m_actionKeyReset)
                    {
                        m_actionKeyTimer = 0;
                        m_actionKey = true;
                    }

                    if (attackDir.magnitude == 0)
                    {
                        m_actionKey = false;
                    }
                }
                else if (m_currentEquipment != EQUIPMENT.NONE)
                {
                    // When attack joystick is moving
                    if (m_attackJoystick.isDown)
                    {
                        m_OnAttackJoystickDown = true;
                    }
                    else if (!m_attackJoystick.isDown && m_OnAttackJoystickDown) // When attack joystick is not moving and joystick was previously being held
                    {
                        m_actionKey = true;
                        m_OnAttackJoystickDown = false;
                    }
                    else if (!m_attackJoystick.isDown && !m_OnAttackJoystickDown) // When attack joystick is not moving and joystick wasn't previously being held
                    {
                        // Nothing happens (usual joystick idle state)
                    }
                }
                else // If nothing is held
                {

                }
            }
        }
    }

    public void SetCurrentHealth(int value)
	{
        m_currentHealth = value;
	}

    public void SetCurrentMoveSpeed(float value)
	{
        m_currentMoveSpeed = value;
	}

    public void SetCurrency(int value)
    {
        m_currency = value;
    }

    public static void TransferOwner(GameObject newOwner, GameObject gameobject)
    {
        if (newOwner.GetComponent<PhotonView>().Controller != gameobject.GetComponent<PhotonView>().Controller)
            gameobject.GetComponent<PhotonView>().TransferOwnership(newOwner.GetComponent<PhotonView>().Controller);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // If this current stream is our player
            stream.SendNext(m_currentHealth);
            stream.SendNext(m_maxHealth);
            stream.SendNext(m_maxMoveSpeed);
        }
        else
        {
            // Network player
            m_currentHealth = (int)stream.ReceiveNext();
            m_maxHealth = (int)stream.ReceiveNext();
            m_maxMoveSpeed = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void SetSwordReference(int otherPlayerSwordView, PhotonMessageInfo info)
    {
        // Only update it if the incoming other playview is the same as one of the networked game objects
        if (GetComponent<PhotonView>().ViewID == info.photonView.ViewID)
        {
            GetComponent<PlayerInteraction>().m_sword = PhotonView.Find(otherPlayerSwordView).gameObject;
            GetComponent<PlayerInteraction>().m_hand = PhotonView.Find(otherPlayerSwordView).gameObject;
            GetComponent<PlayerInteraction>().m_handAnimator = PhotonView.Find(otherPlayerSwordView).gameObject.GetComponent<Animator>();
        }
    }
}
