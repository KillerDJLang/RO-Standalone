using EFT;
using EFT.Weather;
using UnityEngine;
using ROStandalone.Helpers;

using static ROStandalone.Plugin;

namespace ROStandalone.Controllers
{
    public class SeasonalWeatherController : MonoBehaviour
    {
        private static WeatherController weatherController => WeatherController.Instance;
        private static float cloudDensity;
        private static float fog;
        private static float rain;
        private static float lightningThunderProb;
        private static float temperature;
        private static float windMagnitude;
        private static int windDir = 2;
        private static WeatherDebug.Direction windDirection;
        private static int topWindDir = 2;
        private static Vector2 topWindDirection;
        private static bool weatherDebug = false;
        private static bool weatherChangesRun = false;


        public void DoStorm()
        {
            if (Ready() && StormActive() && ConfigController.ServerConfig.Events.SeasonalWeatherProgression && weatherChangesRun == false)
            {
                weatherChangesRun = true;

                cloudDensity = 0.05f;
                fog = 0.004f;
                lightningThunderProb = 0.8f;
                rain = 1f;
                temperature = 22f;
                windDir = Random.Range(1, 8);
                windMagnitude = 0.6f;
                topWindDir = Random.Range(0, 5);

                weatherController.WeatherDebug.Enabled = weatherDebug;
                weatherController.WeatherDebug.CloudDensity = cloudDensity;
                
                Utils.FogField.SetValue(weatherController.WeatherDebug, fog);
                Utils.LighteningThunderField.SetValue(weatherController.WeatherDebug, lightningThunderProb);
                Utils.RainField.SetValue(weatherController.WeatherDebug, rain);
                Utils.TemperatureField.SetValue(weatherController.WeatherDebug, temperature);
                
                weatherController.WeatherDebug.TopWindDirection = topWindDirection;
                weatherController.WeatherDebug.WindDirection = windDirection;
                weatherController.WeatherDebug.WindMagnitude = windMagnitude;
                return;
            }

            if (!Ready() || !ConfigController.ServerConfig.Events.SeasonalWeatherProgression)
            {
                weatherChangesRun = false;
                return;
            }
        } 

        public bool StormActive()
        {
            ConfigController.SeasonConfig = Utils.Get<SeasonalConfig>("/ROStandaloneBackend/GetWeatherConfig");

            if (ConfigController.SeasonConfig.SeasonsProgression == 4 || ConfigController.SeasonConfig.SeasonsProgression == 5 || ConfigController.SeasonConfig.SeasonsProgression == 6 )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool Ready()
        {
            return ROGameWorld != null && ROGameWorld.AllAlivePlayersList != null && ROGameWorld.AllAlivePlayersList.Count > 0 && !(ROPlayer is HideoutPlayer);
        }
    }
}