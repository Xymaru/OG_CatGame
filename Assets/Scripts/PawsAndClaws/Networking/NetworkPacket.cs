using System;
using System.Text;

namespace PawsAndClaws.Networking
{
    public enum NPacketType : ushort
    {
        LOBBYREQ,
        LOBBYRES
    }

    public class NetworkPacket
    {
        public ushort p_type;
        public int p_size;

        private int setBasePacketData(byte[] data)
        {
            // Packet type at index 0
            BitConverter.GetBytes(p_type).CopyTo(data, 0); // first 0

            // Leave 4 of space for size (ushort(2) + int(4))
            return 6;
        }

        private byte[] NPLobbyReqToByteArray(NPLobbyReq packet)
        {
            byte[] data = new byte[2048];

            // Set base packet data
            int index = setBasePacketData(data);

            // Set client id
            BitConverter.GetBytes(packet.id).CopyTo(data, index);
            index += 2; // ushort(2)

            byte[] name = Encoding.ASCII.GetBytes(packet.name);

            // Set client name
            name.CopyTo(data, index);
            index += name.Length; // wchar(2) * character length

            // Set packet size in bytes
            p_size = index;
            BitConverter.GetBytes(p_size).CopyTo(data, 2); // size index

            return data;
        }

        private byte[] NPLobbyResToByteArray(NPLobbyRes packet)
        {
            byte[] data = new byte[2048];

            // Set type and size
            int index = setBasePacketData(data);

            // Set if accepted or not
            BitConverter.GetBytes(packet.accepted).CopyTo(data, index);
            index += 1; // bool(1)

            // Set packet size in bytes
            p_size = index;
            BitConverter.GetBytes(p_size).CopyTo(data, 2); // size index

            return data;
        }

        public byte[] ToByteArray(NetworkPacket packet)
        {
            byte[] data = null;

            switch ((NPacketType)packet.p_type)
            {
                case NPacketType.LOBBYREQ:
                    data = NPLobbyReqToByteArray((NPLobbyReq)packet);
                    break;
                case NPacketType.LOBBYRES:
                    data = NPLobbyResToByteArray((NPLobbyRes)packet);
                    break;
            }

            return data;
        }
    }

    public class ClientNetworkPacket : NetworkPacket
    {
        public ushort id;
    }

    public class NPLobbyReq : ClientNetworkPacket
    {
        public string name;

        NPLobbyReq()
        {
            p_type = (ushort)NPacketType.LOBBYREQ;
        }
    }

    public class NPLobbyRes : NetworkPacket
    {
        public bool accepted;
    }
}