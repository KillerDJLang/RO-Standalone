$buildFolder = "..\Build"
$bepinexFolder = "$buildFolder\BepInEx"
$userModsFolder = "$buildFolder\user\mods\RaidOverhaulStandalone"
$bepinexPluginFolder = "$bepinexFolder\plugins\RaidOverhaulStandalone"
$bepinexResourcesFolder = "$bepinexPluginFolder\Resources"
$bepinexFlagsFolder = "$bepinexResourcesFolder\Flags"
$serverModFolder = "..\ROStandaloneBackend"
$packetsFolder = "..\ROStandalonePackets\bin\Release\net471"
$pluginFolder = "..\ROStandalonePlugin\bin\Release\net471"
$basePluginFolder = "..\ROStandalonePlugin"

if (Test-Path "$buildFolder") { Remove-Item -Path "$buildFolder" -Recurse -Force }

$foldersToCreate = @("$buildFolder", "$bepinexFolder", "$bepinexPluginFolder", "$bepinexResourcesFolder",  "$bepinexFlagsFolder", "$userModsFolder")
foreach ($folder in $foldersToCreate) {
    if (-not (Test-Path "$folder")) { New-Item -Path "$folder" -ItemType Directory }
}

Copy-Item "$packetsFolder\RaidOverhaulPackets.dll" -Destination "$bepinexPluginFolder" -Force
Copy-Item "$pluginFolder\ROStandalone.dll" -Destination "$bepinexPluginFolder" -Force
Copy-Item "$basePluginFolder\TraderRep.json" -Destination "$bepinexFlagsFolder" -Force
Copy-Item "$serverModFolder\config" -Destination "$userModsFolder" -Recurse -Force
Copy-Item "$serverModFolder\db" -Destination "$userModsFolder" -Recurse -Force
Copy-Item "$serverModFolder\src" -Destination "$userModsFolder" -Recurse -Force
Copy-Item "$serverModFolder\package.json" -Destination "$userModsFolder" -Force