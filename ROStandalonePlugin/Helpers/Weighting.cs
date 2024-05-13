using System;
using System.Linq;
using System.Collections.Generic;

namespace DJsROStandalone.Helpers
{
    public static class Weighting
    {
        public static List<(Action, int)> weightedEvents;
        public static List<(Action, int)> weightedDoorMethods;

        public static Weightings EventWeights = new Weightings();

        public static void InitWeightings()
        {
            InitDoorWeighting();
            InitEventWeighting();
        }

        public static void DoRandomEvent(List<(Action, int)> weighting)
        {
            // Shuffle the list to randomize the order
            weighting = weighting.OrderBy(_ => Guid.NewGuid()).ToList();

            // Calculate total weight
            int totalWeight = weighting.Sum(pair => pair.Item2);

            // Generate a random number between 1 and totalWeight
            int randomNum = new System.Random().Next(1, totalWeight + 1);

            // Find the method to call based on the random number
            foreach (var (method, weight) in weighting)
            {
                randomNum -= weight;
                if (randomNum <= 0)
                {
                    // Call the selected method
                    method();
                    break;
                }
            }
        }

        private static void InitDoorWeighting()
        {
            var _switchWeighting = DJConfig.PowerOn.Value ? EventWeights.DoorEvents.SwitchWeights : 0;
            var _doorWeighting = DJConfig.DoorUnlock.Value ? EventWeights.DoorEvents.LockedDoorWeights : 0;
            var _keycardWeighting = DJConfig.KDoorUnlock.Value ? EventWeights.DoorEvents.KeycardWeights : 0;

            weightedDoorMethods = new List<(Action, int)>
            {
                (Plugin.DCScript.PowerOn,     _switchWeighting),
                (Plugin.DCScript.DoUnlock,    _doorWeighting),
                (Plugin.DCScript.DoKUnlock,   _keycardWeighting)
            };
        }

        private static void InitEventWeighting()
        {
            var _damageWeighting = DJConfig.NoJokesHere.Value ? EventWeights.RaidEvents.DamageEventWeights : 0;
            var _airdropWeighting = DJConfig.Airdrop.Value ? EventWeights.RaidEvents.AirdropEventWeights : 0;
            var _blackoutWeighting = DJConfig.Blackout.Value ? EventWeights.RaidEvents.BlackoutEventWeights : 0;
            var _jokeWeighting = DJConfig.JokesAndFun.Value ? EventWeights.RaidEvents.JokeEventWeights : 0;
            var _healWeighting = DJConfig.Heal.Value ? EventWeights.RaidEvents.HealEventWeights : 0;
            var _armorWeighting = DJConfig.ArmorRepair.Value ? EventWeights.RaidEvents.ArmorEventWeights : 0;
            var _skillWeighting = DJConfig.Skill.Value ? EventWeights.RaidEvents.SkillEventWeights : 0;
            var _metWeighting = DJConfig.Metabolism.Value ? EventWeights.RaidEvents.MetabolismEventWeights : 0;
            var _malfWeighting = DJConfig.Malfunction.Value ? EventWeights.RaidEvents.MalfEventWeights : 0;
            var _traderWeighting = DJConfig.Trader.Value ? EventWeights.RaidEvents.TraderEventWeights : 0;
            var _berserkWeighting = DJConfig.Berserk.Value ? EventWeights.RaidEvents.BerserkEventWeights : 0;
            var _weightWeightingLOL = DJConfig.Weight.Value ? EventWeights.RaidEvents.WeightEventWeights : 0;
            var _exfilWeighting = DJConfig.ExfilLockdown.Value ? EventWeights.RaidEvents.ExfilEventWeights : 0;

            weightedEvents = new List<(Action, int)>
            {
                (Plugin.ECScript.DoDamageEvent,     _damageWeighting),
                (Plugin.ECScript.DoAirdropEvent,    _airdropWeighting),
                (Plugin.ECScript.DoBlackoutEvent,   _blackoutWeighting),
                (Plugin.ECScript.DoFunny,           _jokeWeighting),
                (Plugin.ECScript.DoHealPlayer,      _healWeighting),
                (Plugin.ECScript.DoArmorRepair,     _armorWeighting),
                (Plugin.ECScript.DoSkillEvent,      _skillWeighting),
                (Plugin.ECScript.DoMetabolismEvent, _metWeighting),
                (Plugin.ECScript.DoMalfEvent,       _malfWeighting),
                (Plugin.ECScript.DoLLEvent,         _traderWeighting),
                (Plugin.ECScript.DoBerserkEvent,    _berserkWeighting),
                (Plugin.ECScript.DoWeightEvent,     _weightWeightingLOL),
                (Plugin.ECScript.DoLockDownEvent,   _exfilWeighting)
            };
        }
    }
}
