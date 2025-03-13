import { inject, injectable } from "tsyringe";
//Spt Classes
import type { DatabaseService } from "@spt/services/DatabaseService";
import type { PreSptModLoader } from "@spt/loaders/PreSptModLoader";
import type { RandomUtil } from "@spt/utils/RandomUtil";
import type { HashUtil } from "@spt/utils/HashUtil";
//Custom Classes
import type { ROLogger } from "./Logger";
//Modules
import path from "node:path";

@injectable()
export class Utils {
    constructor(
        @inject("ROLogger") protected logger: ROLogger,
        @inject("HashUtil") protected hashUtil: HashUtil,
        @inject("RandomUtil") protected randomUtil: RandomUtil,
        @inject("PreSptModLoader") protected preSptModLoader: PreSptModLoader,
        @inject("DatabaseService") protected databaseService: DatabaseService,
    ) {}
    public static modLoc = path.join(__dirname, "..", "..");

    //#region Base Utils
    /**
     * Checks the mods directory to see if another mod is installed.
     *
     * @param modName - Folder name of the mod to check for.
     * @returns True if the mod is installed, else return false.
     */
    public checkForMod(modName: string): boolean {
        return this.preSptModLoader.getImportedModsNames().includes(modName);
    }

    /**
     * Checks for dependancies in the specified path.
     *
     * @param path - The path to your dependancy. This is the containing folder. Ie BepInEx/plugins
     * @param dependancy - The dependancy you are checking for. Ie raidoverhaul.dll
     * @returns True if the dependancy exists and false if it doesn't.
     */
    public checkDependancies(path: string[], dependancy: string): boolean {
        try {
            return path.includes(dependancy);
        } catch {
            return false;
        }
    }

    /**
     * Generates a random string to be used as an instance Id.
     * Gens a new ID each time it runs so not suitable for tpls and such unless you cache the Id.
     *
     * @returns Valid instance Id.
     */
    public genId(): string {
        return this.hashUtil.generate();
    }

    /**
     * Generates a random number in the supplied range.
     *
     * @returns Random integer in the given range.
     */
    public genRandomCount(min: number, max: number): number {
        return this.randomUtil.randInt(min, max);
    }

    /**
     * Pulls a random item from the specified array.
     *
     * @param list - The array to pull from.
     * @param count - Optional param. The number of items to return from the array. Returns 1 if left unused
     * @returns The pulled item as a string.
     */
    public drawRandom(list: string[], count?: number): string {
        return this.randomUtil.drawRandomFromList(list, count ?? 1, false).toString();
    }

    /**
     * Fetches the handbook info for an item.
     *
     * @param itemID - Id of the item you want to get handbook info for.
     * @returns The handbook price of the item.
     */
    public getItemInHandbook(itemID: string): number {
        const tables = this.databaseService.getTables();
        try {
            const hbItem = tables.templates.handbook.Items.find((i) => i.Id === itemID);
            return Math.round(hbItem.Price);
        } catch (error) {
            this.logger.logWarning(`\nError getting Handbook ID for ${itemID}`);
        }
    }

    /**
     * Fetches the flea market info for an item.
     *
     * @param itemID - Id of the item you want to get flea market info for.
     * @returns The flea market price of the item.
     */
    public getFleaPrice(itemID: string): number {
        const tables = this.databaseService.getTables();
        if (typeof tables.templates.prices[itemID] !== "undefined") {
            return Math.round(tables.templates.prices[itemID]);
        } else {
            return this.getItemInHandbook(itemID);
        }
    }
}
