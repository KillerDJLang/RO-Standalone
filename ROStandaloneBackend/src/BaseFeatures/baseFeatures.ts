import type { IConfig } from "@spt/models/eft/common/IGlobals";
import type { IQuest } from "@spt/models/eft/common/tables/IQuest";
import { ConfigTypes } from "@spt/models/enums/ConfigTypes";
import { Season } from "@spt/models/enums/Season";
import type { ILocationConfig } from "@spt/models/spt/config/ILocationConfig";
import type { IWeatherConfig } from "@spt/models/spt/config/IWeatherConfig";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";

import type { configFile } from "../Utils/Enums";
import type { Logger } from "../Utils/Logger";
import type { References } from "../Utils/References";

import * as fs from "node:fs";
import * as path from "node:path";

export class Base {
    constructor(
        private ref: References,
        private logger: Logger,
    ) {}

    globals(): IConfig {
        return this.ref.tables.globals.config;
    }
    quests(): Record<string, IQuest> {
        return this.ref.tables.templates.quests;
    }

    //#region Raid Changes
    public raidChanges(modConfig: configFile): void {
        if (modConfig.Raid.EnableExtendedRaids) {
            for (const location in this.ref.tables.locations) {
                if (location === "base") continue;

                this.ref.tables.locations[location].base.EscapeTimeLimit = modConfig.Raid.TimeLimit * 60;
                this.ref.tables.locations[location].base.EscapeTimeLimitCoop = modConfig.Raid.TimeLimit * 60;
            }
        }

        if (modConfig.Raid.ReduceFoodAndHydroDegrade.Enabled) {
            this.globals().Health.Effects.Existence.EnergyDamage = modConfig.Raid.ReduceFoodAndHydroDegrade.EnergyDecay;
            this.globals().Health.Effects.Existence.HydrationDamage =
                modConfig.Raid.ReduceFoodAndHydroDegrade.HydroDecay;
        }
    }
    //#endregion
    //
    //
    //
    //#region Item Changes
    public itemChanges(): void {
        const handbookBase = this.ref.tables.templates.handbook;
        const fleaPrices = this.ref.tables.templates.prices;
        const whiteFlare = "62178be9d0050232da3485d9";

        for (const flare in handbookBase.Items) {
            if (handbookBase.Items[flare].Id === whiteFlare) {
                handbookBase.Items[flare].Price = 89999;
            }
        }
        fleaPrices[whiteFlare] = 97388 + this.ref.randomUtil.getInt(500, 53000);
    }
    //#endregion
    //
    //
    //
    //#region Loot
    public lootChanges(modConfig: configFile): void {
        const maps = this.ref.configServer.getConfig<ILocationConfig>(ConfigTypes.LOCATION);
        const markedRoomCustoms = this.ref.tables.locations.bigmap.looseLoot.spawnpoints;
        const markedRoomReserve = this.ref.tables.locations.rezervbase.looseLoot.spawnpoints;
        const markedRoomStreets = this.ref.tables.locations.tarkovstreets.looseLoot.spawnpoints;
        const markedRoomLighthouse = this.ref.tables.locations.lighthouse.looseLoot.spawnpoints;

        if (modConfig.LootChanges.EnableLootOptions) {
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

        for (const cSP of markedRoomCustoms) {
            if (
                cSP.template.Position.x > 180 &&
                cSP.template.Position.x < 185 &&
                cSP.template.Position.z > 180 &&
                cSP.template.Position.z < 185 &&
                cSP.template.Position.y > 6 &&
                cSP.template.Position.y < 7
            ) {
                cSP.probability *= modConfig.LootChanges.MarkedRoomLootMultiplier;
            }
        }

        for (const rSP of markedRoomReserve) {
            if (
                rSP.template.Position.x > -125 &&
                rSP.template.Position.x < -120 &&
                rSP.template.Position.z > 25 &&
                rSP.template.Position.z < 30 &&
                rSP.template.Position.y > -15 &&
                rSP.template.Position.y < -14
            ) {
                rSP.probability *= modConfig.LootChanges.MarkedRoomLootMultiplier;
            } else if (
                rSP.template.Position.x > -155 &&
                rSP.template.Position.x < -150 &&
                rSP.template.Position.z > 70 &&
                rSP.template.Position.z < 75 &&
                rSP.template.Position.y > -9 &&
                rSP.template.Position.y < -8
            ) {
                rSP.probability *= modConfig.LootChanges.MarkedRoomLootMultiplier;
            } else if (
                rSP.template.Position.x > 190 &&
                rSP.template.Position.x < 195 &&
                rSP.template.Position.z > -230 &&
                rSP.template.Position.z < -225 &&
                rSP.template.Position.y > -6 &&
                rSP.template.Position.y < -5
            ) {
                rSP.probability *= modConfig.LootChanges.MarkedRoomLootMultiplier;
            }
        }

        for (const sSP of markedRoomStreets) {
            if (
                sSP.template.Position.x > -133 &&
                sSP.template.Position.x < -129 &&
                sSP.template.Position.z > 265 &&
                sSP.template.Position.z < 275 &&
                sSP.template.Position.y > 8.5 &&
                sSP.template.Position.y < 11
            ) {
                sSP.probability *= modConfig.LootChanges.MarkedRoomLootMultiplier;
            } else if (
                sSP.template.Position.x > 186 &&
                sSP.template.Position.x < 191 &&
                sSP.template.Position.z > 224 &&
                sSP.template.Position.z < 229 &&
                sSP.template.Position.y > -0.5 &&
                sSP.template.Position.y < 1.5
            ) {
                sSP.probability *= modConfig.LootChanges.MarkedRoomLootMultiplier;
            }
        }

        for (const lSP of markedRoomLighthouse) {
            if (
                lSP.template.Position.x > 319 &&
                lSP.template.Position.x < 330 &&
                lSP.template.Position.z > 482 &&
                lSP.template.Position.z < 489 &&
                lSP.template.Position.y > 5 &&
                lSP.template.Position.y < 6.5
            ) {
                lSP.probability *= modConfig.LootChanges.MarkedRoomLootMultiplier;
            }
        }
    }
    //#endregion
    //
    //
    //
    //#region Events
    public eventChanges(modConfig: configFile): void {
        const randomEventList = ["Halloween", "Christmas", "HalloweenIllumination"];

        const randomEvent = this.ref.randomUtil.drawRandomFromList(randomEventList, 1).toString();

        if (modConfig.Events.EnableWeatherOptions) {
            this.globals().EventType = [];
            this.globals().EventType = ["None"];

            if (modConfig.Events.RandomizedSeasonalEvents) {
                if (this.ref.probHelper.rollChance(15, 100)) {
                    this.globals().EventType.push(randomEvent);
                    this.logger.log(`${randomEvent} event has been loaded`, LogTextColor.MAGENTA);
                }
            }
        }
    }

    public weatherChangesAllSeasons(modConfig: configFile) {
        const weatherConfig: IWeatherConfig = this.ref.configServer.getConfig<IWeatherConfig>(ConfigTypes.WEATHER);

        if (
            modConfig.Events.AllSeasons &&
            !modConfig.Events.WinterWonderland &&
            !modConfig.Events.NoWinter &&
            !modConfig.Events.SeasonalProgression
        ) {
            const weatherChance = this.ref.randomUtil.getInt(1, 100);

            if (weatherChance >= 1 && weatherChance <= 20) {
                weatherConfig.overrideSeason = Season.SUMMER;
                this.logger.log("Summer is active.", LogTextColor.MAGENTA);

                return;
            }
            if (weatherChance >= 21 && weatherChance <= 40) {
                weatherConfig.overrideSeason = Season.AUTUMN;
                this.logger.log("Autumn is active.", LogTextColor.MAGENTA);

                return;
            }
            if (weatherChance >= 41 && weatherChance <= 60) {
                weatherConfig.overrideSeason = Season.WINTER;
                this.logger.log("Winter is coming.", LogTextColor.MAGENTA);

                return;
            }
            if (weatherChance >= 61 && weatherChance <= 80) {
                weatherConfig.overrideSeason = Season.SPRING;
                this.logger.log("Spring is active.", LogTextColor.MAGENTA);

                return;
            }
            if (weatherChance >= 81 && weatherChance <= 100) {
                weatherConfig.overrideSeason = Season.STORM;
                this.logger.log("Storm is active.", LogTextColor.MAGENTA);

                return;
            }
        }

        if (
            (modConfig.Events.AllSeasons && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.NoWinter && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.SeasonalProgression && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.NoWinter && modConfig.Events.SeasonalProgression) ||
            (modConfig.Events.NoWinter && modConfig.Events.AllSeasons) ||
            (modConfig.Events.SeasonalProgression && modConfig.Events.AllSeasons)
        ) {
            this.logger.log(
                "Error modifying your weather. Make sure you only have ONE of the weather options enabled",
                LogTextColor.RED,
            );

            return;
        }
    }

    public weatherChangesNoWinter(modConfig: configFile) {
        const weatherConfig: IWeatherConfig = this.ref.configServer.getConfig<IWeatherConfig>(ConfigTypes.WEATHER);

        if (
            modConfig.Events.NoWinter &&
            !modConfig.Events.WinterWonderland &&
            !modConfig.Events.AllSeasons &&
            !modConfig.Events.SeasonalProgression
        ) {
            const weatherChance = this.ref.randomUtil.getInt(1, 100);

            if (weatherChance >= 1 && weatherChance <= 25) {
                weatherConfig.overrideSeason = Season.SUMMER;
                this.logger.log("Summer is active.", LogTextColor.MAGENTA);

                return;
            }
            if (weatherChance >= 26 && weatherChance <= 50) {
                weatherConfig.overrideSeason = Season.AUTUMN;
                this.logger.log("Autumn is active.", LogTextColor.MAGENTA);

                return;
            }
            if (weatherChance >= 51 && weatherChance <= 75) {
                weatherConfig.overrideSeason = Season.SPRING;
                this.logger.log("Spring is active.", LogTextColor.MAGENTA);

                return;
            }
            if (weatherChance >= 76 && weatherChance <= 100) {
                weatherConfig.overrideSeason = Season.STORM;
                this.logger.log("Storm is active.", LogTextColor.MAGENTA);

                return;
            }
        }

        if (
            (modConfig.Events.AllSeasons && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.NoWinter && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.SeasonalProgression && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.NoWinter && modConfig.Events.SeasonalProgression) ||
            (modConfig.Events.NoWinter && modConfig.Events.AllSeasons) ||
            (modConfig.Events.SeasonalProgression && modConfig.Events.AllSeasons)
        ) {
            this.logger.log(
                "Error modifying your weather. Make sure you only have ONE of the weather options enabled",
                LogTextColor.RED,
            );

            return;
        }
    }

    public weatherChangesWinterWonderland(modConfig: configFile) {
        const weatherConfig: IWeatherConfig = this.ref.configServer.getConfig<IWeatherConfig>(ConfigTypes.WEATHER);

        if (
            modConfig.Events.WinterWonderland &&
            !modConfig.Events.AllSeasons &&
            !modConfig.Events.NoWinter &&
            !modConfig.Events.SeasonalProgression
        ) {
            weatherConfig.overrideSeason = Season.WINTER;
            this.logger.log(`Snow is active. It's a whole fuckin' winter wonderland out there.`, LogTextColor.MAGENTA);

            return;
        }

        if (
            (modConfig.Events.AllSeasons && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.NoWinter && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.SeasonalProgression && modConfig.Events.WinterWonderland) ||
            (modConfig.Events.NoWinter && modConfig.Events.SeasonalProgression) ||
            (modConfig.Events.NoWinter && modConfig.Events.AllSeasons) ||
            (modConfig.Events.SeasonalProgression && modConfig.Events.AllSeasons)
        ) {
            this.logger.log(
                "Error modifying your weather. Make sure you only have ONE of the weather options enabled",
                LogTextColor.RED,
            );

            return;
        }
    }

    public seasonProgression(modConfig: configFile) {
        const weatherConfig: IWeatherConfig = this.ref.configServer.getConfig<IWeatherConfig>(ConfigTypes.WEATHER);
        const seasonsProgression = require("../../config/SeasonsProgressionFile.json");
        const seasonsProgressionFile = path.join(__dirname, "../../config/SeasonsProgressionFile.json");
        let RaidsRun = seasonsProgression.seasonsProgression;

        if (RaidsRun > 0 && RaidsRun < 4) {
            RaidsRun++;

            weatherConfig.overrideSeason = Season.SPRING;
            seasonsProgression.seasonsProgression = RaidsRun;
            fs.writeFileSync(seasonsProgressionFile, JSON.stringify(seasonsProgression, null, 4));
            if (modConfig.Debug.ExtraLogging) {
                this.logger.log("Spring is active.", LogTextColor.MAGENTA);
            }

            return;
        }
        if (RaidsRun > 3 && RaidsRun < 7) {
            RaidsRun++;

            seasonsProgression.seasonsProgression = RaidsRun;
            fs.writeFileSync(seasonsProgressionFile, JSON.stringify(seasonsProgression, null, 4));
            if (modConfig.Debug.ExtraLogging) {
                this.logger.log("Storm is active.", LogTextColor.MAGENTA);
            }

            return;
        }
        if (RaidsRun > 6 && RaidsRun < 10) {
            RaidsRun++;

            weatherConfig.overrideSeason = Season.SUMMER;
            seasonsProgression.seasonsProgression = RaidsRun;
            fs.writeFileSync(seasonsProgressionFile, JSON.stringify(seasonsProgression, null, 4));
            if (modConfig.Debug.ExtraLogging) {
                this.logger.log("Summer is active.", LogTextColor.MAGENTA);
            }

            return;
        }
        if (RaidsRun > 9 && RaidsRun < 13) {
            RaidsRun++;

            weatherConfig.overrideSeason = Season.AUTUMN;
            seasonsProgression.seasonsProgression = RaidsRun;
            fs.writeFileSync(seasonsProgressionFile, JSON.stringify(seasonsProgression, null, 4));
            if (modConfig.Debug.ExtraLogging) {
                this.logger.log("Autumn is active.", LogTextColor.MAGENTA);
            }

            return;
        }
        if (RaidsRun > 12 && RaidsRun < 16) {
            RaidsRun++;

            weatherConfig.overrideSeason = Season.WINTER;
            seasonsProgression.seasonsProgression = RaidsRun;
            fs.writeFileSync(seasonsProgressionFile, JSON.stringify(seasonsProgression, null, 4));
            if (modConfig.Debug.ExtraLogging) {
                this.logger.log("Winter is coming.", LogTextColor.MAGENTA);
            }

            return;
        }
        if (RaidsRun > 15 || RaidsRun < 1) {
            RaidsRun = 1;

            seasonsProgression.seasonsProgression = RaidsRun;
            fs.writeFileSync(seasonsProgressionFile, JSON.stringify(seasonsProgression, null, 4));
            if (modConfig.Debug.ExtraLogging) {
                this.logger.log("Winter has passed.", LogTextColor.MAGENTA);
            }

            return;
        }
    }
    //#endregion
}
