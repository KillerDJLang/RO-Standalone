using EFT;
using System;
using UnityEngine;
using EFT.Interactive;
using EFT.Communications;
using System.Reflection;
using System.Collections;
using ROStandalone.Helpers;
using ROStandalone.Configs;
//using ROStandalone.Fika;

namespace ROStandalone.Controllers
{
    internal class DoorController : MonoBehaviour
    {
        private Switch[] _switchs = null;
        private Door[] _door = null;
        private KeycardDoor[] _kdoor = null;
        private bool _dooreventisRunning = false;

        private static int _doorChangedCount = 0;
        private static int _doorNotChangedCount = 0;
        private static int _lampCount = 0;

        void Update()
        {
            if (!Ready() || !DJConfig.EnableDoorEvents.Value)
            {
                if (_lampCount != 0)            { _lampCount = 0; }
                if (_doorChangedCount != 0)     { _doorChangedCount = 0; }
                if (_doorNotChangedCount != 0)  { _doorNotChangedCount = 0; }

                return;
            }

            if (_switchs == null)
            {
                _switchs = FindObjectsOfType<Switch>();
            }

            if (_door == null)
            {
                _door = FindObjectsOfType<Door>();
            }

            if (_kdoor == null)
            {
                _kdoor = FindObjectsOfType<KeycardDoor>();
            }

            if (!_dooreventisRunning) // && FikaInterface.IAmHost()
            {
                StaticManager.Instance.StartCoroutine(DoorEvents());

                _dooreventisRunning = true;
            }


        }

        private IEnumerator DoorEvents()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(ConfigController.EventConfig.DoorEventRangeMinimumServer, ConfigController.EventConfig.DoorEventRangeMaximumServer) * 60f);

            if (Plugin.ROGameWorld != null && Plugin.ROGameWorld.AllAlivePlayersList != null && Plugin.ROGameWorld.AllAlivePlayersList.Count > 0 && !(Plugin.ROPlayer is HideoutPlayer)) // && FikaInterface.IAmHost()
            {
                Weighting.DoRandomEvent(Weighting.weightedDoorMethods);
            }

            else
            {
                _switchs = null;
                _door = null;
                _kdoor = null;
            }

