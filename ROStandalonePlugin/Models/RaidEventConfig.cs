using Newtonsoft.Json;

namespace ROStandalone.Models
{
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

        [JsonProperty("LockdownEvent")]
        public int ExfilEventWeights;

        [JsonProperty("ArtilleryEvent")]
        public int ArtilleryEventWeights;
    }
    
    public struct DoorWeightings
    {
        [JsonProperty("SwitchToggle")]
        public int SwitchWeights;

        [JsonProperty("DoorUnlock")]
        public int LockedDoorWeights;

        [JsonProperty("KeycardUnlock")]
        public int KeycardWeights;
    }
}