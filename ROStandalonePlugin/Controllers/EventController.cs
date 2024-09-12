using EFT;
using EFT.UI;
using JsonType;
using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using EFT.Interactive;
using EFT.HealthSystem;
using EFT.UI.BattleTimer;
using EFT.InventoryLogic;
using EFT.Communications;
using SPT.Custom.Airdrops;
using System.Threading.Tasks;
using ROStandalone.Helpers;
using ROStandalone.Patches;

using static ROStandalone.Plugin;


namespace ROStandalone.Controllers
{
    public class EventController : MonoBehaviour
    {
        private bool _exfilUIChanged = false;

        public static bool _eventisRunning = false;
        private bool _airdropDisabled = false;
        private bool _metabolismDisabled = false;
        private bool _jokeEventHasRun = false;
        private bool _airdropEventHasRun = false;
        private bool _berserkEventHasRun = false;
        private bool _malfEventHasRun = false;
        private bool _weightEventHasRun = false;

        private int _skillEventCount = 0;
        private int _repairEventCount = 0;
        private int _healthEventCount = 0;
        private int _damageEventCount = 0;
        private int _maxLLEventCount = 0;
        private int _exfilEventCount = 0;

        public static int _mouseInputCount = 0;

        private Switch[] _pswitchs = null;
        private KeycardDoor[] _keydoor = null;
        private LampController[] _lamp = null;

        public DamageInfo Blunt { get; private set; }

        public EExfiltrationStatus AwaitsManualActivation { get; private set; }

        private class OriginalWeaponStatsBers
        {
            public float malfChance;
            public float duraBurn;
            public float ergo;
            public float recoilBack;
            public float recoilUp;
        }

        private class OriginalWeaponStatsMalf
        {
            public float malfChance;
            public float duraBurn;
            public float ergo;
        }

        private Dictionary<string, OriginalWeaponStatsBers> _originalWSBers = new Dictionary<string, OriginalWeaponStatsBers>();
        private Dictionary<string, OriginalWeaponStatsMalf> _originalWSMalf = new Dictionary<string, OriginalWeaponStatsMalf>();
        private IEnumerable<Item> _allWeapons => Session.Profile.Inventory.AllRealPlayerItems;

        private const string _whiteFlare = "62178be9d0050232da3485d9";

        void Update()
        {
            if (!Ready() || !DJConfig.EnableEvents.Value)
            {
                // Reset Events
                if (_airdropDisabled != false)      { _airdropDisabled = false; }
                if (_metabolismDisabled != false)   { _metabolismDisabled = false; }
                if (_jokeEventHasRun != false)      { _jokeEventHasRun = false; }
                if (_airdropEventHasRun != false)   { _airdropEventHasRun = false; }
                if (_berserkEventHasRun != false)   { _berserkEventHasRun = false; }
                if (_malfEventHasRun != false)      { _malfEventHasRun = false; }
                if (_weightEventHasRun != false)    { _weightEventHasRun = false; }

                if (_skillEventCount != 0)          { _skillEventCount = 0; }
                if (_repairEventCount != 0)         { _repairEventCount = 0; }
                if (_healthEventCount != 0)         { _healthEventCount = 0; }
                if (_damageEventCount != 0)         { _damageEventCount = 0; }
                if (_maxLLEventCount != 0)          { _maxLLEventCount = 0; }
                if (_exfilEventCount != 0)          { _exfilEventCount = 0; }

                return;
            }

            if (_pswitchs == null)
            {
                _pswitchs = FindObjectsOfType<Switch>();
            }

            if (_keydoor == null)
            {
                _keydoor = FindObjectsOfType<KeycardDoor>();
            }

            if (_lamp == null)
            {
                _lamp = FindObjectsOfType<LampController>();
            }


            if (!_eventisRunning)
            {
                StaticManager.Instance.StartCoroutine(StartEvents());

                _eventisRunning = true;
            }

            if (EventExfilPatch.IsLockdown)
            {
                if (!_exfilUIChanged)
                {
                    ChangeExfilUI();
                }
            }

            if (Ready())
            {
                FlareLogicEC();
            }
        }

