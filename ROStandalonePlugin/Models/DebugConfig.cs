using Newtonsoft.Json;

namespace ROStandalone.Models
{
    public struct DebugConfigs
    {
        [JsonProperty("debugMode")]
        public bool DebugMode;

        [JsonProperty("EnableTimeChanges")]
        public bool TimeChanges;
    }
}