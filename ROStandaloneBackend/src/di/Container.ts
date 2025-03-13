import { DependencyContainer, Lifecycle } from "tsyringe";
//Custom Classes
import { ROWeatherController } from "../controllers/WeatherController";
import { RaidController } from "../controllers/RaidController";
import { DynamicRouters } from "../routers/DynamicRouterHooks";
import { StaticRouters } from "../routers/StaticRouterHooks";
import { ItemGenerator } from "../generators/ItemGenerator";
import { ConfigManager } from "../managers/ConfigManager";
import { AssortUtils } from "../utils/AssortUtils";
import { ROLogger } from "../utils/Logger";
import { Utils } from "../utils/Utils";
import { RaidOverhaulStandalone } from "../ROcore";

export class DiContainer {
    public static register(container: DependencyContainer): void {
        container.register<RaidOverhaulStandalone>("RaidOverhaulStandalone", RaidOverhaulStandalone, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<ROWeatherController>("ROWeatherController", ROWeatherController, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<RaidController>("RaidController", RaidController, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<DynamicRouters>("DynamicRouters", DynamicRouters, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<StaticRouters>("StaticRouters", StaticRouters, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<ConfigManager>("ConfigManager", ConfigManager, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<ItemGenerator>("ItemGenerator", ItemGenerator, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<AssortUtils>("AssortUtils", AssortUtils, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<ROLogger>("ROLogger", ROLogger, {
            lifecycle: Lifecycle.Singleton,
        });
        container.register<Utils>("Utils", Utils, {
            lifecycle: Lifecycle.Singleton,
        });
    }
}