        private IEnumerator StartEvents()
        {
            yield return new WaitForSeconds(Random.Range(ConfigController.EventConfig.RandomEventRangeMinimumServer, ConfigController.EventConfig.RandomEventRangeMaximumServer) * 60f);

            if (ROGameWorld != null && ROGameWorld.AllAlivePlayersList != null && ROGameWorld.AllAlivePlayersList.Count > 0 && !(ROPlayer is HideoutPlayer))
            {
                Weighting.DoRandomEvent(Weighting.weightedEvents);
            }

            else
            {
                _pswitchs = null;
                _keydoor = null;
                _lamp = null;
            }

            _eventisRunning = false;
            yield break;
        }

        async void ChangeExfilUI()
        {
            if (EventExfilPatch.IsLockdown)
            {
                Color red = new Color(0.8113f, 0.0376f, 0.0714f, 0.8627f);
                Color green = new Color(0.4863f, 0.7176f, 0.0157f, 0.8627f);
                RectTransform mainDescription = (RectTransform)typeof(ExtractionTimersPanel).GetField("_mainDescription", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(FindObjectOfType<ExtractionTimersPanel>());

                var text = mainDescription.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                var box = mainDescription.gameObject.GetComponentInChildren<Image>();

                text.text = EventExfilPatch.IsLockdown ? "Extraction unavailable" : "Find an extraction point";
                box.color = red;

                foreach (ExitTimerPanel panel in FindObjectsOfType<ExitTimerPanel>())
                    panel.enabled = false;

                _exfilUIChanged = true;

                while (EventExfilPatch.IsLockdown)
                    await Task.Yield();

                text.text = "Find an extraction point";
                box.color = green;

                foreach (ExitTimerPanel panel in FindObjectsOfType<ExitTimerPanel>())
                    panel.enabled = true;

                _exfilUIChanged = false;
            }
        }

        #region Core Events Controller

        public void DoHealPlayer()
        {
            if (_healthEventCount >= 2) { return; }

            NotificationManagerClass.DisplayMessageNotification("Heal Event: On your feet you ain't dead yet.", ENotificationDurationType.Long, ENotificationIconType.Default);
            ROPlayer.ActiveHealthController.RestoreFullHealth();
                _healthEventCount++;

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Heal Event has run");
            }
#endif
        }

        public void DoDamageEvent()
        {
            if (_damageEventCount >= 1) { return; }

            NotificationManagerClass.DisplayMessageNotification("Heart Attack Event: Better get to a medic quick, you don't have long left.", ENotificationDurationType.Long, ENotificationIconType.Alert);
            ROPlayer.ActiveHealthController.DoContusion(4f, 50f);
            ROPlayer.ActiveHealthController.DoStun(5f, 0f);
            ROPlayer.ActiveHealthController.DoFracture(EBodyPart.LeftArm);
            ROPlayer.ActiveHealthController.ApplyDamage(EBodyPart.Chest, 65f, Blunt);
                _damageEventCount++;

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Heart Attack Event has run");
            }
#endif
        }

