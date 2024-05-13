import { DependencyContainer }      from "tsyringe";

import { StaticRouterModService }   from "@spt-aki/services/mod/staticRouter/StaticRouterModService";
import { OnUpdateModService }       from "@spt-aki/services/mod/onUpdate/OnUpdateModService";
import { CustomItemService }        from "@spt-aki/services/mod/CustomItemService";
import { RagfairPriceService }      from "@spt-aki/services/RagfairPriceService";
import { ProbabilityHelper }        from "@spt-aki/helpers/ProbabilityHelper";
import { ProfileHelper }            from "@spt-aki/helpers/ProfileHelper";
import { TraderHelper }             from "@spt-aki/helpers/TraderHelper";
import { ItemHelper }               from "@spt-aki/helpers/ItemHelper";
import { BotHelper }                from "@spt-aki/helpers/BotHelper";
import { DatabaseServer }           from "@spt-aki/servers/DatabaseServer";
import { ConfigServer }             from "@spt-aki/servers/ConfigServer";
import { SaveServer }               from "@spt-aki/servers/SaveServer";
import { IDatabaseTables }          from "@spt-aki/models/spt/server/IDatabaseTables";
import { ILogger }                  from "@spt-aki/models/spt/utils/ILogger";
import { HttpResponseUtil }         from "@spt-aki/utils/HttpResponseUtil";
import { ImporterUtil }             from "@spt-aki/utils/ImporterUtil";
import { RandomUtil }               from "@spt-aki/utils/RandomUtil";
import { HashUtil }                 from "@spt-aki/utils/HashUtil";
import { JsonUtil }                 from "@spt-aki/utils/JsonUtil";
import { VFS }                      from "@spt-aki/utils/VFS";
import { BotController }            from "@spt-aki/controllers/BotController";
import { PreAkiModLoader }          from "@spt-aki/loaders/PreAkiModLoader";
import { ImageRouter }              from "@spt-aki/routers/ImageRouter";

export class References 
{
    public modName: string;
    public debug: boolean;

    public container: DependencyContainer;
    public preAkiModLoader: PreAkiModLoader;
    public configServer: ConfigServer;
    public saveServer: SaveServer;
    public itemHelper: ItemHelper;
    public logger: ILogger;
    public staticRouter: StaticRouterModService;
    public onUpdateModService: OnUpdateModService;

    public database: DatabaseServer;
    public customItem: CustomItemService;
    public imageRouter: ImageRouter;
    public jsonUtil: JsonUtil;
    public profileHelper: ProfileHelper;
    public ragfairPriceService: RagfairPriceService;
    public importerUtil: ImporterUtil;
    public vfs: VFS;
    public tables: IDatabaseTables;
    public botHelper: BotHelper;
    public randomUtil: RandomUtil;
    public hashUtil: HashUtil;
    public probHelper: ProbabilityHelper;
    public traderHelper: TraderHelper;
    public botController: BotController;
    public httpResponse: HttpResponseUtil;

    public preAkiLoad(container: DependencyContainer): void
    {
        this.container = container;
        this.preAkiModLoader = container.resolve<PreAkiModLoader>("PreAkiModLoader");
        this.imageRouter = container.resolve<ImageRouter>("ImageRouter");
        this.configServer = container.resolve<ConfigServer>("ConfigServer");
        this.saveServer = container.resolve<SaveServer>("SaveServer");
        this.itemHelper = container.resolve<ItemHelper>("ItemHelper");
        this.logger = container.resolve<ILogger>("WinstonLogger");
        this.staticRouter = container.resolve<StaticRouterModService>("StaticRouterModService");
        this.onUpdateModService = container.resolve<OnUpdateModService>( "OnUpdateModService" );
        this.randomUtil = container.resolve<RandomUtil>("RandomUtil");
        this.database = container.resolve<DatabaseServer>("DatabaseServer");
        this.tables = container.resolve<DatabaseServer>("DatabaseServer").getTables();
        this.customItem = container.resolve<CustomItemService>("CustomItemService");
        this.jsonUtil = container.resolve<JsonUtil>("JsonUtil");
        this.profileHelper = container.resolve<ProfileHelper>("ProfileHelper");
        this.ragfairPriceService = container.resolve<RagfairPriceService>("RagfairPriceService");
        this.importerUtil = container.resolve<ImporterUtil>("ImporterUtil");
        this.vfs = container.resolve<VFS>("VFS");
        this.botHelper = container.resolve<BotHelper>("BotHelper");
        this.hashUtil = container.resolve<HashUtil>("HashUtil");
        this.probHelper = container.resolve<ProbabilityHelper>("ProbabilityHelper");
        this.traderHelper = container.resolve<TraderHelper>("TraderHelper");
        this.botController = container.resolve<BotController>("BotController");
        this.httpResponse = container.resolve<HttpResponseUtil>("HttpResponseUtil");
    }

    public postDBLoad(container: DependencyContainer): void
    {
        this.container = container;
        this.database = container.resolve<DatabaseServer>("DatabaseServer");
        this.imageRouter = container.resolve<ImageRouter>("ImageRouter");
        this.logger = container.resolve<ILogger>("WinstonLogger");
        this.tables = container.resolve<DatabaseServer>("DatabaseServer").getTables();
        this.customItem = container.resolve<CustomItemService>("CustomItemService");
        this.jsonUtil = container.resolve<JsonUtil>("JsonUtil");
        this.profileHelper = container.resolve<ProfileHelper>("ProfileHelper");
        this.ragfairPriceService = container.resolve<RagfairPriceService>("RagfairPriceService");
        this.importerUtil = container.resolve<ImporterUtil>("ImporterUtil");
        this.vfs = container.resolve<VFS>("VFS");
        this.botHelper = container.resolve<BotHelper>("BotHelper");
        this.randomUtil = container.resolve<RandomUtil>("RandomUtil");
        this.itemHelper = container.resolve<ItemHelper>("ItemHelper");
        this.hashUtil = container.resolve<HashUtil>("HashUtil");
        this.probHelper = container.resolve<ProbabilityHelper>("ProbabilityHelper");
        this.botController = container.resolve<BotController>("BotController");
        this.httpResponse = container.resolve<HttpResponseUtil>("HttpResponseUtil");
    }
}