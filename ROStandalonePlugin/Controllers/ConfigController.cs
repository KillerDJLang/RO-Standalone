using UnityEngine;
using ROStandalone.Helpers;

namespace ROStandalone.Controllers
{
    public class ConfigController : MonoBehaviour
    {
        public static ServerConfigs ServerConfig = new ServerConfigs();
        public static EventsConfig EventConfig = new EventsConfig();
        public static SeasonalConfig SeasonConfig = new SeasonalConfig();
    }
}