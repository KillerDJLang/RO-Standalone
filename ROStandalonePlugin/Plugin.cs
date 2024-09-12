using EFT;
using Comfort.Common;
using System;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using SPT.Reflection.Utils;
using ROStandalone.Helpers;
using ROStandalone.Patches;
using ROStandalone.Controllers;
using ROStandalone.Checkers;

namespace ROStandalone
{
    [BepInPlugin("DJ.ROStandalone", "Raid Overhaul Standalone", PluginVersion)]

    public class Plugin : BaseUnityPlugin
    {
        public const int TarkovVersion = 30626;
        public const string PluginVersion = "1.1.0";
        internal static GameObject Hook;
        internal static EventController ECScript;
        internal static DoorController DCScript;
        internal static ManualLogSource Log;

        public static ISession Session;

        public static GameWorld ROGameWorld
        { get => Singleton<GameWorld>.Instance; }
        
        public static Player ROPlayer
        { get => ROGameWorld.MainPlayer; }

        public static SkillManager ROSkillManager
        { get => ROGameWorld.MainPlayer.Skills; }

        void Awake()
        {
            if (!VersionChecker.CheckEftVersion(Logger, Info, Config))
            {
                throw new Exception("Invalid EFT Version");
            }

            // Bind the configs
            DJConfig.BindConfig(Config);

            Log = Logger;
            Logger.LogInfo("Loading RO Standalone");
            Hook = new GameObject("Event Object");
            ECScript = Hook.AddComponent<EventController>();
            DCScript = Hook.AddComponent<DoorController>();
            DontDestroyOnLoad(Hook);

            // Get and Initialize the Server Configs
            ConfigController.EventConfig = Utils.Get<EventsConfig>("/ROStandaloneBackend/GetEventConfig");
            Weighting.InitWeightings();

            ConfigController.ServerConfig = Utils.Get<ServerConfigs>("/ROStandaloneBackend/GetServerConfig");

            Utils.GetWeatherFields();

            new RandomizeDefaultStatePatch().Enable();
            new EventExfilPatch().Enable();
            new AirdropBoxPatch().Enable();

#if DEBUG
            ConsoleCommands.RegisterCC();
#endif
        }

        void Update()
        {
            if (Session == null && ClientAppUtils.GetMainApp().GetClientBackEndSession() != null)
            {
                Session = ClientAppUtils.GetMainApp().GetClientBackEndSession();

                Log.LogDebug("RO Standalone Session set");
            }
        }
    }
}