            _dooreventisRunning = false;
            yield break;
        }

        #region Door Event Controller

        public void PowerOn()
        {
            if (Plugin.ROPlayer.Location != "laboratory" && Plugin.ROPlayer.Location != "rezervbase" && Plugin.ROPlayer.Location != "bigmap" && Plugin.ROPlayer.Location != "interchange")
            {
                if (ConfigController.DebugConfig.DebugMode) {
                    Plugin.Log.LogInfo("No switches available on this map, returning.");
                }
                return;
            }

            if (_switchs == null || _switchs.Length <= 0)
            {
                if (ConfigController.DebugConfig.DebugMode) {
                    Plugin.Log.LogInfo("No switches left to open, returning.");
                }
                return;
            }

            System.Random random = new System.Random();

            int selection = random.Next(_switchs.Length + 1);
            Switch _switch = _switchs[selection];

            if (_switch.DoorState == EDoorState.Shut)
            {
                //FikaInterface.SendSwitchStateChangePacket(_switch.Id);
                typeof(Switch).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(_switch, null);

                if (ConfigController.DebugConfig.DebugMode) {
                    Utils.LogToServerConsole("A random switch has been thrown.");
                }

                RemoveAt(ref _switchs, selection);
            }

            else
            {
                RemoveAt(ref _door, selection);
            }
        }

        public void DoUnlock()
        {
            if (_door == null || _door.Length <= 0)
            {
                if (ConfigController.DebugConfig.DebugMode) {
                    Plugin.Log.LogInfo("No locked doors available, returning.");
                }
                return;
            }

            System.Random random = new System.Random();

            int selection = random.Next(_door.Length + 1);
            Door door = _door[selection];

            if (door.gameObject.layer != LayerMaskClass.InteractiveLayer)
            {
                if (ConfigController.DebugConfig.DebugMode) {
                    Plugin.Log.LogInfo("Chosen door isn't on the interactive layer, returning.");
                }
                return;
            }

            if (door.DoorState == EDoorState.Locked && door.Operatable && door.enabled)
            {
                //FikaInterface.SendDoorStateChangePacket(door.Id);
                typeof(Door).GetMethod("Unlock", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
                typeof(Door).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);

                if (ConfigController.DebugConfig.DebugMode) {
                    Utils.LogToServerConsole("A random door has been unlocked.");
                }

                RemoveAt(ref _door, selection);
            }

            else
            {
                RemoveAt(ref _door, selection);
            }
        }

        public void DoKUnlock()
        {
            if (Plugin.ROPlayer.Location != "laboratory" && Plugin.ROPlayer.Location != "interchange")
            {
                if (ConfigController.DebugConfig.DebugMode) {
                    Plugin.Log.LogInfo("No keycard doors available on this map, returning.");
                }
                return;
            }

            if (_kdoor == null || _kdoor.Length <= 0)
            {
                if (ConfigController.DebugConfig.DebugMode) {
                    Plugin.Log.LogInfo("No keycard doors left to open, returning.");
                }
                return;
            }

            System.Random random = new System.Random();

            int selection = random.Next(_kdoor.Length + 1);
            KeycardDoor kdoor = _kdoor[selection];

            if (kdoor.DoorState == EDoorState.Locked)
            {
                //FikaInterface.SendKeycardDoorStateChangePacket(kdoor.Id);
                typeof(KeycardDoor).GetMethod("Unlock", BindingFlags.Instance | BindingFlags.Public).Invoke(kdoor, null);
                typeof(KeycardDoor).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(kdoor, null);

                if (ConfigController.DebugConfig.DebugMode) {
                    Utils.LogToServerConsole("A random keycard door has been unlocked.");
                }

                RemoveAt(ref _kdoor, selection);
            }

            else
            {
                RemoveAt(ref _door, selection);
            }
        }
        #endregion

        #region Random Raid Start Events

        public static void RandomizeDefaultDoors()
        {
            if (DJConfig.EnableRaidStartEvents.Value && Plugin.ROPlayer.Location != "laboratory")
            {
                FindObjectsOfType<Door>().ExecuteForEach(door =>
                {

                    if (!door.Operatable || !door.enabled)
                    {
                        _doorNotChangedCount++;
                        return;
                    }

                    if (door.DoorState != EDoorState.Shut && door.DoorState != EDoorState.Open)
                    {
                        _doorNotChangedCount++;
                        return;
                    }

                    if (door.DoorState == EDoorState.Locked)
                    {
                        _doorNotChangedCount++;
                        return;
                    }

                    if (door.gameObject.layer != LayerMaskClass.InteractiveLayer)
                    {
                        _doorNotChangedCount++;
                        return;
                    }

                    if (UnityEngine.Random.Range(0, 100) < 50 && (door.DoorState == EDoorState.Shut))
                    {
                        //FikaInterface.SendRaidStartDoorStateChangePacket(door.Id);
                        typeof(Door).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
                        _doorChangedCount++;
                    }

                    if (UnityEngine.Random.Range(0, 100) < 50 && (door.DoorState == EDoorState.Open))
                    {
                        //FikaInterface.SendRaidStartDoorStateChangePacket(door.Id);
                        typeof(Door).GetMethod("Close", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
                        _doorChangedCount++;
                    }
                });

                if (ConfigController.DebugConfig.DebugMode) {
                    NotificationManagerClass.DisplayMessageNotification($"[{_doorChangedCount}] total Doors have had their states changed. [{_doorNotChangedCount}] haven't been modified.", ENotificationDurationType.Long, ENotificationIconType.Default);
                    Utils.LogToServerConsole($"[{_doorChangedCount}] total Doors have had their states changed. [{_doorNotChangedCount}] haven't been modified.");
                }
            }
        }

        public static void RandomizeLampState()
        {
            if (DJConfig.EnableRaidStartEvents.Value && Plugin.ROPlayer.Location != "laboratory")
            {
                FindObjectsOfType<LampController>().ExecuteForEach(lamp =>
                {
                    if (UnityEngine.Random.Range(0, 100) < 25)
                    {
                        //FikaInterface.SendRaidStartLampStateChangePacket(lamp.Id);
                        lamp.Switch(Turnable.EState.Off);
                        lamp.enabled = false;
                        _lampCount++;
                    }
                });

                if (ConfigController.DebugConfig.DebugMode) {
                    NotificationManagerClass.DisplayMessageNotification($"[{_lampCount}] total Lamps have been modified.", ENotificationDurationType.Long, ENotificationIconType.Default);
                    Utils.LogToServerConsole($"[{_lampCount}] total Lamps have been modified.");
                }
            }
        }
        #endregion

        static void RemoveAt<T>(ref T[] array, int index)
        {
            if (index >= 0 && index < array.Length)
            {
                for (int i = index; i < array.Length - 1; i++)
                {
                    array[i] = array[i + 1];
                }

                Array.Resize(ref array, array.Length - 1);
            }
        }

        public bool Ready()
        {
            return Plugin.ROGameWorld != null && Plugin.ROGameWorld.AllAlivePlayersList != null && Plugin.ROGameWorld.AllAlivePlayersList.Count > 0 && !(Plugin.ROPlayer is HideoutPlayer);
        }
    }
}
