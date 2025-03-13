using Comfort.Common;
using EFT;
using EFT.Interactive;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ROStandalone.Fika
{
    internal class ROSession : MonoBehaviour
    {
        private ROSession() { }
        private static ROSession _instance = null;

        public Player Player { get; private set; }
        public GameWorld GameWorld { get; private set; }
        public GamePlayerOwner GamePlayerOwner { get; private set; }
        public static ROSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!Singleton<GameWorld>.Instantiated)
                    {
                        throw new Exception("Can't get Instance, GameWorld is not instantiated");
                    }

                    _instance = Singleton<GameWorld>.Instance.MainPlayer.gameObject.GetOrAddComponent<ROSession>();
                }
                return _instance;
            }
        }

        public Dictionary<string, Door> AllDoors { get; private set; } = new();
        public Dictionary<string, Switch> AllSwitches { get; private set; } = new();
        public Dictionary<string, KeycardDoor> AllKDoors { get; private set; } = new();
        public Dictionary<string, LampController> AllLamps { get; private set; } = new();

        private void Awake()
        {
            GameWorld = Singleton<GameWorld>.Instance;
            Player = GameWorld.MainPlayer;
            GamePlayerOwner = Player.gameObject.GetComponent<GamePlayerOwner>();

            FindObjectsOfType<Door>().ExecuteForEach(door => { AllDoors[door.Id] = door; });
            FindObjectsOfType<LampController>().ExecuteForEach(lamp => { AllLamps[lamp.Id] = lamp; });
            FindObjectsOfType<KeycardDoor>().ExecuteForEach(kDoor => { AllKDoors[kDoor.Id] = kDoor; });
            FindObjectsOfType<Switch>().ExecuteForEach(pSwitch => { AllSwitches[pSwitch.Id] = pSwitch; });
        }

        public static Door GetDoorById(string id)
        {
            return Instance.AllDoors[id];
        }

        public static KeycardDoor GetKeycardDoorById(string id)
        {
            return Instance.AllKDoors[id];
        }

        public static Switch GetSwitchById(string id)
        {
            return Instance.AllSwitches[id];
        }

        public static LampController GetLampById(string id)
        {
            return Instance.AllLamps[id];
        }
    }
}
