using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using PawsAndClaws.Utils;

namespace PawsAndClaws.Networking
{
    [System.Serializable]
    public abstract class GameplayPacket : NetworkPacket
    {
        public int net_id;

        public GameplayPacket()
        {

        }

        public GameplayPacket(byte[] data) : base(data)
        {

        }

        public override void SetBasePacketData(ref BlobStreamWriter blob)
        {
            base.SetBasePacketData(ref blob);
            // Copy ID
            blob.Write(net_id);
        }

        public override void ReadBasePacketData(ref BlobStreamReader blob)
        {
            base.ReadBasePacketData(ref blob);
            // Read ID
            net_id = blob.Read<int>();
        }
    }

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
    public class NPObjectPos : GameplayPacket
    {
        public float x;
        public float y;

        public NPObjectPos(byte[] data) : base(data)
        {
            p_type = NPacketType.OBJECTPOS;
        }

        public NPObjectPos()
        {
            p_type = NPacketType.OBJECTPOS;
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

    [Serializable]
    public class NPAbility : GameplayPacket
    {
        public int ab_number;
        public Vector2 ab_direction;

        public NPAbility(byte[] data) : base(data)
        {
            p_type = NPacketType.ABILITY;
        }

        public NPAbility()
        {
            p_type = NPacketType.ABILITY;
        }

        public override NetworkPacket LoadByteArray(byte[] data)
        {
            BlobStreamReader blob = new BlobStreamReader(data);
            ReadBasePacketData(ref blob);

            // Read data
            ab_number = blob.Read<int>();

            ab_direction.x = blob.Read<float>();
            ab_direction.y = blob.Read<float>();

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            // Set type and size
            SetBasePacketData(ref blob);

            // Set data
            blob.Write(ab_number);
            blob.Write(ab_direction.x);
            blob.Write(ab_direction.y);

            return blob.Data;
        }
    }

    [System.Serializable]
    public class NPMoveDirection : GameplayPacket
    {
        public float dx;
        public float dy;

        public NPMoveDirection(byte[] data) : base(data)
        {
            p_type = NPacketType.MOVEDIR;
        }

        public NPMoveDirection()
        {
            p_type = NPacketType.MOVEDIR;
        }

        public override NetworkPacket LoadByteArray(byte[] data)
        {
            BlobStreamReader blob = new BlobStreamReader(data);
            ReadBasePacketData(ref blob);

            dx = blob.Read<float>();
            dy = blob.Read<float>();

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            // Set type and size
            SetBasePacketData(ref blob);

            // Write the position
            blob.Write(dx);
            blob.Write(dy);

            return blob.Data;
        }
    }
}