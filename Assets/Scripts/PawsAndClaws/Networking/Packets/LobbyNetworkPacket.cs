using System;
using System.Text;

namespace PawsAndClaws.Networking.Packets
{
    [System.Serializable]
    public class NPLobbyReq : ClientNetworkPacket
    {
        public string name;

        public NPLobbyReq(byte[] data) : base(data)
        {
            p_type = NPacketType.LOBBYREQ;
        }

        public NPLobbyReq()
        {
            p_type = NPacketType.LOBBYREQ;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            int offset = 0;

            offset = readBasePacketData(buffer);

            // Read name
            name = Encoding.ASCII.GetString(buffer, offset, p_size - offset);

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            // Set base packet data
            int index = setBasePacketData(data);

            byte[] name_bytes = Encoding.ASCII.GetBytes(name);

            // Set client name
            name_bytes.CopyTo(data, index);
            index += name.Length; // char count in string

            // Set packet size in bytes
            p_size = index;
            BitConverter.GetBytes(p_size).CopyTo(data, 0); // size index

            return data;
        }
    }

    public class NPLobbyRes : NetworkPacket
    {
        public ResponseType response;
        public ushort player_id;

        public NPLobbyRes(byte[] data) : base(data)
        {
            p_type = NPacketType.LOBBYRES;
        }

        public NPLobbyRes()
        {
            p_type = NPacketType.LOBBYRES;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            int offset = 0;

            offset = readBasePacketData(buffer);

            response = (ResponseType)BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);

            player_id = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            // Set type and size
            int index = setBasePacketData(data);

            // Set if accepted or not
            BitConverter.GetBytes((ushort)response).CopyTo(data, index);
            index += sizeof(ushort);

            // Set player id
            BitConverter.GetBytes(player_id).CopyTo(data, index);
            index += sizeof(ushort);

            // Set packet size in bytes
            p_size = index;
            BitConverter.GetBytes(p_size).CopyTo(data, 0); // size index

            return data;
        }
    }

    [System.Serializable]
    public class NPLobbyReadyReq : ClientNetworkPacket
    {
        public NPLobbyReadyReq(byte[] data) : base(data)
        {
            p_type = NPacketType.LOBBY_READY_REQ;
        }

        public NPLobbyReadyReq()
        {
            p_type = NPacketType.LOBBY_READY_REQ;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            readBasePacketData(buffer);

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            setBasePacketData(data);

            return data;
        }
    }

    public class NPLobbyReadyRes : NetworkPacket
    {
        public bool accepted;
        public NPLobbyReadyRes()
        {
            p_type = NPacketType.LOBBY_READY_RES;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            return this;
        }

        public override byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }
    }

    public class NPLobbySpotReq : ClientNetworkPacket
    {
        public ushort spot;
        public Player.Team team;

        public NPLobbySpotReq(byte[] data) : base(data)
        {
            p_type = NPacketType.LOBBYSPOTREQ;
        }

        public NPLobbySpotReq()
        {
            p_type = NPacketType.LOBBYSPOTREQ;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            int offset = 0;

            offset = readBasePacketData(buffer);

            spot = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            team = (Player.Team)buffer[offset];
            offset += 1;

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            int offset = setBasePacketData(data);

            BitConverter.GetBytes(spot).CopyTo(data, offset);
            offset += 2;

            data[offset] = (byte)team;
            offset += 1;

            return data;
        }
    }

    public class NPLobbySpotUpdate : ClientNetworkPacket
    {
        public ushort spot;
        public Player.Team team;

        public NPLobbySpotUpdate(byte[] data) : base(data)
        {
            p_type = NPacketType.LOBBYSPOTUPDATE;
        }

        public NPLobbySpotUpdate()
        {
            p_type = NPacketType.LOBBYSPOTUPDATE;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            int offset = 0;

            offset = readBasePacketData(buffer);

            spot = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);

            team = (Player.Team)buffer[offset];
            offset += sizeof(Player.Team);

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            int offset = setBasePacketData(data);

            BitConverter.GetBytes(spot).CopyTo(data, offset);
            offset += sizeof(ushort);

            data[offset] = (byte)team;
            offset += sizeof(Player.Team);

            return data;
        }
    }

    public class NPLobbyStartGame : NetworkPacket
    {
        public NPLobbyStartGame()
        {
            p_type = NPacketType.LOBBYSTARTGAME;
        }

        public NPLobbyStartGame(byte[] data) : base(data)
        {
            p_type = NPacketType.LOBBYSTARTGAME;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            readBasePacketData(buffer);

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            setBasePacketData(data);

            return data;
        }
    }

    public class NPLobbyPlayerCon : NetworkPacket
    {
        public ushort client_id;
        public string name;

        public NPLobbyPlayerCon(byte[] data) : base(data)
        {
            p_type = NPacketType.LOBBYPLAYERCON;
        }

        public NPLobbyPlayerCon()
        {
            p_type = NPacketType.LOBBYPLAYERCON;
        }

        public override NetworkPacket LoadByteArray(byte[] buffer)
        {
            int offset = readBasePacketData(buffer);

            client_id = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            name = Encoding.ASCII.GetString(buffer, offset, p_size - offset);

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];

            int offset = setBasePacketData(data);

            BitConverter.GetBytes(client_id).CopyTo(data, offset);
            offset += 2;

            Encoding.ASCII.GetBytes(name).CopyTo(data, offset);
            offset += name.Length;

            // Set packet size in bytes
            p_size = offset;
            BitConverter.GetBytes(p_size).CopyTo(data, 0); // size index

            return data;
        }
    }
}