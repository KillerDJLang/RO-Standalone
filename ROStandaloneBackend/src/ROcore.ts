import type { DependencyContainer } from "tsyringe";

import type { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import type { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";

import { Base } from "./BaseFeatures/baseFeatures";
import { DynamicRouters } from "./Utils/DynamicRouterHooks";
import type { configFile } from "./Utils/Enums";
import { Logger } from "./Utils/Logger";
import { References } from "./Utils/References";
import { StaticRouters } from "./Utils/StaticRouterHooks";

import * as fs from "node:fs";
import * as path from "node:path";
import JSON5 from "json5";

class RaidOverhaulStandalone implements IPreSptLoadMod, IPostDBLoadMod {
    private ref: References = new References();
    private logger: Logger = new Logger(this.ref);

    private static pluginDepCheck(): boolean {
        const pluginRO = "rostandalone.dll";

        try {
            const pluginPath = fs.readdirSync("./BepInEx/plugins").map((plugin) => plugin.toLowerCase());
            return pluginPath.includes(pluginRO);
        } catch {
            return false;
        }
    }

    /**
     * @param container
     */
    public preSptLoad(container: DependencyContainer): void {
        this.ref.preSptLoad(container);

        const staticRouters = new StaticRouters(this.ref, this.logger);
        const dynamicRouters = new DynamicRouters(this.ref, this.logger);
        staticRouters.registerHooks();
        dynamicRouters.registerHooks();
    }

    /**
     * @param container
     */
    public postDBLoad(container: DependencyContainer): void {
        this.ref.postDBLoad(container);

        //Imports
        const modFeatures = new Base(this.ref, this.logger);
        const modConfig = JSON5.parse(
            this.ref.vfs.readFile(path.resolve(__dirname, "../config/config.json5")),
        ) as configFile;

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

        //Dependancy Check
        if (!RaidOverhaulStandalone.pluginDepCheck()) {
            this.logger.logError(
                "Error, client portion of Raid Overhaul Standalone is missing from BepInEx/plugins folder.\nPlease install correctly. ***bonk***",
            );
            return;
        }

        //Push all of the mods base features
        this.pushModFeatures(modFeatures, modConfig);

        this.logger.log(`has finished modifying your raids. ${randomMessage}.`, LogTextColor.CYAN);
    }

    private pushModFeatures(modFeatures: Base, modConfig: configFile) {
        //Push all of the mods base features
        modFeatures.raidChanges(modConfig);
        modFeatures.itemChanges();
        modFeatures.lootChanges(modConfig);
        modFeatures.eventChanges(modConfig);
        if (modConfig.Events.EnableWeatherOptions && modConfig.Events.WinterWonderland) {
            modFeatures.weatherChangesWinterWonderland(modConfig);
        }
    }
}
//      \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/

module.exports = { mod: new RaidOverhaulStandalone() };
