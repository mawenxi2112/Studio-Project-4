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
    // Start is called before the first frame update

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
    // Temporary using this to seperate the different platforms.
    public Joystick m_movementJoystick;
    public Joystick m_attackJoystick;
    public Button m_dashButton;
    public int platform = 0; // 0 = PC, 1 = ANDROID
    void Start()
    {
        m_currentAttack = 5;
    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>() == null)
            return;
        if (!GetComponent<PhotonView>().IsMine)
            return;

        m_actionKeyTimer += Time.deltaTime;

        if (m_iFrame)
        {
            // Render a different colour during iFrame

            m_iFrameCounter += Time.deltaTime;

            if (m_iFrameCounter >= m_iFrameThreshold)
            {
                m_iFrame = false;
                m_iFrameCounter = 0f;
            }
        }

        if (platform == 0)
        {
            if (m_movementJoystick != null)
                m_movementJoystick.gameObject.SetActive(false);

            if (m_attackJoystick != null)
                m_attackJoystick.gameObject.SetActive(false);

            if (m_dashButton != null)
                m_dashButton.gameObject.SetActive(false);

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
            m_movementJoystick.gameObject.SetActive(true);
            m_attackJoystick.gameObject.SetActive(true);
            m_dashButton.gameObject.SetActive(true);

            Vector2 attackDir = new Vector2(m_attackJoystick.Horizontal, m_attackJoystick.Vertical);

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

    #region IPunObservable

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

    #endregion
}
