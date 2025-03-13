using EFT;
using EFT.Weather;
using UnityEngine;
using ROStandalone.Helpers;
using ROStandalone.Models;

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
        private static WeatherDebug.Direction windDirection;
        private static Vector2 topWindDirection;
        private static bool weatherDebug = false;
        private static bool weatherChangesRun = false;


        public void DoStorm()
        {
            if (Ready() && StormActive() && ConfigController.ServerConfig.Seasons.SeasonalWeatherProgression && weatherChangesRun == false)
            {
                weatherChangesRun = true;

                cloudDensity = 0.05f;
                fog = 0.004f;
                lightningThunderProb = 0.8f;
                rain = 1f;
                temperature = 22f;
                windMagnitude = 0.6f;

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

            if (!Ready() || !ConfigController.ServerConfig.Seasons.SeasonalWeatherProgression)
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