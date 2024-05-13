import { Item }                   from "@spt-aki/models/eft/common/tables/IItem";
import { IBarterScheme, ITrader } from "@spt-aki/models/eft/common/tables/ITrader";
import { Money }                  from "@spt-aki/models/enums/Money";
import { References }             from "./References";

export class AssortUtils
{
    protected itemsToSell: Item[] = [];
    protected barterScheme: Record<string, IBarterScheme[][]> = {};
    protected loyaltyLevel: Record<string, number> = {};
    
    constructor(private ref: References) 
    {}

    public createSingleAssortItem(itemTpl: string, itemId = undefined): AssortUtils
    {
        const newItemToAdd: Item = {
            _id: !itemId ? this.ref.hashUtil.generate(): itemId,
            _tpl: itemTpl,
            parentId: "hideout",
            slotId: "hideout",
            upd: {
                UnlimitedCount: false,
                StackObjectsCount: 100
            }
        };

        this.itemsToSell.push(newItemToAdd);

        return this;
    }

    public createComplexAssortItem(items: Item[]): AssortUtils
    {
        items[0].parentId = "hideout";
        items[0].slotId = "hideout";

        if (!items[0].upd)
        {
            items[0].upd = {}
        }

        items[0].upd.UnlimitedCount = false;
        items[0].upd.StackObjectsCount = 100;

        this.itemsToSell.push(...items);

        return this;
    }

    public addStackCount(stackCount: number): AssortUtils
    {
        this.itemsToSell[0].upd.StackObjectsCount = stackCount;

        return this;
    }

    public addUnlimitedStackCount(): AssortUtils
    {
        this.itemsToSell[0].upd.StackObjectsCount = 999999;
        this.itemsToSell[0].upd.UnlimitedCount = true;

        return this;
    }

    public makeStackCountUnlimited(): AssortUtils
    {
        this.itemsToSell[0].upd.StackObjectsCount = 999999;

        return this;
    }

    public addBuyRestriction(maxBuyLimit: number): AssortUtils
    {
        this.itemsToSell[0].upd.BuyRestrictionMax = maxBuyLimit;
        this.itemsToSell[0].upd.BuyRestrictionCurrent = 0;

        return this;
    }

    public addLoyaltyLevel(level: number)
    {
        this.loyaltyLevel[this.itemsToSell[0]._id] = level;

        return this;
    }

    public addMoneyCost(currencyType: Money, amount: number): AssortUtils
    {
        this.barterScheme[this.itemsToSell[0]._id] = [
            [
                {
                    count: amount,
                    _tpl: currencyType
                }
            ]
        ];

        return this;
    }

    public addBarterCost(itemTpl: string, count: number): AssortUtils
    {
        const sellableItemId = this.itemsToSell[0]._id;

        if (Object.keys(this.barterScheme).length === 0)
        {
            this.barterScheme[sellableItemId] = [[
                {
                    count: count,
                    _tpl: itemTpl
                }
            ]];
        }
        else
        {
            const existingData = this.barterScheme[sellableItemId][0].find(x => x._tpl === itemTpl);
            if (existingData)
            {
                existingData.count+= count;
            }
            else
            {
                this.barterScheme[sellableItemId][0].push({
                    count: count,
                    _tpl: itemTpl
                })
            }
            
        }

        return this;
    }

    public export(data: ITrader, blockDupes: boolean): AssortUtils
    {
        const itemBeingSoldId  = this.itemsToSell[0]._id;
        const itemBeingSoldTpl = this.itemsToSell[0]._tpl;
        if (blockDupes)
        {
            if (data.assort.items.find(x => x._id === itemBeingSoldId))
            {
                return;
            }

            if (data.assort.items.find(x => x._tpl === itemBeingSoldTpl))
            {
                return;
            }
        }

        data.assort.items.push(...this.itemsToSell);
        data.assort.barter_scheme[itemBeingSoldId] = this.barterScheme[itemBeingSoldId];
        data.assort.loyal_level_items[itemBeingSoldId] = this.loyaltyLevel[itemBeingSoldId];

        this.itemsToSell = [];
        this.barterScheme = {};
        this.loyaltyLevel = {};

        return this;
    }

    public pushFromTraderAssort(items: Item[], itemTpl: string, count: number, stackCount: number, level: number, data: ITrader, blockDupes: boolean, )
    {
        items[0].parentId = "hideout";
        items[0].slotId = "hideout";

        if (!items[0].upd)
        {
            items[0].upd = {}
        }

        items[0].upd.UnlimitedCount = false;
        items[0].upd.StackObjectsCount = 100;

        this.itemsToSell.push(...items);

        const sellableItemId = this.itemsToSell[0]._id;

        if (Object.keys(this.barterScheme).length === 0)
        {
            this.barterScheme[sellableItemId] = [[
                {
                    count: count,
                    _tpl: itemTpl
                }
            ]];
        }

        else
        {
            const existingData = this.barterScheme[sellableItemId][0].find(x => x._tpl === itemTpl);
            if (existingData)
            {
                existingData.count+= count;
            }
            else
            {
                this.barterScheme[sellableItemId][0].push({
                    count: count,
                    _tpl: itemTpl
                })
            }          
        }
       
        this.itemsToSell[0].upd.StackObjectsCount = stackCount;
        
        this.loyaltyLevel[this.itemsToSell[0]._id] = level;

        const itemBeingSoldId  = this.itemsToSell[0]._id;
        const itemBeingSoldTpl = this.itemsToSell[0]._tpl;
        if (blockDupes)
        {
            if (data.assort.items.find(x => x._id === itemBeingSoldId))
            {
                return;
            }

            if (data.assort.items.find(x => x._tpl === itemBeingSoldTpl))
            {
                return;
            }
        }

        data.assort.items.push(...this.itemsToSell);
        data.assort.barter_scheme[itemBeingSoldId] = this.barterScheme[itemBeingSoldId];
        data.assort.loyal_level_items[itemBeingSoldId] = this.loyaltyLevel[itemBeingSoldId];

        this.itemsToSell = [];
        this.barterScheme = {};
        this.loyaltyLevel = {};
    }
}