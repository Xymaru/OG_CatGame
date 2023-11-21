using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;

namespace PawsAndClaws.Networking
{
    [System.Serializable]
    public class NPPlayerPos : ClientNetworkPacket
    {
        public float x;
        public float y;

        public NPPlayerPos()
        {
            p_type = NPacketType.PLAYERPOS;
        }
    }

    public static class GameplayNetworkPacket
    {
        public static byte[] NPPlayerPosToByteArray(NPPlayerPos packet)
        {
            byte[] data = new byte[NetworkPacket.MAX_BUFFER_SIZE];

            // Set type and size
            int index = packet.setBasePacketData(data);

            BitConverter.GetBytes(packet.x).CopyTo(data, index);
            index += 4;

            BitConverter.GetBytes(packet.y).CopyTo(data, index);
            index += 4;

            packet.p_size = index;
            BitConverter.GetBytes(packet.p_size).CopyTo(data, 0);

            return data;
        }

        public static NPPlayerPos PlayerPosToNetworkPacket(byte[] data)
        {
            int offset = 0;
            NPPlayerPos packet = new NPPlayerPos();
            offset = packet.readBasePacketData(data);

            packet.x = BitConverter.ToSingle(data, offset);
            offset += 4;

            packet.y = BitConverter.ToSingle(data, offset);

            return packet;
        }
    }
}