        public void DoArmorRepair()
        {
            if (_repairEventCount >= 2) { return; }

            NotificationManagerClass.DisplayMessageNotification("Armor Repair Event: All equipped armor repaired... nice!", ENotificationDurationType.Long, ENotificationIconType.Default);
            ROPlayer.Profile.Inventory.GetPlayerItems().ExecuteForEach((item) =>
            {
                if (item.GetItemComponent<ArmorComponent>() != null) item.GetItemComponent<RepairableComponent>().Durability = item.GetItemComponent<RepairableComponent>().MaxDurability;
                    _repairEventCount++;
            });

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Armor Repair Event has run");
            }
#endif            
        }

        public void DoAirdropEvent()
        {
            if (ROPlayer.Location != "factory4_day" && ROPlayer.Location != "factory4_night" && ROPlayer.Location != "laboratory" && ROPlayer.Location != "sandbox" && !_airdropEventHasRun)
            {
                ROGameWorld.gameObject.AddComponent<AirdropsManager>().isFlareDrop = true;
                NotificationManagerClass.DisplayMessageNotification("Aidrop Event: Incoming Airdrop!", ENotificationDurationType.Long, ENotificationIconType.Quest);

                _airdropEventHasRun = true;

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Aidrop Event has run");
            }
#endif         
            }

            else
            {
                Weighting.DoRandomEvent(Weighting.weightedEvents);
            }
        }

        public async void DoFunny()
        {
            if (!_jokeEventHasRun)
            {
                NotificationManagerClass.DisplayMessageNotification("Heart Attack Event: Nice knowing ya, you've got 10 seconds", ENotificationDurationType.Long, ENotificationIconType.Alert);

                await Task.Delay(10000);

                NotificationManagerClass.DisplayMessageNotification("jk", ENotificationDurationType.Long, ENotificationIconType.Quest);

                await Task.Delay(2000);

                DoHealPlayer();

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Joke Event has run");
            }
#endif 

                _jokeEventHasRun = true;
            }

            if (_jokeEventHasRun)
            {
                Weighting.DoRandomEvent(Weighting.weightedEvents);
            }
        }

        public async void DoBlackoutEvent()
        {
                foreach (Switch pSwitch in _pswitchs)
                {
                    typeof(Switch).GetMethod("Close", BindingFlags.Instance | BindingFlags.Public).Invoke(pSwitch, null);
                    typeof(Switch).GetMethod("Lock", BindingFlags.Instance | BindingFlags.Public).Invoke(pSwitch, null);
                }

                foreach (LampController lamp in _lamp)
                {
                    lamp.Switch(Turnable.EState.Off);
                    lamp.enabled = false;
                }

                foreach (KeycardDoor door in _keydoor)
                {
                    if (_keydoor != null || _keydoor.Length >= 0)
                    {
                        typeof(KeycardDoor).GetMethod("Unlock", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
                        typeof(KeycardDoor).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public).Invoke(door, null);
                    }
                }

            NotificationManagerClass.DisplayMessageNotification("Blackout Event: All power switches and lights disabled for 10 minutes", ENotificationDurationType.Long, ENotificationIconType.Alert);
#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Blackout Event: All power switches and lights disabled for 10 minutes");
            }
#endif  
                await Task.Delay(600000);

                foreach (Switch pSwitch in _pswitchs)
                {
                    typeof(Switch).GetMethod("Unlock", BindingFlags.Instance | BindingFlags.Public).Invoke(pSwitch, null);
                }

                foreach (LampController lamp in _lamp)
                {
                    lamp.Switch(Turnable.EState.On);
                    lamp.enabled = true;
                }

            NotificationManagerClass.DisplayMessageNotification("Blackout Event over", ENotificationDurationType.Long, ENotificationIconType.Quest);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Blackout Event has run");
            }
#endif              
        }

        public void DoSkillEvent()
        {
            if (_skillEventCount >= 3) { return; }

                System.Random random = new System.Random();

                int chance = random.Next(0, 100 + 1);
                var selectedSkill = ROSkillManager.DisplayList.RandomElement();
                int level = selectedSkill.Level;

                // If the skill is a locked skill, start over.
                if (selectedSkill.Locked == true) { DoSkillEvent(); };

                // 55% chance to roll a skill gain
                // 45% chance to roll a skill loss
                if (chance >= 0 && chance <= 55)
                {
                    if (level > 50 || level < 0) { return; }

                    selectedSkill.SetLevel(level + 1);
                    _skillEventCount++;
                    NotificationManagerClass.DisplayMessageNotification("Skill Event: You've advanced a skill to the next level!", ENotificationDurationType.Long, ENotificationIconType.Quest);
                }
                else
                {
                    if (level <= 0) { return; }

                    selectedSkill.SetLevel(level - 1);
                    _skillEventCount++;
                    NotificationManagerClass.DisplayMessageNotification("Skill Event: You've lost a skill level, unlucky!", ENotificationDurationType.Long, ENotificationIconType.Quest);
                }

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Skill Event has run");
            }
