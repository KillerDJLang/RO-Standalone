//#region Enums
export enum Currency {
    Roubles = "5449016a4bdc2d6f028b456f",
    Dollars = "5696686a4bdc2da3298b456a",
    Euros = "569668774bdc2da2298b4568",
}

export enum AllBots {
    ArenaFighter = "arenafighter",
    ArenaFighterEvent = "arenafighterevent",
    Assault = "assault",
    Bear = "bear",
    Reshala = "bossbully",
    Gluhar = "bossgluhar",
    Killa = "bosskilla",
    Knight = "bossknight",
    Shturman = "bosskojaniy",
    Sanitar = "bosssanitar",
    Tagilla = "bosstagilla",
    Zryachiy = "bosszryachiy",
    CrazyAssaultEvent = "crazyassaultevent",
    CursedAssault = "cursedassault",
    Rogue = "exusec",
    Bigpipe = "followerbigpipe",
    Birdeye = "followerbirdeye",
    FollowerBully = "followerbully",
    FollowerGluharAssault = "followergluharassault",
    FollowerGluharScout = "followergluharscout",
    FollowerGluharSecurity = "followergluharsecurity",
    FollowerGluharSnipe = "followergluharsnipe",
    FollowerKojaniy = "followerkojaniy",
    FollowerSanitar = "followersanitar",
    FollowerTagilla = "followertagilla",
    FollowerZryachiy = "followerzryachiy",
    Santa = "gifter",
    Sniper = "marksman",
    Raider = "pmcbot",
    Priest = "sectantpriest",
    Cultist = "sectantwarrior",
    Usec = "usec",
}
//#endregion

export interface configFile {
    Raid: {
        ReduceFoodAndHydroDegrade: {
            Enabled: boolean;
            EnergyDecay: number;
            HydroDecay: number;
        };
        EnableExtendedRaids: boolean;
        TimeLimit: number;
    };
    LootChanges: {
        EnableLootOptions: boolean;
        StaticLootMultiplier: number;
        LooseLootMultiplier: number;
        MarkedRoomLootMultiplier: number;
    };
    Events: {
        EnableWeatherOptions: boolean;
        AllSeasons: boolean;
        NoWinter: boolean;
        SeasonalProgression: boolean;
        WinterWonderland: boolean;
        RandomizedSeasonalEvents: boolean;
    };
    Debug: {
        ExtraLogging: boolean;
    };
}

export interface seasonalProgression {
    seasonsProgression: number;
}
