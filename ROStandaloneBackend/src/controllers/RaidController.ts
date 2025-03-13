import { inject, injectable } from "tsyringe";
//Spt Classes
import type { ILocationConfig } from "@spt/models/spt/config/ILocationConfig";
import { DatabaseService } from "@spt/services/DatabaseService";
import { ConfigTypes } from "@spt/models/enums/ConfigTypes";
import { ConfigServer } from "@spt/servers/ConfigServer";
//Custom Classes
import type { ConfigManager } from "../managers/ConfigManager";

@injectable()
export class RaidController {
    constructor(
        @inject("ConfigManager") protected configManager: ConfigManager,
        @inject("ConfigServer") protected configServer: ConfigServer,
        @inject("DatabaseService") protected databaseService: DatabaseService,
    ) {}

    public raidChanges(): void {
        const tables = this.databaseService.getTables();
        const globals = tables.globals.config;

        if (this.configManager.modConfig().Raid.EnableExtendedRaids) {
            for (const location in tables.locations) {
                if (location === "base") continue;

                tables.locations[location].base.EscapeTimeLimit = this.configManager.modConfig().Raid.TimeLimit * 60;
                tables.locations[location].base.EscapeTimeLimitCoop =
                    this.configManager.modConfig().Raid.TimeLimit * 60;
            }
        }

        if (this.configManager.modConfig().Raid.ReduceFoodAndHydroDegrade.Enabled) {
            globals.Health.Effects.Existence.EnergyDamage =
                this.configManager.modConfig().Raid.ReduceFoodAndHydroDegrade.EnergyDecay;
            globals.Health.Effects.Existence.HydrationDamage =
                this.configManager.modConfig().Raid.ReduceFoodAndHydroDegrade.HydroDecay;
        }
    }

    public lootChanges(): void {
        const tables = this.databaseService.getTables();
        const maps = this.configServer.getConfig<ILocationConfig>(ConfigTypes.LOCATION);
        const markedRoomCustoms = tables.locations.bigmap.looseLoot.spawnpoints;
        const markedRoomReserve = tables.locations.rezervbase.looseLoot.spawnpoints;
        const markedRoomStreets = tables.locations.tarkovstreets.looseLoot.spawnpoints;
        const markedRoomLighthouse = tables.locations.lighthouse.looseLoot.spawnpoints;

        if (this.configManager.modConfig().LootChanges.EnableLootOptions) {
            maps.looseLootMultiplier.bigmap = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.factory4_day = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.factory4_night = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.interchange = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.laboratory = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.rezervbase = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.shoreline = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.woods = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.lighthouse = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.tarkovstreets = this.configManager.modConfig().LootChanges.LooseLootMultiplier;
            maps.looseLootMultiplier.sandbox = this.configManager.modConfig().LootChanges.LooseLootMultiplier;

            maps.staticLootMultiplier.bigmap = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.factory4_day = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.factory4_night = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.interchange = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.laboratory = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.rezervbase = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.shoreline = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.woods = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.lighthouse = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.tarkovstreets = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
            maps.staticLootMultiplier.sandbox = this.configManager.modConfig().LootChanges.StaticLootMultiplier;
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
                cSP.probability *= this.configManager.modConfig().LootChanges.MarkedRoomLootMultiplier;
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
                rSP.probability *= this.configManager.modConfig().LootChanges.MarkedRoomLootMultiplier;
            } else if (
                rSP.template.Position.x > -155 &&
                rSP.template.Position.x < -150 &&
                rSP.template.Position.z > 70 &&
                rSP.template.Position.z < 75 &&
                rSP.template.Position.y > -9 &&
                rSP.template.Position.y < -8
            ) {
                rSP.probability *= this.configManager.modConfig().LootChanges.MarkedRoomLootMultiplier;
            } else if (
                rSP.template.Position.x > 190 &&
                rSP.template.Position.x < 195 &&
                rSP.template.Position.z > -230 &&
                rSP.template.Position.z < -225 &&
                rSP.template.Position.y > -6 &&
                rSP.template.Position.y < -5
            ) {
                rSP.probability *= this.configManager.modConfig().LootChanges.MarkedRoomLootMultiplier;
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
                sSP.probability *= this.configManager.modConfig().LootChanges.MarkedRoomLootMultiplier;
            } else if (
                sSP.template.Position.x > 186 &&
                sSP.template.Position.x < 191 &&
                sSP.template.Position.z > 224 &&
                sSP.template.Position.z < 229 &&
                sSP.template.Position.y > -0.5 &&
                sSP.template.Position.y < 1.5
            ) {
                sSP.probability *= this.configManager.modConfig().LootChanges.MarkedRoomLootMultiplier;
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
                lSP.probability *= this.configManager.modConfig().LootChanges.MarkedRoomLootMultiplier;
            }
        }
    }
}
