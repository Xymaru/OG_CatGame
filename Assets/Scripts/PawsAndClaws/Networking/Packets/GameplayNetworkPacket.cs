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

        public NPPlayerPos(byte[] data) : base(data)
        {
            p_type = NPacketType.PLAYERPOS;
        }

        public NPPlayerPos()
        {
            p_type = NPacketType.PLAYERPOS;
        }

        public override NetworkPacket LoadByteArray(byte[] data)
        {
            int offset = 0;

            offset = readBasePacketData(data);

            x = BitConverter.ToSingle(data, offset);
            offset += 4;

            y = BitConverter.ToSingle(data, offset);
            offset += 4;

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            // Set type and size
            int index = setBasePacketData(data);

            BitConverter.GetBytes(x).CopyTo(data, index);
            index += 4;

            BitConverter.GetBytes(y).CopyTo(data, index);
            index += 4;

            p_size = index;
            BitConverter.GetBytes(p_size).CopyTo(data, 0);

            return data;
        }
    }
}
