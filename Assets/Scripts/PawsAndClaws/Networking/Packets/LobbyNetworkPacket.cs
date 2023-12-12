using System;
using System.Text;
using PawsAndClaws.Utils;
using UnityEngine;

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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);
            // Read name
            name = blob.Read<string>();

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            // Set base packet data
            SetBasePacketData(ref blob);
            
            // Set client name
            blob.Write(name);
            
            return blob.Data;
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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);
            response = (ResponseType)blob.Read<ushort>();
            player_id = blob.Read<ushort>();

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            // Set type and size
            SetBasePacketData(ref blob);

            // Set if accepted or not
            blob.Write((ushort)response);

            // Set player id
            blob.Write(player_id);
            
            return blob.Data;
        }
    }

    [System.Serializable]
    public class NPLobbyReadyReq : ClientNetworkPacket
    {
        public bool is_ready;
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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);
            is_ready = blob.Read<bool>();
            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            SetBasePacketData(ref blob);
            // Write ready flag
            blob.Write(is_ready);
            return blob.Data;
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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);
            accepted = blob.Read<bool>();
            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data);
            SetBasePacketData(ref blob);
            blob.Write(accepted);

            return blob.Data;
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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);
            
            spot = blob.Read<ushort>();
            team = (Player.Team)blob.Read<byte>();
            
            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            SetBasePacketData(ref blob);

            blob.Write(spot);
            blob.Write((byte)team);
            
            return blob.Data;
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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);

            spot = blob.Read<ushort>();
            team = (Player.Team)blob.Read<byte>();

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            SetBasePacketData(ref blob);

            blob.Write(spot);
            blob.Write((byte)team);
            
            return blob.Data;
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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            SetBasePacketData(ref blob);
            return blob.Data;
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
            BlobStreamReader blob = new BlobStreamReader(buffer);
            ReadBasePacketData(ref blob);
            client_id = blob.Read<ushort>();
            name = blob.Read<string>();

            return this;
        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[MAX_BUFFER_SIZE];
            BlobStreamWriter blob = new BlobStreamWriter(data, MAX_BUFFER_SIZE);
            SetBasePacketData(ref blob);

            blob.Write(client_id);
            blob.Write(name);
            
            return blob.Data;
        }
    }
}