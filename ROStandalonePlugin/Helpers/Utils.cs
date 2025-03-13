using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Comfort.Common;
using HarmonyLib;
using BepInEx.Logging;
using Newtonsoft.Json;
using SPT.Common.Http;
using EFT;
using EFT.Weather;
using EFT.InventoryLogic;

namespace ROStandalone.Helpers
{
    public static class Utils
    {
        public static FieldInfo FogField;
        public static FieldInfo LighteningThunderField;
        public static FieldInfo RainField;
        public static FieldInfo TemperatureField;
        public static readonly List<string> Traders = new List<string>
        {
            "54cb50c76803fa8b248b4571",     //Prapor
            "54cb57776803fa99248b456e",     //Therapist 
            "579dc571d53a0658a154fbec",     //Fence
            "58330581ace78e27b8b10cee",     //Skier 
            "5935c25fb3acc3127c3d8cd9",     //Peacekeeper 
            "5a7c2eca46aef81a7ca2145d",     //Mechanic 
            "5ac3b934156ae10c4430e83c",     //Ragman 
            "5c0647fdd443bc2504c2d371"      //Jaeger
        };
        
        public static readonly string MainROKey = "DJ.RaidOverhaul";
        public static readonly string specialExfilFlare = "67cda57f8f59300db5c0ec5b";
        public static readonly string trainFlare = "67cde31eea2d15e888fa7dee";
        public static readonly string redFlare = "624c09cfbc2e27219346d955";
        public static readonly string Heal = "Heal";
        public static readonly string Damage = "Damage";
        public static readonly string Repair = "Repair";
        public static readonly string Airdrop = "Airdrop";
        public static readonly string Jokes = "Jokes";
        public static readonly string Blackout = "Blackout";
        public static readonly string Skill = "Skill";
        public static readonly string Metabolism = "Metabolism";
        public static readonly string Malf = "Malf";
        public static readonly string LoyaltyLevel = "LoyaltyLevel";
        public static readonly string Berserk = "Berserk";
        public static readonly string Weight = "Weight";
        public static readonly string MaxLoyaltyLevel = "MaxLoyaltyLevel";
        public static readonly string CorrectRep = "CorrectRep";
        public static readonly string Lockdown = "Lockdown";
        public static readonly string GearExfilEvent = "GearExfilEvent";
        public static readonly string Train = "Train";
        public static readonly string PmcExfil = "PmcExfil";
        public static readonly string Artillery = "Artillery";
	    private static readonly Dictionary<string, ItemTemplate> templates = [];
        private static readonly JsonConverter[] _defaultJsonConverters;

        public static T Get<T>(string url)
        {
            var req = RequestHandler.GetJson(url);

            if (string.IsNullOrEmpty(req))
            {
                throw new InvalidOperationException("The response from the server is null or empty.");
            }

            return JsonConvert.DeserializeObject<T>(req);
        }

        public static void LogToServerConsole(string message) {
            Plugin.Log.Log( LogLevel.Info, message);
            string info = JsonConvert.SerializeObject(message);
            RequestHandler.PostJson("/ROStandaloneBackend/LogToServer", info);
        }

        public static void GetWeatherFields()
        {
            FogField = AccessTools.Field(typeof(WeatherDebug), "Fog");
            LighteningThunderField = AccessTools.Field(typeof(WeatherDebug), "LightningThunderProbability");
            RainField = AccessTools.Field(typeof(WeatherDebug), "Rain");
            TemperatureField = AccessTools.Field(typeof(WeatherDebug), "Temperature");
        }
        
        public static bool IsDay(DateTime time)
        {
            if (time.Hour > 5 && time.Hour < 21)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsFactory(string location)
        {
            if (location == "factory4_day" || location == "factory4_night")
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public static DateTime GetDateTime()
        {
            TarkovApplication.Exist(out TarkovApplication tarkovApplication);
            DateTime dateTime = tarkovApplication.Session.GetCurrentLocationTime;
            return dateTime;
        }

        private static void AddTemplatesToArray()
        {
            if (!Singleton<ItemFactoryClass>.Instantiated) { return; }

            var mongoTemplates = Singleton<ItemFactoryClass>.Instance.ItemTemplates;
            
            if (templates.Count == mongoTemplates.Count) { return; }
            foreach (var keyValuePair in mongoTemplates) { templates.Add(keyValuePair.Key.ToString(), keyValuePair.Value); }
        }

        public static ItemTemplate[] FindTemplates(string templateToFind)
        {
            AddTemplatesToArray();
            if (templates.TryGetValue(templateToFind, out var template)) { return [template]; }
            return [.. templates.Values.Where(t => t.ShortNameLocalizationKey.Localized().IndexOf(templateToFind, StringComparison.OrdinalIgnoreCase) >= 0
                                                || t.NameLocalizationKey.Localized().IndexOf(templateToFind, StringComparison.OrdinalIgnoreCase) >= 0)];
        }
    }
}
