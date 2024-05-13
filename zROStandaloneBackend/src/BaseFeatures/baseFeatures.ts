import { IWeatherConfig }   from "@spt-aki/models/spt/config/IWeatherConfig";
import { ILocationConfig }  from "@spt-aki/models/spt/config/ILocationConfig";
import { LogTextColor }     from "@spt-aki/models/spt/logging/LogTextColor";
import { ConfigTypes }      from "@spt-aki/models/enums/ConfigTypes";
import { Traders }          from "@spt-aki/models/enums/Traders";
import { Money }            from "@spt-aki/models/enums/Money";
import { References }       from "../Utils/References";
import { AssortUtils }      from "../Utils/Methods";

import * as modConfig       from "../../config/config.json";

const modName = "RO Standalone";

export class ModFeatures
{
    constructor(private assortUtils: AssortUtils, private ref: References) 
    {}

    public pushModFeatures(): void
    {
        const randomEventList = [
            "Halloween",
            "Christmas",
            "HalloweenIllumination"
        ]

        const randomEvent =     this.ref.randomUtil.drawRandomFromList(randomEventList, 1).toString();
        const maps =            this.ref.configServer.getConfig<ILocationConfig>(ConfigTypes.LOCATION);
        const weatherConfig =   this.ref.configServer.getConfig<IWeatherConfig>(ConfigTypes.WEATHER);
        const handbookBase =    this.ref.tables.templates.handbook;
        const fleaPrices =      this.ref.tables.templates.prices;
        const globals =         this.ref.tables.globals.config;
        const whiteFlare =      "62178be9d0050232da3485d9"

        if (modConfig.RaidTime.EnableCustomRaidTimes)
        {
            for (const location in this.ref.tables.locations)
            {
                if (location == "base") continue;
                this.ref.tables.locations[location].base.EscapeTimeLimit = modConfig.RaidTime.TotalTimeInMinutes * 60;
                this.ref.tables.locations[location].base.EscapeTimeLimitCoop = modConfig.RaidTime.TotalTimeInMinutes * 60;
            }
        }
   
        for (const flare in handbookBase.Items)
        {
            if (handbookBase.Items[flare].Id == whiteFlare)
            {
                handbookBase.Items[flare].Price = 89999 * modConfig.WhiteFlareCostMultiplier
            }
        }
        fleaPrices[whiteFlare] = 97388 * modConfig.WhiteFlareCostMultiplier

        this.assortUtils.createSingleAssortItem(whiteFlare)
                        .addStackCount(999999)
                        .addBuyRestriction(3)
                        .addMoneyCost(Money.ROUBLES, 99999 * modConfig.WhiteFlareCostMultiplier)
                        .addLoyaltyLevel(1)
                        .export(this.ref.tables.traders[Traders.MECHANIC], false);

        if (modConfig.Events.EnableWeatherOptions)
        {
            globals.EventType = [];
            globals.EventType = ["None"];

            if (modConfig.Events.RandomizedWeather && !modConfig.Events.WinterWonderland)
            {
                if (this.ref.probHelper.rollChance(30, 100))
                {
                    weatherConfig.forceWinterEvent = true;
                    this.ref.logger.log(`[${modName}] Snow is active. It's a whole fuckin' winter wonderland out there.`, LogTextColor.MAGENTA)
                }

                else
                {
                    return;
                }
            }

            if (modConfig.Events.WinterWonderland && !modConfig.Events.RandomizedWeather)
            {
                weatherConfig.forceWinterEvent = true;
                this.ref.logger.log(`[${modName}] Snow is active. It's a whole fuckin' winter wonderland out there.`, LogTextColor.MAGENTA)
            }

            if (modConfig.Events.RandomizedWeather && modConfig.Events.WinterWonderland)
            {
                this.ref.logger.log(`[${modName}] Error modifying your weather. Make sure you only have ONE of the weather options enabled`, LogTextColor.RED)
            }

            if (modConfig.Events.RandomizedSeasonalEvents)
            {
                if (this.ref.probHelper.rollChance(15, 100))
                {
                    globals.EventType.push(randomEvent);
                    this.ref.logger.log(`[${modName}] ${randomEvent} event has been loaded`, LogTextColor.MAGENTA)
                }
            }
        }

        if (modConfig.LootChanges.EnableLootOptions)
        {
            maps.looseLootMultiplier.bigmap = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.factory4_day = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.factory4_night = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.interchange = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.laboratory = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.rezervbase = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.shoreline = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.woods = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.lighthouse = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.tarkovstreets = modConfig.LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.sandbox = modConfig.LootChanges.LooseLootMultiplier;

            maps.staticLootMultiplier.bigmap = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.factory4_day = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.factory4_night = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.interchange = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.laboratory = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.rezervbase = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.shoreline = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.woods = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.lighthouse = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.tarkovstreets = modConfig.LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.sandbox = modConfig.LootChanges.StaticLootMultiplier;
        }
    }
}