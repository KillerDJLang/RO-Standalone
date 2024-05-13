import { DependencyContainer }      from "tsyringe";

import { IPreAkiLoadMod }           from "@spt-aki/models/external/IPreAkiLoadMod";
import { IPostDBLoadMod }           from "@spt-aki/models/external/IPostDBLoadMod";
import { LogTextColor }             from "@spt-aki/models/spt/logging/LogTextColor";
import { StaticRouterModService }   from "@spt-aki/services/mod/staticRouter/StaticRouterModService";

import { ModFeatures }              from "./BaseFeatures/baseFeatures";
import { References }               from "./Utils/References";
import { AssortUtils }              from "./Utils/Methods";

import * as EventWeightingsConfig   from "../config/EventWeightings.json";

const modName = "RO Standalone";
const fs = require('fs');

class RaidOverhaulStandalone implements IPreAkiLoadMod, IPostDBLoadMod
{
    private ref: References = new References();
    private assortUtils: AssortUtils = new AssortUtils(this.ref);

    private static pluginDepCheck(): boolean 
    {
        const pluginRO = "rostandalone.dll";

        try { const pluginPath = fs.readdirSync("./BepInEx/plugins/ROStandalone").map(plugin => plugin.toLowerCase()); return pluginPath.includes(pluginRO); }
        catch { return false; }
    }

    /**
    * @param container
    */
    public preAkiLoad(container: DependencyContainer): void 
    {
        this.ref.preAkiLoad(container);

        const staticRouterModService: StaticRouterModService = container.resolve<StaticRouterModService>("StaticRouterModService");

        staticRouterModService.registerStaticRouter(
            "GetEventWeightings",
            [
                {
                    url: "/ROStandaloneBackend/GetEventWeightings",
                    action: (url: string, 
                        info: string, 
                        sessionId: string, 
                        output: string) => 
                    {                     
                        const EventWeightings = EventWeightingsConfig;

                        return JSON.stringify(EventWeightings);
                    }
                }
            ],
            ""
        );
    }

    /**
    * @param container
    */
    public postDBLoad(container: DependencyContainer): void
    {
        this.ref.postDBLoad(container);

        //Imports
        const modFeatures = new ModFeatures(this.assortUtils, this.ref);

        //Random message on server on startup
        const messageArray =  ["The hamsters can take a break now", "Time to get wrecked by Birdeye LOL", "Back to looking for cat pics", "I made sure to crank up your heart attack event chances", "If there's a bunch of red text it's 100% not my fault", "Good luck out there", "Nerd...."];
        const randomMessage = messageArray[Math.floor(Math.random() * messageArray.length)];

        //Dependancy Check
        if (!RaidOverhaulStandalone.pluginDepCheck())
        {
            return this.ref.logger.error(`[${modName}] Error, client portion of Raid Overhaul Standalone is missing from BepInEx/plugins folder.\nPlease install correctly. ***bonk***`);
        }

        //Push all of the mods base features
        modFeatures.pushModFeatures();

        this.ref.logger.log(`[${modName}] has finished modifying your raids. ${randomMessage}.`, LogTextColor.CYAN);
    }
}
//      \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/     \('_')/

module.exports = { mod: new RaidOverhaulStandalone() };