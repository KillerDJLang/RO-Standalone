using System;
using System.Reflection;
using TMPro;
using HarmonyLib;
using EFT.UI.Map;
using EFT.UI.Matchmaker;
using SPT.Reflection.Patching;

using EFT;
using EFT.UI;
using JsonType;
using ROStandalone.Helpers;

namespace ROStandalone.Patches
{

    public struct RaidTime
    {
        internal static bool inverted = false;

        private static DateTime inverseTime
        {
            get
            {
                DateTime result = DateTime.Now.AddHours(12);
                return result.Day > DateTime.Now.Day
                       ? result.AddDays(-1)
                       : result.Day < DateTime.Now.Day
                       ? result.AddDays(1) : result;
            }
        }

        public static DateTime GetCurrTime() => DateTime.Now;
        public static DateTime GetInverseTime() => inverseTime;
        public static DateTime GetDateTime() => inverted ? GetInverseTime() : GetCurrTime();
    }

    public class EnableEntryPointPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(EntryPointView).GetMethod("Show", BindingFlags.Instance | BindingFlags.Public);

        [PatchPrefix]
        static void Prefix(ref bool allowSelection) => allowSelection = true;
    }
    
    public class WatchPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(Watch).GetProperty("DateTime_0", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(true);

        [PatchPostfix]
        static void Postfix(ref DateTime __result)
        {
            __result = RaidTime.GetDateTime();
        }
    }

    public class TimePanelPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MatchMakerSelectionLocationScreen), "method_6");
        }

        [PatchPostfix]
        public static void Postfix(MatchMakerSelectionLocationScreen __instance, LocationConditionsPanel ____conditions)
        {
            ____conditions.Close();

        }
    }

    public class RaidSettingsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LocationConditionsPanel), nameof(LocationConditionsPanel.Set));
        }

        [PatchPostfix]
        public static void Postfix(LocationConditionsPanel __instance, ref RaidSettings raidSettings)
        {
            if (Utils.IsFactory(raidSettings.LocationId))
            {
                if (Utils.IsDay(__instance.DateTime_2))
                {
                    raidSettings.SelectedDateTime = EDateTime.CURR;
                }
                else
                {
                    raidSettings.SelectedDateTime = EDateTime.PAST;
                }
            }
            else
            {
                raidSettings.SelectedDateTime = EDateTime.CURR;
            }
        }
    }

    public class LocationInfoPanelPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LocationInfoPanel), nameof(LocationInfoPanel.Set));
        }

        [PatchPostfix]
        public static void Postfix(LocationInfoPanel __instance, ref TextMeshProUGUI ____playTime, LocationSettingsClass.Location location)
        {
            DateTime dateTime;
            if(location == null) { return; }

            if(Utils.IsFactory(location.Id))
            {
                DateTime backendTime = Utils.GetDateTime();
                if (Utils.IsDay(backendTime)) 
                {
                    dateTime = LocationConditionsPanel.DateTime_0;
                }
                else 
                {
                    dateTime = LocationConditionsPanel.DateTime_1;
                }
            }
            else
            {
                dateTime = Utils.GetDateTime();
            }

            ____playTime.text = dateTime.ToString("HH:mm:ss");


        }
    }
}