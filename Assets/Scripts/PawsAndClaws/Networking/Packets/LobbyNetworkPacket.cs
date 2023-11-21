using System;
using System.Text;

namespace PawsAndClaws.Networking.Packets
{
    [System.Serializable]
    public class NPLobbyReq : ClientNetworkPacket
    {
        public string name;

        public NPLobbyReq()
        {
            p_type = (ushort)NPacketType.LOBBYREQ;
        }
    }

    public class NPLobbyRes : NetworkPacket
    {
        public bool accepted;

        public NPLobbyRes()
        {
            p_type = (ushort)NPacketType.LOBBYRES;
        }
    }
    
    [System.Serializable]
    public class NPLobbyReadyReq : ClientNetworkPacket
    {
        public uint userId;

        public NPLobbyReadyReq()
        {
            p_type = (ushort)NPacketType.LOBBY_READY_REQ;
        }
    }
    
    public class NPLobbyReadyRes : NetworkPacket
    {
        public bool accepted;
        public NPLobbyReadyRes()
        {
            p_type = (ushort)NPacketType.LOBBY_READY_RES;
        }
    }
    
    public static class LobbyNetworkPacket
    {
        public static byte[] NPLobbyResToByteArray(NPLobbyRes packet)
        {
            byte[] data = new byte[NetworkPacket.MAX_BUFFER_SIZE];

            // Set type and size
            int index = packet.setBasePacketData(data);

            // Set if accepted or not
            BitConverter.GetBytes(packet.accepted).CopyTo(data, index);
            index += 1; // bool(1)

            // Set packet size in bytes
            packet.p_size = index;
            BitConverter.GetBytes(packet.p_size).CopyTo(data, 0); // size index

            return data;
        }

        public static byte[] NPLobbyReqToByteArray(NPLobbyReq packet)
        {
            byte[] data = new byte[NetworkPacket.MAX_BUFFER_SIZE];

            // Set base packet data
            int index = packet.setBasePacketData(data);

            byte[] name = Encoding.ASCII.GetBytes(packet.name);

            // Set client name
            name.CopyTo(data, index);
            index += name.Length; // char count in string

            // Set packet size in bytes
            packet.p_size = index;
            BitConverter.GetBytes(packet.p_size).CopyTo(data, 0); // size index

            return data;
        }

        public static NetworkPacket LobbyReqToNetworkPacket(byte[] buffer)
        {
            int offset = 0;

            NPLobbyReq packet = new NPLobbyReq();
            offset = packet.readBasePacketData(buffer);

            // Read name
            packet.name = Encoding.ASCII.GetString(buffer, offset, packet.p_size - offset);

            return packet;
        }

        public static NetworkPacket LobbyResToNetworkPacket(byte[] buffer)
        {
            int offset = 0;
            NPLobbyRes packet = new NPLobbyRes();
            offset = packet.readBasePacketData(buffer);

            packet.accepted = BitConverter.ToBoolean(buffer, offset);

            return packet;
        }
    }
}