import { Base } from "../BaseFeatures/baseFeatures";
import type { configFile, seasonalProgression } from "./Enums";
import type { Logger } from "./Logger";
import type { References } from "./References";

import * as fs from "node:fs";
import * as path from "node:path";
import JSON5 from "json5";

const EventWeightingsConfig = require("../../config/EventWeightings.json");

export class StaticRouters {
    private routerPrefix = "[RO Standalone] ";

    constructor(
        public ref: References,
        public logger: Logger,
    ) {}

    public registerHooks(): void {
        const weatherConfigPath = path.resolve(__dirname, "../../config/SeasonsProgressionFile.json");
        const modConfig = JSON5.parse(
            this.ref.vfs.readFile(path.resolve(__dirname, "../../config/config.json5")),
        ) as configFile;
        const weatherConfig = this.ref.jsonUtil.deserialize(
            fs.readFileSync(weatherConfigPath, "utf-8"),
            "config.json",
        ) as seasonalProgression;
        const modFeatures = new Base(this.ref, this.logger);

        //Get and send configs to the client
        this.ref.staticRouter.registerStaticRouter(
            "GetEventConfig",
            [
                {
                    url: "/ROStandaloneBackend/GetEventConfig",
                    action: async (url, info, sessionId, output) => {
                        const EventWeightings = EventWeightingsConfig;

                        return JSON.stringify(EventWeightings);
                    },
                },
            ],
            "spt",
        );

        this.ref.staticRouter.registerStaticRouter(
            "GetServerConfig",
            [
                {
                    url: "/ROStandaloneBackend/GetServerConfig",
                    action: async (url, info, sessionId, output) => {
                        const ServerConfig = modConfig;

                        return JSON.stringify(ServerConfig);
                    },
                },
            ],
            "spt",
        );

        this.ref.staticRouter.registerStaticRouter(
            "GetWeatherConfig",
            [
                {
                    url: "/ROStandaloneBackend/GetWeatherConfig",
                    action: async (url, info, sessionId, output) => {
                        const WeatherConfig = weatherConfig;

                        return JSON.stringify(WeatherConfig);
                    },
                },
            ],
            "spt",
        );

        // Randomize weather pre-raid
        if (modConfig.Events.EnableWeatherOptions) {
            if (
                modConfig.Events.NoWinter &&
                !modConfig.Events.AllSeasons &&
                !modConfig.Events.SeasonalProgression &&
                !modConfig.Events.WinterWonderland
            ) {
                this.ref.staticRouter.registerStaticRouter(
                    `[${this.routerPrefix}]-/client/items`,
                    [
                        {
                            url: "/client/items",
                            action: async (url, info, sessionId, output) => {
                                modFeatures.weatherChangesNoWinter(modConfig);
                                return Promise.resolve(output);
                            },
                        },
                    ],
                    "spt",
                );
            }

            if (
                modConfig.Events.AllSeasons &&
                !modConfig.Events.NoWinter &&
                !modConfig.Events.SeasonalProgression &&
                !modConfig.Events.WinterWonderland
            ) {
                this.ref.staticRouter.registerStaticRouter(
                    `[${this.routerPrefix}]-/client/items`,
                    [
                        {
                            url: "/client/items",
                            action: async (url, info, sessionId, output) => {
                                modFeatures.weatherChangesAllSeasons(modConfig);
                                return Promise.resolve(output);
                            },
                        },
                    ],
                    "spt",
                );
            }

            if (
                modConfig.Events.SeasonalProgression &&
                !modConfig.Events.AllSeasons &&
                !modConfig.Events.NoWinter &&
                !modConfig.Events.WinterWonderland
            ) {
                this.ref.staticRouter.registerStaticRouter(
                    `[${this.routerPrefix}]-/client/items`,
                    [
                        {
                            url: "/client/items",
                            action: async (url, info, sessionId, output) => {
                                modFeatures.seasonProgression(modConfig);
                                return Promise.resolve(output);
                            },
                        },
                    ],
                    "spt",
                );
            }
        }
    }
}
