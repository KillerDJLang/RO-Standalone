/*
using LiteNetLib.Utils;

namespace ROStandalone.Packets
{
    public struct RandomEventSyncPacket : INetSerializable
    {
        public string EventToRun;

        public void Deserialize(NetDataReader reader)
        {
            EventToRun = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(EventToRun);
        }
    }

    public struct DoorEventSyncPacket : INetSerializable
    {
        public string DoorId;

        public void Deserialize(NetDataReader reader)
        {
            DoorId = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(DoorId);
        }
    }

    public struct KeycardDoorEventSyncPacket : INetSerializable
    {
        public string KeycardDoorId;

        public void Deserialize(NetDataReader reader)
        {
            KeycardDoorId = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(KeycardDoorId);
        }
    }

    public struct SwitchEventSyncPacket : INetSerializable
    {
        public string SwitchId;

        public void Deserialize(NetDataReader reader)
        {
            SwitchId = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(SwitchId);
        }
    }

    public struct RaidStartDoorStateSyncPacket : INetSerializable
    {
        public string DoorId;

        public void Deserialize(NetDataReader reader)
        {
            DoorId = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(DoorId);
        }
    }

    public struct RaidStartLampStateSyncPacket : INetSerializable
    {
        public string LampId;

        public void Deserialize(NetDataReader reader)
        {
            LampId = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(LampId);
        }
    }
}
*/