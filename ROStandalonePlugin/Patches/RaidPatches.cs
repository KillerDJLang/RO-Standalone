using EFT;
using System.Reflection;
using System.Threading.Tasks;
using Comfort.Common;
using Newtonsoft.Json;
using UnityEngine;
using EFT.Interactive;
using EFT.Communications;
using SPT.Common.Http;
using SPT.Custom.Airdrops;
using SPT.Custom.Airdrops.Utils;
using SPT.Custom.Airdrops.Models;
using SPT.Reflection.Patching;
using ROStandalone.Controllers;
using SPT.Custom.BTR.Utils;

namespace ROStandalone.Patches
{
        public class EventExfilPatch : ModulePatch
    {
        internal static bool IsLockdown = false;

        internal static bool awaitDrop = false;

        protected override MethodBase GetTargetMethod() => typeof(ExfiltrationRequirement).GetMethod("Met", BindingFlags.Instance | BindingFlags.Public);

        [PatchPostfix]
        static void Postfix(Player player, ref bool __result)
        {
            if (player.IsYourPlayer)
            {
                if (IsLockdown)
                {
                    NotificationManagerClass.DisplayMessageNotification("Cannot extract during a lockdown", ENotificationDurationType.Long, ENotificationIconType.Alert);
                }
            }
            __result = true;
        }
    }

    public class RandomizeDefaultStatePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));
        }

        [PatchPrefix]
        public static void PatchPrefix()
        {
            DoorController.RandomizeDefaultDoors();
            DoorController.RandomizeLampState();
        }
    }

    public class AirdropBoxPatch : ModulePatch
    {
        internal static bool isExtractCrate = false;
        private static JsonConverter[] _defaultJsonConverters;

        protected override MethodBase GetTargetMethod() { return typeof(AirdropsManager).GetMethod("BuildLootContainer", BindingFlags.Instance | BindingFlags.NonPublic); }

        [PatchPrefix]
        static bool Prefix(ref ItemFactoryUtil ___factory, ref AirdropParametersModel ___airdropParameters, AirdropBox ___airdropBox)
        {
            if (!isExtractCrate) return true;
            BuildCrate(___airdropBox);
            ___airdropParameters.AirdropAvailable = true;
            return false;
        }

        [PatchPostfix]
        static void Postfix(ref AirdropBox ___airdropBox, ref AirdropParametersModel ___airdropParameters)
        {
            if (!isExtractCrate) return;
            AwaitThenGetBox(___airdropParameters, ___airdropBox.container);
        }

        static void BuildCrate(AirdropBox airdrop)
        {
            var itemCrate = Singleton<ItemFactory>.Instance.CreateItem("exfilcratecontainer", "6223349b3136504a544d1608", null);
            LootItem.CreateLootContainer(airdrop.container, itemCrate, "Heavy crate", Singleton<GameWorld>.Instance);
        }

        static async void AwaitThenGetBox(AirdropParametersModel param, LootableContainer box)
        {
            if (!isExtractCrate) return;
            isExtractCrate = false;

            while (Vector3.Distance(box.transform.position, param.RandomAirdropPoint) > 3f)
            {
                await Task.Yield();
            }

            while (Vector3.Distance(box.transform.position, ((IPlayer)Singleton<GameWorld>.Instance.MainPlayer).Position) > 15f)
            {
                await Task.Yield();
            }

            NotificationManagerClass.DisplayMessageNotification("The extract crate is open, stash your loot while you can!", ENotificationDurationType.Long, ENotificationIconType.Default);

            EventExfilPatch.awaitDrop = true;

            await Task.Delay(150000);

            EventController._eventisRunning = false;

            NotificationManagerClass.DisplayMessageNotification("The extract crate is locked, and any gear within it is now secured and will be returned to your stash at the end of the raid.", ENotificationDurationType.Long, ENotificationIconType.Default);

            typeof(LootableContainer).GetMethod("Lock", BindingFlags.Instance | BindingFlags.Public).Invoke(box, null);

            sendExfilBox(box);

            EventExfilPatch.awaitDrop = false;
            EventController._mouseInputCount = 0;
        }

        static void sendExfilBox(LootableContainer airdropBox)
        {
            var exfilCrateItems = Singleton<ItemFactory>.Instance.TreeToFlatItems(airdropBox.ItemOwner.MainStorage[0].Items);

            RequestHandler.PutJson("/singleplayer/traderServices/itemDelivery", new
            {
                items = exfilCrateItems,
                traderId = BTRUtil.BTRTraderId
            }.ToJson(_defaultJsonConverters));
        }
    }
}