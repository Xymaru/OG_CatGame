using System;
using System.Text;

namespace PawsAndClaws.Networking
{
    public enum NPacketType : ushort
    {
        LOBBYREQ,
        LOBBYRES
    }

    [System.Serializable]
    public class NetworkPacket
    {
        public const int MAX_BUFFER_SIZE = 256;

        public int p_size;
        public ushort p_type;

        public static NetworkPacket ByteArrayToNetworkPacket(byte[] buffer)
        {
            NetworkPacket packet = null;

            NPacketType type = (NPacketType)BitConverter.ToUInt16(buffer, 4);

            switch (type)
            {
                case NPacketType.LOBBYREQ:
                    packet = LobbyNetworkPacket.LobbyReqToNetworkPacket(buffer);
                    break;
                case NPacketType.LOBBYRES:
                    packet = LobbyNetworkPacket.LobbyResToNetworkPacket(buffer);
                    break;
            }

            return packet;
        }

        public virtual int setBasePacketData(byte[] data)
        {
            // Packet type at index 4
            BitConverter.GetBytes(p_type).CopyTo(data, 4); // first 4 is size, second is type

            // Leave 4 of space for size (int(4) + ushort(2))
            return 6;
        }

        public virtual int readBasePacketData(byte[] buffer)
        {
            p_size = BitConverter.ToInt32(buffer, 0);

            // Leave 4 of space for size (int(4) + ushort(2))
            return 6;
        }

        public byte[] ToByteArray(NetworkPacket packet)
        {
            byte[] data = null;

            switch ((NPacketType)packet.p_type)
            {
                case NPacketType.LOBBYREQ:
                    data = LobbyNetworkPacket.NPLobbyReqToByteArray((NPLobbyReq)packet);
                    break;
                case NPacketType.LOBBYRES:
                    data = LobbyNetworkPacket.NPLobbyResToByteArray((NPLobbyRes)packet);
                    break;
            }

            return data;
        }
    }

    [System.Serializable]
    public class ClientNetworkPacket : NetworkPacket
    {
        public ushort id;

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