#endif                  
        }

        public void DoMetabolismEvent()
        {
            if (!_metabolismDisabled)
            {
                System.Random random = new System.Random();
                int chance = random.Next(0, 100 + 1);

                // 33% chance to disable metabolism for the raid
                // 33% chance to increase metabolism rate by 20% for the raid
                // 33% chance to reduce metabolism rate by 20% for the raid
                if (chance >= 0 && chance <= 33)
                {
                    ROPlayer.ActiveHealthController.DisableMetabolism();
                    _metabolismDisabled = true;
                    NotificationManagerClass.DisplayMessageNotification("Metabolism Event: You've got an iron stomach, No hunger or hydration drain!", ENotificationDurationType.Long, ENotificationIconType.Quest);
                }
                else if (chance >= 34f && chance <= 66)
                {
                    AccessTools.Property(typeof(ActiveHealthController), "EnergyRate").SetValue(
                        ROPlayer.ActiveHealthController,
                        ROPlayer.ActiveHealthController.EnergyRate * 0.80f);

                    AccessTools.Property(typeof(ActiveHealthController), "HydrationRate").SetValue(
                        ROPlayer.ActiveHealthController,
                        ROPlayer.ActiveHealthController.HydrationRate * 0.80f);

                    NotificationManagerClass.DisplayMessageNotification("Metabolism Event: Your metabolism has slowed. Decreased hunger and hydration drain!", ENotificationDurationType.Long, ENotificationIconType.Quest);
                }
                else if (chance >= 67 && chance <= 100f)
                {
                    AccessTools.Property(typeof(ActiveHealthController), "EnergyRate").SetValue(
                        ROPlayer.ActiveHealthController,
                        ROPlayer.ActiveHealthController.EnergyRate * 1.20f);

                    AccessTools.Property(typeof(ActiveHealthController), "HydrationRate").SetValue(
                        ROPlayer.ActiveHealthController,
                        ROPlayer.ActiveHealthController.HydrationRate * 1.20f);

                    NotificationManagerClass.DisplayMessageNotification("Metabolism Event: Your metabolism has fastened. Increased hunger and hydration drain!", ENotificationDurationType.Long, ENotificationIconType.Quest);
                }
            }

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Metabolism Event has run");
            }
#endif    
        }

        public async void DoMalfEvent()
        {
            var Items = Session.Profile.Inventory.GetItemsInSlots(new EquipmentSlot[] {EquipmentSlot.FirstPrimaryWeapon, EquipmentSlot.SecondPrimaryWeapon});

            if (!_malfEventHasRun)
            {
                _malfEventHasRun = true;

                var tempItems = _allWeapons;

                foreach (var item in tempItems)
                {
                    if (item is Weapon weapon)
                    {
                        var origStats = new OriginalWeaponStatsMalf();

                        origStats.malfChance = weapon.Template.BaseMalfunctionChance;
                        origStats.duraBurn = weapon.Template.DurabilityBurnRatio;
                        origStats.ergo = weapon.Template.Ergonomics;

                        _originalWSMalf.Add(item.TemplateId, origStats);
                    }
                }

                //
                //
                //

                foreach (var item in Items)
                {
                    if (item is Weapon weapon)
                    {
                        weapon.Template.BaseMalfunctionChance = _originalWSMalf[item.TemplateId].malfChance * 3f;
                        weapon.Template.DurabilityBurnRatio = _originalWSMalf[item.TemplateId].duraBurn * 2f;
                        weapon.Template.Ergonomics = _originalWSMalf[item.TemplateId].ergo * 0.5f;
                    }
                }

                NotificationManagerClass.DisplayMessageNotification("Malfunction Event: Be careful not to jam up!", ENotificationDurationType.Long, ENotificationIconType.Alert);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Malfunction Event has started");
            }
#endif    

                await Task.Delay(300000);

                foreach (var item in Items)
                {
                    if (item is Weapon weapon)
                    {
                        weapon.Template.BaseMalfunctionChance = _originalWSMalf[item.TemplateId].malfChance;
                        weapon.Template.DurabilityBurnRatio = _originalWSMalf[item.TemplateId].duraBurn;
                        weapon.Template.Ergonomics = _originalWSMalf[item.TemplateId].ergo;
                    }
                }

                NotificationManagerClass.DisplayMessageNotification("Malfunction Event: Your weapon has had time to cool off, shouldn't have any more troubles!", ENotificationDurationType.Long, ENotificationIconType.Default);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Malfunction Event has run");
            }
