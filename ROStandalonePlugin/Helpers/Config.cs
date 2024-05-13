using BepInEx.Configuration;

namespace DJsROStandalone.Helpers
{
    public static class DJConfig
    {
        public static ConfigEntry<bool> TimeChanges;
        public static ConfigEntry<bool> EnableEvents;
        public static ConfigEntry<bool> EnableDoorEvents;
        public static ConfigEntry<bool> EnableRaidStartEvents;
        public static ConfigEntry<int> EventRangeMin;
        public static ConfigEntry<int> EventRangeMax;
        public static ConfigEntry<int> DoorRangeMin;
        public static ConfigEntry<int> DoorRangeMax;

        public static ConfigEntry<bool> NoJokesHere;
        public static ConfigEntry<bool> Blackout;
        public static ConfigEntry<bool> ArmorRepair;
        public static ConfigEntry<bool> Heal;
        public static ConfigEntry<bool> Airdrop;
        public static ConfigEntry<bool> Skill;
        public static ConfigEntry<bool> Metabolism;
        public static ConfigEntry<bool> Malfunction;
        public static ConfigEntry<bool> Trader;
        public static ConfigEntry<bool> Berserk;
        public static ConfigEntry<bool> Weight;
        public static ConfigEntry<bool> JokesAndFun;
        public static ConfigEntry<bool> ExfilLockdown;

        public static ConfigEntry<bool> PowerOn;
        public static ConfigEntry<bool> DoorUnlock;
        public static ConfigEntry<bool> KDoorUnlock;

        public static void BindConfig(ConfigFile cfg)
        {
            #region Core Events

            EnableRaidStartEvents = cfg.Bind(
                "1. Core Events  (Changing Any Of The Event Sections Requires Restart)",
                "Raid Start Events",
                true,
                new ConfigDescription("Dictates whether you allow the Door and Light randomization events to run on raid start or not.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 7 }));

            EnableEvents = cfg.Bind(
                "1. Core Events  (Changing Any Of The Event Sections Requires Restart)",
                "Dynamic Events",
                true,
                new ConfigDescription("Dictates whether the dynamic event timer should increment and allow events to run or not.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 6 }));

            EnableDoorEvents = cfg.Bind(
                "1. Core Events  (Changing Any Of The Event Sections Requires Restart)",
                "Dynamic Door Events",
                true,
                new ConfigDescription("Dictates whether the dynamic event timer should increment and allow door events to run or not.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 5 }));

            DoorRangeMax = cfg.Bind(
               "1. Core Events  (Changing Any Of The Event Sections Requires Restart)",
               "Door Events timer maximum range",
               2,
               new ConfigDescription("The time is in minutes, cannot be lower than the minimum",
               new AcceptableValueRange<int>(1, 60),
               new ConfigurationManagerAttributes { IsAdvanced = true, ShowRangeAsPercent = false, Order = 4 }));

            DoorRangeMin = cfg.Bind(
               "1. Core Events  (Changing Any Of The Event Sections Requires Restart)",
               "Door Events timer minimum range",
               1,
               new ConfigDescription("The time is in minutes, cannot be higher than the maximum",
               new AcceptableValueRange<int>(1, 60),
               new ConfigurationManagerAttributes { IsAdvanced = true, ShowRangeAsPercent = false, Order = 3 }));

            EventRangeMax = cfg.Bind(
               "1. Core Events  (Changing Any Of The Event Sections Requires Restart)",
               "Random Events timer maximum range",
               20,
               new ConfigDescription("The time is in minutes, cannot be lower than the minimum",
               new AcceptableValueRange<int>(1, 60),
               new ConfigurationManagerAttributes { IsAdvanced = true, ShowRangeAsPercent = false, Order = 2 }));

            EventRangeMin = cfg.Bind(
               "1. Core Events  (Changing Any Of The Event Sections Requires Restart)",
               "Random Events timer minimum range",
               5,
               new ConfigDescription("The time is in minutes, cannot be higher than the maximum",
               new AcceptableValueRange<int>(1, 60),
               new ConfigurationManagerAttributes { IsAdvanced = true, ShowRangeAsPercent = false, Order = 1 }));

            #endregion

            #region Raid Events

            ExfilLockdown = cfg.Bind(
               "2. Raid Events",
               "Lockdown Event",
                true,
                new ConfigDescription("Disable/Enable the Extract Lockdown event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 14 }));

            JokesAndFun = cfg.Bind(
               "2. Raid Events",
               "Joke Event",
                true,
                new ConfigDescription("Disable/Enable the Joke event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 12 }));

            Weight = cfg.Bind(
               "2. Raid Events",
               "Weight Event",
                true,
                new ConfigDescription("Disable/Enable the Weight event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 11 }));

            Berserk = cfg.Bind(
               "2. Raid Events",
               "Berserk Event",
                true,
                new ConfigDescription("Disable/Enable the Berserk event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 10 }));

            Trader = cfg.Bind(
               "2. Raid Events",
               "Trader Events",
                true,
                new ConfigDescription("Disable/Enable the Trader events.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 9 }));

            Malfunction = cfg.Bind(
               "2. Raid Events",
               "Malfunction Event",
                true,
                new ConfigDescription("Disable/Enable the Malfunction event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 8 }));

            NoJokesHere = cfg.Bind(
               "2. Raid Events",
               "Heart Attack Event",
                false,
                new ConfigDescription("Disable/Enable the heart attack event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 7 }));

            Blackout = cfg.Bind(
               "2. Raid Events",
               "Blackout Event",
                true,
                new ConfigDescription("Disable/Enable the blackout event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 6 }));

            ArmorRepair = cfg.Bind(
               "2. Raid Events",
               "Armor Repair Event",
                true,
                new ConfigDescription("Disable/Enable the armor repair event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 5 }));

            Heal = cfg.Bind(
               "2. Raid Events",
               "Heal Event",
                true,
                new ConfigDescription("Disable/Enable the healing event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 4 }));

            Airdrop = cfg.Bind(
               "2. Raid Events",
               "Airdrop Event",
                true,
                new ConfigDescription("Disable/Enable the Airdrop event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 3 }));
            
            Skill = cfg.Bind(
               "2. Raid Events",
               "Skill Event",
                true,
                new ConfigDescription("Disable/Enable the Skill event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 2 }));

            Metabolism = cfg.Bind(
               "2. Raid Events",
               "Metabolism Event",
                true,
                new ConfigDescription("Disable/Enable the Metabolism event.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 1 }));

            #endregion

            #region Door Events

            PowerOn = cfg.Bind(
                "3. Door Events",
                "Power On event",
                true,
                new ConfigDescription("Disable/Enable the event to turn on Power Switches at random throughout the raid.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 3 }));

            DoorUnlock = cfg.Bind(
                "3. Door Events",
                "Door Unlock event",
                true,
                new ConfigDescription("Disable/Enable the event to unlock Doors at random throughout the raid.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 2 }));

            KDoorUnlock = cfg.Bind(
                "3. Door Events",
                "Keycard Door Unlock event",
                true,
                new ConfigDescription("Disable/Enable the event to unlock Keycard Doors at random throughout the raid.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 1 }));

            #endregion
        }
    }
}