using Newtonsoft.Json;

namespace DJsROStandalone.Helpers
{
    public struct Weightings
    {
        public DoorWeightings DoorEvents;

        public RaidEventWeightings RaidEvents;
    }

    public struct DoorWeightings
    {
        [JsonProperty("_switchWeighting")]
        public int SwitchWeights;

        [JsonProperty("_doorWeighting")]
        public int LockedDoorWeights;

        [JsonProperty("_keycardWeighting")]
        public int KeycardWeights;
    }

    public struct RaidEventWeightings
    {
        [JsonProperty("_damageWeighting")]
        public int DamageEventWeights;

        [JsonProperty("_airdropWeighting")]
        public int AirdropEventWeights;

        [JsonProperty("_blackoutWeighting")]
        public int BlackoutEventWeights;

        [JsonProperty("_jokeWeighting")]
        public int JokeEventWeights;

        [JsonProperty("_healWeighting")]
        public int HealEventWeights;

        [JsonProperty("_armorWeighting")]
        public int ArmorEventWeights;

        [JsonProperty("_skillWeighting")]
        public int SkillEventWeights;

        [JsonProperty("_metWeighting")]
        public int MetabolismEventWeights;

        [JsonProperty("_malfWeighting")]
        public int MalfEventWeights;

        [JsonProperty("_traderWeighting")]
        public int TraderEventWeights;

        [JsonProperty("_berserkWeighting")]
        public int BerserkEventWeights;

        [JsonProperty("_weightWeightingLOL")]
        public int WeightEventWeights;

        [JsonProperty("_exfilWeighting")]
        public int ExfilEventWeights;
    }
}
