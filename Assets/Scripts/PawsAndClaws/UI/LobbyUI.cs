using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;

using TMPro;
using PawsAndClaws.Networking;

namespace PawsAndClaws
{
    public class LobbyUI : MonoBehaviour
    {
        private bool _sentReq = false;

        void Start()
        {
            if(NetworkData.NetSocket.NetCon == NetCon.Client)
            {

            }
            else{
                gameObject.AddComponent<LobbyServer>();
            }
        }

        
    }
}
