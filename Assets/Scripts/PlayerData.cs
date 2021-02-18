using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int m_maxHealth;
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

    void Start()
    {
        // Network instatiate sword;

        // My client side rn
        // Player (Right now client) - > sword network instantiatel
        // the one below is ignored
        // Player (Other person Client) -> sword network instaiate 
        
        // My client side rn
        // Player (Right now client) - > sword network instantiatel
        // Player (Other person Client) -> sword network instaiate
    }

    // Update is called once per frame
    void Update()
    {
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
/*        if (stream.IsWriting)
        {
            // If this current stream is our player
            stream.SendNext(GetComponent<PlayerInteraction>().m_hand);
            stream.SendNext(GetComponent<PlayerInteraction>().m_sword);
        }
        else
        {
            // Network player
            this.GetComponent<PlayerInteraction>().m_hand = (GameObject)stream.ReceiveNext();
            this.GetComponent<PlayerInteraction>().m_sword = (GameObject)stream.ReceiveNext();
        }*/
    }

    [PunRPC]
    void SetSwordReference(int otherPlayerView, int otherPlayerSwordView)
    {
        if (GetComponent<PhotonView>().ViewID == otherPlayerView)
        {
            GetComponent<PlayerInteraction>().m_sword = PhotonView.Find(otherPlayerSwordView).gameObject;
            GetComponent<PlayerInteraction>().m_hand = PhotonView.Find(otherPlayerSwordView).gameObject;
        }
    }

    #endregion
}
