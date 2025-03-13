using System.Reflection;
using EFT.Weather;
using SPT.Reflection.Patching;

namespace ROStandalone.Patches
{
    public class WeatherControllerPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(WeatherController).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public);

        [PatchPostfix]
        static void Postfix(ref WeatherController __instance) => __instance.WindController.CloudWindMultiplier = 1;
    }
}