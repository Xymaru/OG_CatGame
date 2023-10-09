using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public interface INetChat
    {
        public void SendMessage(string message);
    }
}