#endif  
            }  

            else
            {
                Weighting.DoRandomEvent(Weighting.weightedEvents);
            }
        }

        public void DoLLEvent()
        {
            System.Random random = new System.Random();

            var Trader = Utils.Traders.RandomElement();
            int chance = random.Next(0, 100 + 1);

            if (chance is >= 0 && chance is <= 49)
            {
                Session.Profile.TradersInfo[Trader].SetStanding(Session.Profile.TradersInfo[Trader].Standing + 0.1);
                NotificationManagerClass.DisplayMessageNotification("Trader Event: A random Trader has gained a little more respect for you.", ENotificationDurationType.Default, ENotificationIconType.Achievement);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Trader Rep Gain Event has run");
            }
#endif  
            }

            else if (chance is >= 50 && chance is <= 100)
            {
                if (Session.Profile.TradersInfo[Trader].Standing >= 0.05)
                {
                    Session.Profile.TradersInfo[Trader].SetStanding(Session.Profile.TradersInfo[Trader].Standing - 0.05);
                    NotificationManagerClass.DisplayMessageNotification("Trader Event: A random Trader has lost a little faith in you.", ENotificationDurationType.Default, ENotificationIconType.Achievement);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Trader Rep Loss Event has run");
            }
#endif  
                }

                else
                {
                    Weighting.DoRandomEvent(Weighting.weightedEvents);
                }
            }
        }

        public async void DoBerserkEvent()
        {
            var Items = Session.Profile.Inventory.GetItemsInSlots(new EquipmentSlot[] {EquipmentSlot.FirstPrimaryWeapon, EquipmentSlot.SecondPrimaryWeapon});

            if (!_berserkEventHasRun)
            {
                _berserkEventHasRun = true;

                var tempItems = _allWeapons;

                foreach (var item in tempItems)
                {
                    if (item is Weapon weapon)
                    {
                        var origStats = new OriginalWeaponStatsBers();

                        origStats.ergo = weapon.Template.Ergonomics;
                        origStats.duraBurn = weapon.Template.DurabilityBurnRatio;
                        origStats.malfChance = weapon.Template.BaseMalfunctionChance;
                        origStats.recoilBack = weapon.Template.RecoilForceBack;
                        origStats.recoilUp = weapon.Template.RecoilForceUp;

                        _originalWSBers.Add(item.TemplateId, origStats);
                    }
                }

                //
                //
                //


                ROPlayer.ActiveHealthController.DoScavRegeneration(10f);


                foreach (var item in Items)
                {
                    if (item is Weapon weapon)
                    {
                        weapon.Template.BaseMalfunctionChance = _originalWSBers[item.TemplateId].malfChance * 0.25f;
                        weapon.Template.DurabilityBurnRatio = _originalWSBers[item.TemplateId].duraBurn * 0.5f;
                        weapon.Template.Ergonomics = _originalWSBers[item.TemplateId].ergo * 2f;
                        weapon.Template.RecoilForceBack = _originalWSBers[item.TemplateId].recoilBack * 0.5f;
                        weapon.Template.RecoilForceUp = _originalWSBers[item.TemplateId].recoilUp * 0.5f;
                    }
                }

                NotificationManagerClass.DisplayMessageNotification("Berserk Event: You're seeing red, I feel bad for any scavs and PMCs in your way!", ENotificationDurationType.Long, ENotificationIconType.Alert);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Berserk Event has started");
            }
#endif  

                await Task.Delay(180000);

                ROPlayer.ActiveHealthController.DoScavRegeneration(0);
                ROPlayer.ActiveHealthController.PauseAllEffects();

                foreach (var item in Items)
                {
                    if (item is Weapon weapon)
                    {
                        weapon.Template.BaseMalfunctionChance = _originalWSBers[item.TemplateId].malfChance;
                        weapon.Template.DurabilityBurnRatio = _originalWSBers[item.TemplateId].duraBurn;
                        weapon.Template.Ergonomics = _originalWSBers[item.TemplateId].ergo;
                        weapon.Template.RecoilForceBack = _originalWSBers[item.TemplateId].recoilBack;
                        weapon.Template.RecoilForceUp = _originalWSBers[item.TemplateId].recoilUp;
                    }
                }

                NotificationManagerClass.DisplayMessageNotification("Berserk Event: Your vision has cleared up, I guess you got all your rage out!", ENotificationDurationType.Long, ENotificationIconType.Alert);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Berserk Event has run");
            }
