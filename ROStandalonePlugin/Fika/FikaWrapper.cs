/*
using Comfort.Common;
using EFT;
using EFT.Interactive;
using EFT.Communications;
using LiteNetLib;
using System.Reflection;
using Fika.Core.Coop.Utils;
using Fika.Core.Modding;
using Fika.Core.Modding.Events;
using Fika.Core.Networking;
using ROStandalone.Packets;
using ROStandalone.Helpers;

namespace ROStandalone.Fika
{
    internal class FikaWrapper
    {
        public static bool IAmHost()
        {
            return Singleton<FikaServer>.Instantiated;
        }

        public static string GetRaidId()
        {
            return FikaBackendUtils.GroupId;
        }

        public static void SendRandomEventPacket(string eventToSend)
        {
            RandomEventSyncPacket packet = new RandomEventSyncPacket
            {
                EventToRun = eventToSend
            };
            if (Singleton<FikaServer>.Instantiated)
            {
                Singleton<FikaServer>.Instance.SendDataToAll<RandomEventSyncPacket>(ref packet, DeliveryMethod.ReliableOrdered);
            }
        }

        public static void SendDoorStateChangePacket(string doorId)
        {
            DoorEventSyncPacket packet = new DoorEventSyncPacket
            {
                DoorId = doorId
            };
            if (Singleton<FikaServer>.Instantiated)
            {
                Singleton<FikaServer>.Instance.SendDataToAll<DoorEventSyncPacket>(ref packet, DeliveryMethod.ReliableOrdered);
            }
        }

        public static void SendSwitchStateChangePacket(string switchId)
        {
            SwitchEventSyncPacket packet = new SwitchEventSyncPacket
            {
                SwitchId = switchId
            };
            if (Singleton<FikaServer>.Instantiated)
            {
                Singleton<FikaServer>.Instance.SendDataToAll<SwitchEventSyncPacket>(ref packet, DeliveryMethod.ReliableOrdered);
            }
        }

        public static void SendKeycardDoorStateChangePacket(string keycardDoorId)
        {
            KeycardDoorEventSyncPacket packet = new KeycardDoorEventSyncPacket
            {
                KeycardDoorId = keycardDoorId
            };
            if (Singleton<FikaServer>.Instantiated)
            {
                Singleton<FikaServer>.Instance.SendDataToAll<KeycardDoorEventSyncPacket>(ref packet, DeliveryMethod.ReliableOrdered);
            }
        }

        public static void SendRaidStartDoorStateChangePacket(string doorId)
        {
            RaidStartDoorStateSyncPacket packet = new RaidStartDoorStateSyncPacket
            {
                DoorId = doorId
            };
            if (Singleton<FikaServer>.Instantiated)
            {
                Singleton<FikaServer>.Instance.SendDataToAll<RaidStartDoorStateSyncPacket>(ref packet, DeliveryMethod.ReliableOrdered);
            }
        }

        public static void SendRaidStartLampStateChangePacket(string lampId)
        {
            RaidStartLampStateSyncPacket packet = new RaidStartLampStateSyncPacket
            {
                LampId = lampId
            };
            if (Singleton<FikaServer>.Instantiated)
            {
                Singleton<FikaServer>.Instance.SendDataToAll<RaidStartLampStateSyncPacket>(ref packet, DeliveryMethod.ReliableOrdered);
            }
        }

        private static void ReceiveRandomEventPacket(RandomEventSyncPacket packet, NetPeer peer)
        {
            if (packet.EventToRun == Utils.Heal)
            {
                Plugin.ECScript.DoHealPlayer();
            }
            if (packet.EventToRun == Utils.Damage)
            {
                Plugin.ECScript.DoDamageEvent();
            }
            if (packet.EventToRun == Utils.Repair)
            {
                Plugin.ECScript.DoArmorRepair();
            }
            if (packet.EventToRun == Utils.Airdrop)
            {
                NotificationManagerClass.DisplayMessageNotification("Aidrop Event: Incoming Airdrop!", ENotificationDurationType.Long, ENotificationIconType.Quest);
            }
            if (packet.EventToRun == Utils.Jokes)
            {
                Plugin.ECScript.DoFunny();
            }
            if (packet.EventToRun == Utils.Blackout)
            {
                Plugin.ECScript.DoBlackoutEvent();
            }
            if (packet.EventToRun == Utils.Skill)
            {
                Plugin.ECScript.DoSkillEvent();
            }
            if (packet.EventToRun == Utils.Metabolism)
            {
                Plugin.ECScript.DoMetabolismEvent();
            }
            if (packet.EventToRun == Utils.Malf)
            {
                Plugin.ECScript.DoMalfEvent();
            }
            if (packet.EventToRun == Utils.LoyaltyLevel)
            {
                Plugin.ECScript.DoLLEvent();
            }
            if (packet.EventToRun == Utils.Berserk)
            {
                Plugin.ECScript.DoBerserkEvent();
            }
            if (packet.EventToRun == Utils.Weight)
            {
                Plugin.ECScript.DoWeightEvent();
            }
            if (packet.EventToRun == Utils.MaxLoyaltyLevel)
            {
                Plugin.ECScript.DoMaxLLEvent();
            }
            if (packet.EventToRun == Utils.CorrectRep)
            {
                Plugin.ECScript.CorrectRep();
            }
            if (packet.EventToRun == Utils.Lockdown)
            {
                Plugin.ECScript.DoLockDownEvent();
            }
            if (packet.EventToRun == Utils.GearExfilEvent)
            {
                NotificationManagerClass.DisplayMessageNotification("Gear Exfil Event: Host has activated the gear exfil event. \nHunker down and protect them until their gear is safely locked away.", ENotificationDurationType.Long, ENotificationIconType.Quest);
            }
            if (packet.EventToRun == Utils.Train)
            {
                Plugin.ECScript.RunTrain();
            }
            if (packet.EventToRun == Utils.PmcExfil)
            {
                Plugin.ECScript.DoPmcExfilEvent();
            }
            if (packet.EventToRun == Utils.Artillery)
            {
                Plugin.ECScript.DoArtyEvent();
            }

            if (Plugin.ROGameWorld != null && Plugin.ROGameWorld.AllAlivePlayersList != null && Plugin.ROGameWorld.AllAlivePlayersList.Count > 0 && !(Plugin.ROPlayer is HideoutPlayer))
            {
                Plugin.ECScript.CleanForNewEvent();
            }
        }

        private static void ReceiveDoorStateChangePacket(DoorEventSyncPacket packet, NetPeer peer)
        {
            Door door = ROSession.GetDoorById(packet.DoorId);

            if (door.DoorState == EDoorState.Locked && door.Operatable && door.enabled)
            {
                typeof(Door).GetMethod("Unlock", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
                typeof(Door).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
            }
        }

        private static void ReceiveKeycardDoorStateChangePacket(KeycardDoorEventSyncPacket packet, NetPeer peer)
        {
            KeycardDoor kDoor = ROSession.GetKeycardDoorById(packet.KeycardDoorId);

            if (kDoor.DoorState == EDoorState.Locked && kDoor.Operatable && kDoor.enabled)
            {
                typeof(KeycardDoor).GetMethod("Unlock", BindingFlags.Instance | BindingFlags.Public).Invoke(kDoor, null);
                typeof(KeycardDoor).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(kDoor, null);
            }
        }

        private static void ReceiveSwitchStateChangePacket(SwitchEventSyncPacket packet, NetPeer peer)
        {
            Switch pSwitch = ROSession.GetSwitchById(packet.SwitchId);

            if (pSwitch.DoorState == EDoorState.Shut)
            {
                typeof(Switch).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(pSwitch, null);
            }
        }

        private static void ReceiveRaidStartDoorStateChangePacket(RaidStartDoorStateSyncPacket packet, NetPeer peer)
        {
            Door door = ROSession.GetDoorById(packet.DoorId);

            if (door.DoorState == EDoorState.Shut)
            {
                typeof(Door).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
            }

            if (door.DoorState == EDoorState.Open)
            {
                typeof(Door).GetMethod("Close", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
            }
        }

        private static void ReceiveRaidStartLampStateChangePacket(RaidStartLampStateSyncPacket packet, NetPeer peer)
        {
            LampController lamp = ROSession.GetLampById(packet.LampId);

            lamp.Switch(Turnable.EState.Off);
            lamp.enabled = false;
        }

        public static void OnFikaNetManagerCreated(FikaNetworkManagerCreatedEvent managerCreatedEvent)
        {
            managerCreatedEvent.Manager.RegisterPacket<RandomEventSyncPacket, NetPeer>(ReceiveRandomEventPacket);
            managerCreatedEvent.Manager.RegisterPacket<DoorEventSyncPacket, NetPeer>(ReceiveDoorStateChangePacket);
            managerCreatedEvent.Manager.RegisterPacket<KeycardDoorEventSyncPacket, NetPeer>(ReceiveKeycardDoorStateChangePacket);
            managerCreatedEvent.Manager.RegisterPacket<SwitchEventSyncPacket, NetPeer>(ReceiveSwitchStateChangePacket);
            managerCreatedEvent.Manager.RegisterPacket<RaidStartDoorStateSyncPacket, NetPeer>(ReceiveRaidStartDoorStateChangePacket);
            managerCreatedEvent.Manager.RegisterPacket<RaidStartLampStateSyncPacket, NetPeer>(ReceiveRaidStartLampStateChangePacket);
        }

        public static void InitOnPluginEnabled()
        {
            FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerCreatedEvent>(OnFikaNetManagerCreated);
        }
    }
}
*/
