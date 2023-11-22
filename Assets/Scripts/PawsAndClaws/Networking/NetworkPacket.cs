using System;
using System.Text;
using PawsAndClaws.Networking.Packets;

namespace PawsAndClaws.Networking
{
    public enum NPacketType : ushort
    {
        LOBBYREQ,
        LOBBYRES,
        LOBBYSPOTREQ,
        LOBBY_READY_REQ,
        LOBBY_READY_RES,
        PLAYERPOS
    }

    [System.Serializable]
    public abstract class NetworkPacket
    {
        public const int MAX_BUFFER_SIZE = 256;

        public int p_size;
        public NPacketType p_type;

        public NetworkPacket()
        {

        }

        public NetworkPacket(byte[] data)
        {
            LoadByteArray(data);
        }

        public abstract byte[] ToByteArray();

        public abstract NetworkPacket LoadByteArray(byte[] buffer);

        public static NetworkPacket FromByteArray(byte[] buffer)
        {
            NetworkPacket packet = null;

            NPacketType type = (NPacketType)BitConverter.ToUInt16(buffer, 4);

            switch (type)
            {
                case NPacketType.LOBBYREQ:
                    packet = new NPLobbyReq(buffer);
                    break;
                case NPacketType.LOBBYRES:
                    packet = new NPLobbyRes(buffer);
                    break;
                case NPacketType.LOBBYSPOTREQ:
                    packet = new NPLobbySpotReq(buffer);
                    break;
                case NPacketType.PLAYERPOS:
                    packet = new NPPlayerPos(buffer);
                    break;
            }

            return packet;
        }

        public virtual int setBasePacketData(byte[] data)
        {
            // Packet type at index 4
            BitConverter.GetBytes((ushort)p_type).CopyTo(data, 4); // first 4 is size, second is type

            // Leave 4 of space for size (int(4) + ushort(2))
            return 6;
        }

        public virtual int readBasePacketData(byte[] buffer)
        {
            p_size = BitConverter.ToInt32(buffer, 0);

            // Leave 4 of space for size (int(4) + ushort(2))
            return 6;
        }
    }

    [System.Serializable]
    public abstract class ClientNetworkPacket : NetworkPacket
    {
        public ushort id;

        public ClientNetworkPacket()
        {

        }

        public ClientNetworkPacket(byte[] data) : base(data)
        {

        }

        public override int setBasePacketData(byte[] buffer){
            int index = base.setBasePacketData(buffer);

            // Copy ID
            BitConverter.GetBytes(id).CopyTo(buffer, index);

            index += 2;

            return index;
        }

        public override int readBasePacketData(byte[] buffer)
        {
            int index = base.readBasePacketData(buffer);

            // Read ID
            id = BitConverter.ToUInt16(buffer, index);

            index += 2;

            return index;
        }
    }
}