#endif  
            }

            else
            {
                Weighting.DoRandomEvent(Weighting.weightedEvents);
            }
        }

        public async void DoWeightEvent()
        {
            var Items = Session.Profile.Inventory.GetItemsInSlots(new EquipmentSlot[] { EquipmentSlot.FirstPrimaryWeapon, 
                                                                                        EquipmentSlot.SecondPrimaryWeapon,
                                                                                        EquipmentSlot.Holster,
                                                                                        EquipmentSlot.Scabbard,
                                                                                        EquipmentSlot.ArmorVest, 
                                                                                        EquipmentSlot.TacticalVest, 
                                                                                        EquipmentSlot.Backpack,
                                                                                        EquipmentSlot.Earpiece,
                                                                                        EquipmentSlot.Headwear });

            System.Random random = new System.Random();
            int chance = random.Next(0, 100 + 1);

            if (!_weightEventHasRun)
            {
                _weightEventHasRun = true;

                if (chance is >= 0 && chance is <= 49)
                {
                    foreach (var item in Items)
                    {
                        if (item is Item slottedItem)
                        {
                            slottedItem.Template.Weight = slottedItem.Template.Weight * 2f;
                        }
                    }
                    Session.Profile.Inventory.UpdateTotalWeight();

                    NotificationManagerClass.DisplayMessageNotification("Weight Event: Better hunker down until you get your stamina back!", ENotificationDurationType.Long, ENotificationIconType.Alert);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Weight Event has started");
            }
#endif  

                    await Task.Delay(180000);

                    foreach (var item in Items)
                    {
                        if (item is Item slottedItem)
                        {
                            slottedItem.Template.Weight = slottedItem.Template.Weight * 0.5f;
                        }
                    }
                    Session.Profile.Inventory.UpdateTotalWeight();

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Weight Event has run");
            }
#endif  

                    NotificationManagerClass.DisplayMessageNotification("Weight Event: You're rested and ready to get back out there!", ENotificationDurationType.Long, ENotificationIconType.Alert);
                }

                //
                //
                //

                if (chance is >= 50 && chance is <= 100)
                {
                    foreach (var item in Items)
                    {
                        if (item is Item slottedItem)
                        {
                            slottedItem.Template.Weight = slottedItem.Template.Weight * 0.5f;
                        }
                    }
                    Session.Profile.Inventory.UpdateTotalWeight();

                    NotificationManagerClass.DisplayMessageNotification("Weight Event: You feel light on your feet, stock up on everything you can!", ENotificationDurationType.Long, ENotificationIconType.Alert);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Weight Event has started");
            }
#endif  

                    await Task.Delay(180000);

                    foreach (var item in Items)
                    {
                        if (item is Item slottedItem)
                        {
                            slottedItem.Template.Weight = slottedItem.Template.Weight * 2f;
                        }
                    }
                    Session.Profile.Inventory.UpdateTotalWeight();

                    NotificationManagerClass.DisplayMessageNotification("Weight Event: You've lost your extra energy, hope you didn't fill your backpack too much!", ENotificationDurationType.Long, ENotificationIconType.Alert);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Weight Event has run");
            }
