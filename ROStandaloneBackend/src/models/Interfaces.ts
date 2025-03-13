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
    Seasons: {
        EnableWeatherOptions: boolean;
        AllSeasons: boolean;
        NoWinter: boolean;
        SeasonalProgression: boolean;
        WinterWonderland: boolean;
    };
}

export interface seasonalProgression {
    seasonsProgression: number;
}

export interface debugFile {
    debugMode: boolean;
    dumpData: boolean;
    EnableTimeChanges: boolean;
}
