using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    void Start()
    {
        if(NetworkData.netSocket.netcon == NetCon.CLIENT)
        {
            gameObject.AddComponent<NetClient>();
        }
        else
        {
            gameObject.AddComponent<NetServer>();
        }
    }
}
