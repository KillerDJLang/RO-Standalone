using EFT;
using Comfort.Common;
using System;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using Aki.Reflection.Utils;
using DJsROStandalone.Helpers;
using DJsROStandalone.Patches;
using DJsROStandalone.Controllers;

namespace DJsROStandalone
{
    [BepInPlugin("DJ.ROStandalone", "DJs RO Standalone", "1.0.0")]

    public class Plugin : BaseUnityPlugin
    {
        public const int TarkovVersion = 29197;
        internal static GameObject Hook;
        internal static EventController ECScript;
        internal static DoorController DCScript;
        internal static ManualLogSource Log;

        public static ISession Session;

        public static GameWorld ROSGameWorld
        { get => Singleton<GameWorld>.Instance; }
        
        public static Player ROSPlayer
        { get => ROSGameWorld.MainPlayer; }

        public static SkillManager ROSkillManager
        { get => ROSGameWorld.MainPlayer.Skills; }

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

            // Get and Initialize the weightings
            Weighting.EventWeights = Utils.Get<Weightings>("/ROStandaloneBackend/GetEventWeightings");

            Weighting.InitWeightings();

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
