using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    [System.Serializable]
    public enum PacketType
    {
        Text,
        Max
    }
    [System.Serializable]
    public abstract class NetPacket
    {
        public int type;
        public int size;

        public abstract byte[] ToPacketBytes();
    }
    [System.Serializable]
    public class NetPacketText : NetPacket
    {
        public string text;

        public NetPacketText(string text)
        {
            type = (int)PacketType.Text;
            this.text = text;
            size = text.Length;
        }

        public override byte[] ToPacketBytes()
        {
            byte[] packetdata = Utils.BinaryUtils.ObjectToByteArray(this);

            byte[] totalData = new byte[packetdata.Length + 4];

            BitConverter.GetBytes(packetdata.Length).CopyTo(totalData, 0);
            packetdata.CopyTo(totalData, 4);

            return totalData;
        }
    }
}