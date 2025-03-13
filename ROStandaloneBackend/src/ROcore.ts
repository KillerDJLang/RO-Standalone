import { inject, injectable } from "tsyringe";
//Spt Classes
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { Traders } from "@spt/models/enums/Traders";
//Custom Classes
import type { ROWeatherController } from "./controllers/WeatherController";
import type { RaidController } from "./controllers/RaidController";
import type { DynamicRouters } from "./routers/DynamicRouterHooks";
import type { StaticRouters } from "./routers/StaticRouterHooks";
import type { ItemGenerator } from "./generators/ItemGenerator";
import type { ConfigManager } from "./managers/ConfigManager";
import type { AssortUtils } from "./utils/AssortUtils";
import type { ROLogger } from "./utils/Logger";
import type { Utils } from "./utils/Utils";
import { Currency } from "./models/Enums";
//Modules
import fs from "node:fs";

@injectable()
export class RaidOverhaulStandalone {
    constructor(
        @inject("Utils") protected utils: Utils,
        @inject("ROLogger") protected logger: ROLogger,
        @inject("AssortUtils") protected assortUtils: AssortUtils,
        @inject("ItemGenerator") protected itemGenerator: ItemGenerator,
        @inject("ConfigManager") protected configManager: ConfigManager,
        @inject("StaticRouters") protected staticRouters: StaticRouters,
        @inject("DynamicRouters") protected dynamicRouters: DynamicRouters,
        @inject("RaidController") protected raidController: RaidController,
        @inject("ROWeatherController") protected weatherController: ROWeatherController,
    ) {}

    public async preSptLoadAsync(): Promise<void> {
        //Register router hooks
        this.staticRouters.registerHooks();
        this.dynamicRouters.registerHooks();
    }

    public async postDBLoadAsync(): Promise<void> {
        //Random message on server on startup
        const messageArray = [
            "The hamsters can take a break now",
            "Time to get wrecked by Birdeye LOL",
            "Back to looking for cat pics",
            "I made sure to crank up your heart attack event chances",
            "If there's a bunch of red text it's 100% not my fault",
            "We are legion, for we are many",
            "All Hail the Cult of Cj",
            "Good luck out there",
        ];
        const randomMessage = messageArray[Math.floor(Math.random() * messageArray.length)];

        //Check for proper install
        if (!this.fikaInstalled()) {
            const pluginRO = "rostandalone.dll";
            const pluginPath = fs.readdirSync("./BepInEx/plugins/RaidOverhaulStandalone").map((plugin) => plugin.toLowerCase());

            if (!this.utils.checkDependancies(pluginPath, pluginRO)) {
                this.logger.logError(
                    "Error, client portion of Raid Overhaul Standalone is missing from BepInEx/plugins folder.\nPlease install correctly.",
                );
                return;
            }
        }
        this.itemGenerator.createCustomItems("../../db/ItemGen");
        this.pushModFeatures();
        this.addFlaresToPk();

        this.logger.log(`has finished modifying your raids. ${randomMessage}.`, LogTextColor.CYAN);
    }

    private pushModFeatures(): void {
        //Push all the mods base features
        this.raidController.raidChanges();
        this.raidController.lootChanges();
        if (
            this.configManager.modConfig().Seasons.EnableWeatherOptions &&
            this.configManager.modConfig().Seasons.WinterWonderland
        ) {
            this.weatherController.weatherChangesWinterWonderland();
        }
    }

    private addFlaresToPk(): void {
        this.assortUtils.createSingleItemOffer(
            "67cde31eea2d15e888fa7dee",
            this.utils.genRandomCount(1, 3),
            1,
            this.utils.genRandomCount(460, 1380),
            Currency.Dollars,
            Traders.PEACEKEEPER,
        );
        this.assortUtils.createSingleItemOffer(
            "67cda57f8f59300db5c0ec5b",
            this.utils.genRandomCount(1, 3),
            1,
            this.utils.genRandomCount(2250, 4000),
            Currency.Dollars,
            Traders.PEACEKEEPER,
        );
    }

    private fikaInstalled(): boolean {
        const pluginPath = fs.readdirSync("./BepInEx/plugins").map((plugin) => plugin.toLowerCase());
        const fika = "fika.core.dll";
        const dediClient = "fika.dedicated.dll";

        if (!this.utils.checkDependancies(pluginPath, fika) && !this.utils.checkDependancies(pluginPath, dediClient)) {
            return false;
        } else {
            return true;
        }
    }
}
