using System;
using BepInEx.Configuration;

namespace ROStandalone.Configs
{
    public static class DJConfig
    {
        [Flags]
        public enum RaidEvents
        {
            Damage = 1,
            Blackout = 2,
            ArmorRepair = 4,
            Heal = 8,
            Airdrop = 16,
            Skill = 32,
            Metabolism = 64,
            Malfunction = 128,
            Trader = 256,
            Berserk = 512,
            Weight = 1024,
            NoJokesHere = 2048,
            ShoppingSpree = 4096,
            ExfilLockdown = 8192,
            Artillery = 16384,

        All = Damage | Blackout | ArmorRepair | Heal | Airdrop | Skill | Metabolism | Malfunction | Trader | Berserk | Weight | NoJokesHere | ShoppingSpree | ExfilLockdown | Artillery,
        }

        [Flags]
        public enum DoorEvents
        {
            PowerOn = 1,
            DoorUnlock = 2,
            KDoorUnlock = 4,

        All = PowerOn | DoorUnlock | KDoorUnlock,
        }

        public static ConfigEntry<bool> EnableEvents;
        public static ConfigEntry<bool> EnableDoorEvents;
        public static ConfigEntry<bool> EnableRaidStartEvents;

        public static ConfigEntry<RaidEvents> RandomEventsToEnable;

        public static ConfigEntry<DoorEvents> DoorEventsToEnable;

        public static void BindConfig(ConfigFile cfg)
        {
            #region Core Events

            EnableRaidStartEvents = cfg.Bind(
                "1. Core Events  (Changing Any Of These Options Requires Restart)",
                "Raid Start Events",
                true,
                new ConfigDescription("Dictates whether you allow the Door and Light randomization events to run on raid start or not.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 3 }));

            EnableEvents = cfg.Bind(
                "1. Core Events  (Changing Any Of These Options Requires Restart)",
                "Dynamic Events",
                true,
                new ConfigDescription("Dictates whether the dynamic event timer should increment and allow events to run or not.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 2 }));

            EnableDoorEvents = cfg.Bind(
                "1. Core Events  (Changing Any Of These Options Requires Restart)",
                "Dynamic Door Events",
                true,
                new ConfigDescription("Dictates whether the dynamic event timer should increment and allow door events to run or not.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 1 }));

            #endregion

            #region Random Events

            RandomEventsToEnable = cfg.Bind(
               "2. Random Events",
               "Events List",
                RaidEvents.All,
                new ConfigDescription("Disable/Enable any of the random events that occur throughout your raids.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 1 }));

            #endregion

            #region Door Events

            DoorEventsToEnable = cfg.Bind(
                "3. Door Events",
                "Door Events List",
                DoorEvents.All,
                new ConfigDescription("Disable/Enable any of the door/power switch events that occur throughout your raids.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 1 }));

            #endregion
        }
    }
}