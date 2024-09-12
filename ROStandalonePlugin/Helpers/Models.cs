using Newtonsoft.Json;

namespace ROStandalone.Helpers
{
    #region Server Config Layout Base
    public struct ServerConfigs
    {
        public RaidServer RaidChanges;
        public LootChangesServer LootChanges;
        public EventsServer Events;
        public DebugServer Debug;
    }

    public struct EventsConfig
    {
        public DoorWeightings DoorEvents;

        [JsonProperty("DoorEventRangeMinimum")]
        public float DoorEventRangeMinimumServer;

        [JsonProperty("DoorEventRangeMaximum")]
        public float DoorEventRangeMaximumServer;

        public RaidEventWeightings RaidEvents;

        [JsonProperty("RandomEventRangeMinimum")]
        public float RandomEventRangeMinimumServer;

        [JsonProperty("RandomEventRangeMaximum")]
        public float RandomEventRangeMaximumServer;
    }

    public struct SeasonalConfig
    {
        [JsonProperty("seasonsProgression")]
        public int SeasonsProgression;
    }
    #endregion

    #region Event Weightings
    public struct DoorWeightings
    {
        [JsonProperty("SwitchToggle")]
        public int SwitchWeights;

        [JsonProperty("DoorUnlock")]
        public int LockedDoorWeights;

        [JsonProperty("KeycardUnlock")]
        public int KeycardWeights;
    }

    public struct RaidEventWeightings
    {
        [JsonProperty("DamageEvent")]
        public int DamageEventWeights;

        [JsonProperty("AirdropEvent")]
        public int AirdropEventWeights;

        [JsonProperty("BlackoutEvent")]
        public int BlackoutEventWeights;

        [JsonProperty("JokeEvent")]
        public int JokeEventWeights;

        [JsonProperty("HealEvent")]
        public int HealEventWeights;

        [JsonProperty("ArmorEvent")]
        public int ArmorEventWeights;

        [JsonProperty("SkillEvent")]
        public int SkillEventWeights;

        [JsonProperty("MetabolismEvent")]
        public int MetabolismEventWeights;

        [JsonProperty("MalfunctionEvent")]
        public int MalfEventWeights;

        [JsonProperty("TraderEvent")]
        public int TraderEventWeights;

        [JsonProperty("BerserkEvent")]
        public int BerserkEventWeights;

        [JsonProperty("WeightEvent")]
        public int WeightEventWeights;

        [JsonProperty("MaxLLEvent")]
        public int MaxLLEventWeights;

        [JsonProperty("ExfilEvent")]
        public int ExfilEventWeights;
    }
    #endregion

    #region Raid Config
    public struct RaidServer
    {
        public ReduceFoodAndHydroDegradeRaid ReduceFoodAndHydroDegrade;

        [JsonProperty("EnableExtendedRaids")]
        public bool EnableExtendedRaids;

        [JsonProperty("TimeLimit")]
        public int RaidTimeLimit;
    }

    public struct ReduceFoodAndHydroDegradeRaid
    {
        [JsonProperty("Enabled")]
        public bool EnableFoodAndHydroDegrade;

        [JsonProperty("EnergyDecay")]
        public float EnergyDecay;

        [JsonProperty("HydroDecay")]
        public float HydroDecay;
    }
    #endregion

    #region Loot Changes Config
    public struct LootChangesServer
    {
        [JsonProperty("Enabled")]
        public bool EnableLootChanges;

        [JsonProperty("StaticLootMultiplier")]
        public float StaticLootMulti;

        [JsonProperty("LooseLootMultiplier")]
        public float LooseLootMulti;

        [JsonProperty("MarkedRoomLootMultiplier")]
        public float MarkedRoomMulti;
    }
    #endregion

    #region Event Changes Config
    public struct EventsServer
    {
        [JsonProperty("EnableWeatherOptions")]
        public bool EnableWeatherChanges;

        [JsonProperty("AllSeasons")]
        public bool AllSeasonsRandomized;

        [JsonProperty("NoWinter")]
        public bool NoWinterRandomized;

        [JsonProperty("SeasonalProgression")]
        public bool SeasonalWeatherProgression;

        [JsonProperty("WinterWonderland")]
        public bool EnableWinterOnly;

        [JsonProperty("RandomizedSeasonalEvents")]
        public bool EnableRandomizedSeasonalEvents;
    }
    #endregion

    #region Debug Logging Server Config
    public struct DebugServer
    {
        [JsonProperty("ExtraLogging")]
        public bool EnableExtraDebugLogging;
    }
    #endregion
}