#endif  
                }
            }

            else
            {
                Weighting.DoRandomEvent(Weighting.weightedEvents);
            }
        }

        public async void DoMaxLLEvent()
        {
            if (_maxLLEventCount >= 1) 
            {
                return; 
            }

            var Traders = Utils.Traders;

            _maxLLEventCount++;

            foreach (var Trader in Traders)
            {
                {
                    Session.Profile.TradersInfo[Trader].SetStanding(Session.Profile.TradersInfo[Trader].Standing + 1);
                }
            }

            NotificationManagerClass.DisplayMessageNotification("Shopping Spree Event: All Traders have maxed out standing. Better get to them in the next ten minutes!", ENotificationDurationType.Default, ENotificationIconType.Mail);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Shopping Spree Event has started");
            }
#endif  

            await Task.Delay(600000);

            foreach (var Trader in Traders)
            {
                {
                    Session.Profile.TradersInfo[Trader].SetStanding(Session.Profile.TradersInfo[Trader].Standing - 1);
                }
            }

            NotificationManagerClass.DisplayMessageNotification("Shopping Spree Event: All Traders standing has been set back to normal. This is a fickle business after all.", ENotificationDurationType.Default, ENotificationIconType.Mail);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Shopping Spree Event has run");
            }
#endif 
        }

        public async void DoLockDownEvent()
        {
            var raidTimeLeft = SPT.SinglePlayer.Utils.InRaid.RaidTimeUtil.GetRemainingRaidSeconds();
            var exfils = FindObjectsOfType<ExfiltrationPoint>();

            if (_exfilEventCount >= 1) { return; }

            if (raidTimeLeft < 900)
            {
                Weighting.DoRandomEvent(Weighting.weightedEvents);
            }

            else
            {
                NotificationManagerClass.DisplayMessageNotification("Lockdown Event: All extracts are unavaliable for 15 minutes", ENotificationDurationType.Long, ENotificationIconType.EntryPoint);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Lockdown Event has started");
            }
#endif  

                EventExfilPatch.IsLockdown = true;

                foreach (var exfil in exfils)
                {
                    exfil.Disable(AwaitsManualActivation);
                }
                _exfilEventCount++;

                await Task.Delay(600000);

                foreach (var exfil in exfils)
                {
                    exfil.Enable();
                }

                NotificationManagerClass.DisplayMessageNotification("Lockdown Event: Extracts are available again. Time to get out of there!", ENotificationDurationType.Long, ENotificationIconType.EntryPoint);

#if DEBUG
            if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
            {
                Utils.LogToServerConsole("Lockdown Event has run");
            }
#endif 

                EventExfilPatch.IsLockdown = false;
            }
        }
        #endregion

        public void ExfilAirdropBOOMBOOM()
        {
            if (ROPlayer.Location != "factory4_day" && ROPlayer.Location != "factory4_night" && ROPlayer.Location != "laboratory" && ROPlayer.Location != "sandbox")
            {
                AirdropBoxPatch.isExtractCrate = true;
                ROGameWorld.gameObject.AddComponent<AirdropsManager>().isFlareDrop = true;
            }
        }


        public void FlareLogicEC()
        {
            var whiteFlareInHands = ROPlayer.HandsController.Item.TemplateId == _whiteFlare;

            if (!whiteFlareInHands) { return; }

            if (whiteFlareInHands && Ready())
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && _mouseInputCount < 1)
                {
                    _mouseInputCount++;
                    ExfilAirdropBOOMBOOM();
                    _eventisRunning = true;
                    NotificationManagerClass.DisplayMessageNotification("Exfil Airdrop crate on it's way", ENotificationDurationType.Long, ENotificationIconType.EntryPoint);

#if DEBUG
                    if (ConfigController.ServerConfig.Debug.EnableExtraDebugLogging)
                    {
                        Utils.LogToServerConsole("Exfil Airdrop crate on it's way");
                    }
#endif                      
                }
            }
        }
        //
        //
        //

        public bool Ready()
        {
            return ROGameWorld != null && ROGameWorld.AllAlivePlayersList != null && ROGameWorld.AllAlivePlayersList.Count > 0 && !(ROPlayer is HideoutPlayer);
        }
    }
}
