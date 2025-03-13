using EFT;
using Comfort.Common;
using System;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Bootstrap;
using UnityEngine;
using SPT.Reflection.Utils;

using ROStandalone.Fika;
using ROStandalone.Models;
using ROStandalone.Helpers;
using ROStandalone.Patches;
using ROStandalone.Configs;
using ROStandalone.Checkers;
using ROStandalone.Controllers;

namespace ROStandalone
{
    [BepInPlugin(ClientInfo.ROGUID, ClientInfo.ROSPluginName, ClientInfo.PluginVersion)]

    public class Plugin : BaseUnityPlugin
    {
        public static string pluginPath = Path.Combine(Environment.CurrentDirectory, "BepInEx", "plugins", "RaidOverhaulStandalone");
        public static string resourcePath = Path.Combine(pluginPath, "Resources");
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

        public static bool fikaDetected { get; private set; }
        public static bool dedicatedClientDetected { get; private set; }


        void Awake()
        {
            if (!VersionChecker.CheckEftVersion(Logger, Info, Config))
            {
                throw new Exception("Invalid EFT Version");
            }

            if (!DependencyChecker.ValidateDependencies(Logger, Info, this.GetType(), Config))
            {
                throw new Exception("Missing Dependencies");
            }

            if (Chainloader.PluginInfos.ContainsKey("com.fika.core"))
            {
                fikaDetected = true;
            }

            if (Chainloader.PluginInfos.ContainsKey("com.fika.dedicated"))
            {
                dedicatedClientDetected = true;
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
            ConfigController.ServerConfig = Utils.Get<ServerConfigs>("/ROStandaloneBackend/GetServerConfig");
            ConfigController.DebugConfig = Utils.Get<DebugConfigs>("/ROStandaloneBackend/GetDebugConfig");
            Weighting.InitWeightings();

            Utils.GetWeatherFields();

            if (ConfigController.DebugConfig.TimeChanges) {
                new WatchPatch().Enable();
                new TimePanelPatch().Enable();
                new RaidSettingsPatch().Enable();
                new LocationInfoPanelPatch().Enable();
                new WeatherControllerPatch().Enable();
            }
            new RandomizeDefaultStatePatch().Enable();
            new EventExfilPatch().Enable();

            if (ConfigController.DebugConfig.DebugMode) {
                ConsoleCommands.RegisterCC();
            }
        }

        void Update()
        {
            if (Session == null && ClientAppUtils.GetMainApp().GetClientBackEndSession() != null)
            {
                Session = ClientAppUtils.GetMainApp().GetClientBackEndSession();

                Log.LogDebug("RO Standalone Session set");
            }
        }

        private void OnEnable()
        {
            FikaInterface.InitOnPluginEnabled();
        }
    }
}
