using Newtonsoft.Json;

namespace ROStandalone.Models
{
    public struct SeasonalConfig
    {
        [JsonProperty("seasonsProgression")]
        public int SeasonsProgression;
    }
}