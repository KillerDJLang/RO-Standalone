using Newtonsoft.Json;

namespace ROStandalone.Models
{
    #region Server Config Layout Base
    public struct ServerConfigs
    {
        public RaidServer RaidChanges;
        public LootChangesServer LootChanges;
        public SeasonsServer Seasons;
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

    #region Season Changes Config
    public struct SeasonsServer
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
    }
    #endregion
}