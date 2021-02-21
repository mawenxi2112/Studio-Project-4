using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun
{
    public class ConnectToMaster : MonoBehaviour
    {
        // Start is called before the first frame update
       
        public void PlayerConnectToMaster()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }
}
