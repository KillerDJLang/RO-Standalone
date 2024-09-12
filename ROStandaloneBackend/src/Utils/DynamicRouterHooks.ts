import type { configFile } from "./Enums";
import type { Logger } from "./Logger";
import type { References } from "./References";

import * as path from "node:path";
import JSON5 from "json5";

export class DynamicRouters {
    private routerPrefix = "[Raid Overhaul] ";

    constructor(
        public ref: References,
        public logger: Logger,
    ) {}

    public registerHooks(): void {
        const modConfig = JSON5.parse(
            this.ref.vfs.readFile(path.resolve(__dirname, "../../config/config.json5")),
        ) as configFile;

        //Log from the client to the server if in debug build in the client and extra debug logging is enabled in the server
        if (modConfig.Debug.ExtraLogging) {
            this.ref.dynamicRouter.registerDynamicRouter(
                `DynamicReportError-${this.routerPrefix}`,
                [
                    {
                        url: "/ROStandaloneBackend/LogToServer/",
                        action: async (url) => {
                            const urlParts = url.split("/");
                            const clientMessage = urlParts[urlParts.length - 1];

                            const regex = /%20/g;
                            this.logger.logToServer(clientMessage.replace(regex, " "));

                            return JSON.stringify({ resp: "OK" });
                        },
                    },
                ],
                "LogToServer",
            );
        }
    }
}
