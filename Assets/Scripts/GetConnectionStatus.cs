using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Photon.Pun
{
    public class GetConnectionStatus : MonoBehaviour
    {
        // Start is called before the first frame update
        public TMP_Text m_textComponent;

        #region UNITY
        // Update is called once per frame
        void Start()
        {
            m_textComponent = GetComponent<TMP_Text>();
        }
        void Update()
        {
            m_textComponent.text = "" + PhotonNetwork.NetworkClientState;
            if (("" + PhotonNetwork.NetworkClientState) == "PeerCreated")
            {
                m_textComponent.text = "Disconnected";
                m_textComponent.color = new Color32(255, 53, 62, 255);
            }
            if (("" + PhotonNetwork.NetworkClientState) == "ConnectedToMasterServer")
            {
                m_textComponent.text = "Connected";
                m_textComponent.color = new Color32(94, 241, 138, 255);
            }
            if (("" + PhotonNetwork.NetworkClientState) == "ConnectedToNameServer")
            {
                m_textComponent.text = "Connected";
                m_textComponent.color = new Color32(94, 241, 138, 255);
            }
            if(m_textComponent.text == "Disconnected" && m_textComponent.color != new Color32(255,53,62,255))
            {
                m_textComponent.color = new Color32(255, 53, 62, 255);
            }
            if (m_textComponent.text == "Disconnected" && m_textComponent.color != new Color32(255, 53, 62, 255))
            {
                m_textComponent.color = new Color32(255, 53, 62, 255);
            }
        }
        #endregion

    }
}
