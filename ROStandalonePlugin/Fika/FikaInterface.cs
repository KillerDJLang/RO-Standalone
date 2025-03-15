/*
using SPT.Reflection.Utils;

namespace ROStandalone.Fika
{
    internal class FikaInterface
    {
        internal static void InitOnPluginEnabled()
        {
            if (!Plugin.fikaDetected) return;
            FikaWrapper.InitOnPluginEnabled();
        }

        public static bool IAmHost()
        {
            if (!Plugin.fikaDetected) return true;
            return FikaWrapper.IAmHost();
        }

        public static string GetRaidId()
        {
            if (!Plugin.fikaDetected) return ClientAppUtils.GetMainApp().GetClientBackEndSession().Profile.ProfileId;
            return FikaWrapper.GetRaidId();
        }

        internal static void SendRandomEventPacket(string eventToSend)
        {
            if (!Plugin.fikaDetected) return;
            FikaWrapper.SendRandomEventPacket(eventToSend);
        }

        internal static void SendDoorStateChangePacket(string doorId)
        {
            if (!Plugin.fikaDetected) return;
            FikaWrapper.SendDoorStateChangePacket(doorId);
        }

        internal static void SendKeycardDoorStateChangePacket(string keycardDoorId)
        {
            if (!Plugin.fikaDetected) return;
            FikaWrapper.SendKeycardDoorStateChangePacket(keycardDoorId);
        }

        internal static void SendSwitchStateChangePacket(string switchId)
        {
            if (!Plugin.fikaDetected) return;
            FikaWrapper.SendSwitchStateChangePacket(switchId);
        }

        internal static void SendRaidStartDoorStateChangePacket(string doorId)
        {
            if (!Plugin.fikaDetected) return;
            FikaWrapper.SendRaidStartDoorStateChangePacket(doorId);
        }

        internal static void SendRaidStartLampStateChangePacket(string lampId)
        {
            if (!Plugin.fikaDetected) return;
            FikaWrapper.SendRaidStartLampStateChangePacket(lampId);
        }
    }
}
*/
