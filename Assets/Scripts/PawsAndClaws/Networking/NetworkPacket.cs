using System;
using System.Text;
using PawsAndClaws.Networking.Packets;
using PawsAndClaws.Utils;

namespace PawsAndClaws.Networking
{
    public enum NPacketType : ushort
    {
        LOBBYPLAYERCON,
        LOBBYSTARTGAME,
        LOBBYREQ,
        LOBBYRES,
        LOBBYSPOTREQ,
        LOBBYSPOTUPDATE,
        LOBBY_READY_REQ,
        LOBBY_READY_RES,
        HELLO,
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

            NPacketType type = (NPacketType)BitConverter.ToUInt16(buffer, 0);

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
                case NPacketType.LOBBYSPOTUPDATE:
                    packet = new NPLobbySpotUpdate(buffer);
                    break;
                case NPacketType.PLAYERPOS:
                    packet = new NPPlayerPos(buffer);
                    break;
                case NPacketType.LOBBYPLAYERCON:
                    packet = new NPLobbyPlayerCon(buffer);
                    break;
                case NPacketType.LOBBYSTARTGAME:
                    packet = new NPLobbyStartGame(buffer);
                    break;
                case NPacketType.LOBBY_READY_REQ:
                    packet = new NPLobbyReadyReq(buffer);
                    break;
                case NPacketType.HELLO:
                    packet = new NPHello(buffer);
                    break;
            }

            return packet;
        }

        public virtual void SetBasePacketData(ref BlobStreamWriter blob)
        {
            // Packet type
            blob.Write((ushort)p_type);
        }

        public virtual void ReadBasePacketData(ref BlobStreamReader blob)
        {
            p_type = (NPacketType)blob.Read<ushort>();
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

        public override void SetBasePacketData(ref BlobStreamWriter blob)
        {
            base.SetBasePacketData(ref blob);
            // Copy ID
            blob.Write(id);
        }

        public override void ReadBasePacketData(ref BlobStreamReader blob)
        {
            base.ReadBasePacketData(ref blob);
            // Read ID
            id = blob.Read<ushort>();
        }
    }
}