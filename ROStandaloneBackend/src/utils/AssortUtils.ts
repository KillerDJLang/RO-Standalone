import { inject, injectable } from "tsyringe";
//Spt Classes
import type { DatabaseService } from "@spt/services/DatabaseService";
import type { IItem } from "@spt/models/eft/common/tables/IItem";
//Custom Classes
import type { Utils } from "./Utils";

@injectable()
export class AssortUtils {
    constructor(
        @inject("Utils") protected utils: Utils,
        @inject("DatabaseService") protected databaseService: DatabaseService,
    ) {}

    public createSingleItemOffer(
        ItemToAdd: string,
        StockCount: number,
        LoyaltyLevelToPush: number,
        ReqCost: number,
        CurrencyToUse: any,
        TraderToUse: string,
    ): void {
        const tables = this.databaseService.getTables();
        const singleOffer: IItem = {
            _id: this.utils.genId(),
            _tpl: ItemToAdd,
            parentId: "hideout",
            slotId: "hideout",
            upd: {
                UnlimitedCount: false,
                StackObjectsCount: StockCount,
            },
        };
        const barterScheme = [
            [
                {
                    count: ReqCost,
                    _tpl: CurrencyToUse,
                },
            ],
        ];
        const loyaltyLevel = LoyaltyLevelToPush;

        tables.traders[TraderToUse].assort.items.push(singleOffer);
        tables.traders[TraderToUse].assort.barter_scheme[singleOffer._id] = barterScheme;
        tables.traders[TraderToUse].assort.loyal_level_items[singleOffer._id] = loyaltyLevel;
    }
}
