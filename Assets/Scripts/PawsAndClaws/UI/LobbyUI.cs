using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;

using TMPro;
using PawsAndClaws.Networking;
using PawsAndClaws.Networking.Packets;

namespace PawsAndClaws
{
    public class LobbyUI : MonoBehaviour
    {
        void Start()
        {
            if(NetworkData.NetSocket.NetCon == NetCon.Client)
            {

            }
            else{
                gameObject.AddComponent<LobbyServer>();
            }
        }

        void Update()
        {
            
        }

        public void RequestSpotHamster(int index)
        {
            RequestSpot(index, Player.Team.Hamster);
        }

        public void RequestSpotCat(int index)
        {
            RequestSpot(index, Player.Team.Cat);
        }

        private void RequestSpot(int index, Player.Team team)
        {

        }
    }
}
