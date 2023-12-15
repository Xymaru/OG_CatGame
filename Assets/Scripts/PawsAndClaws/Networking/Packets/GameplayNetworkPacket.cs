using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using PawsAndClaws.Utils;

namespace PawsAndClaws.Networking
{
    [System.Serializable]
    public class NPHello : ClientNetworkPacket
    {
        public NPHello(byte[] data) : base(data)
        {
            p_type = NPacketType.HELLO;
        }
        public NPHello()
        {
            p_type = NPacketType.HELLO;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);

            return this;
        }
        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data);
            SetBasePacketData(ref blob);
            return blob.Data;
        }
    }


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
            BlobStreamReader blob = new BlobStreamReader(data);
            ReadBasePacketData(ref blob);

            x = blob.Read<float>();
            y = blob.Read<float>();

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            // Set type and size
            SetBasePacketData(ref blob);

            // Write the position
            blob.Write(x);
            blob.Write(y);
            return blob.Data;
        }
    }
}
