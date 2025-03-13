using EFT.UI;
using System;

namespace ROStandalone.Configs
{
    internal static class ConsoleCommands
    {
        public static void RegisterCC()
        {
            ConsoleScreen.Processor.RegisterCommand("DoHealEvent",          new Action(Plugin.ECScript.DoHealPlayer));
            ConsoleScreen.Processor.RegisterCommand("DoDamageEvent",        new Action(Plugin.ECScript.DoDamageEvent));
            ConsoleScreen.Processor.RegisterCommand("DoArmorEvent",         new Action(Plugin.ECScript.DoArmorRepair));
            ConsoleScreen.Processor.RegisterCommand("DoAirdropEvent",       new Action(Plugin.ECScript.DoAirdropEvent));
            ConsoleScreen.Processor.RegisterCommand("DoFunnyEvent",         new Action(Plugin.ECScript.DoFunny));
            ConsoleScreen.Processor.RegisterCommand("DoBlackoutEvent",      new Action(Plugin.ECScript.DoBlackoutEvent));
            ConsoleScreen.Processor.RegisterCommand("DoSkillEvent",         new Action(Plugin.ECScript.DoSkillEvent));
            ConsoleScreen.Processor.RegisterCommand("DoMetabolismEvent",    new Action(Plugin.ECScript.DoMetabolismEvent));
            ConsoleScreen.Processor.RegisterCommand("DoMalfEvent",          new Action(Plugin.ECScript.DoMalfEvent));
            ConsoleScreen.Processor.RegisterCommand("DoLLEvent",            new Action(Plugin.ECScript.DoLLEvent));
            ConsoleScreen.Processor.RegisterCommand("DoBerserkEvent",       new Action(Plugin.ECScript.DoBerserkEvent));
            ConsoleScreen.Processor.RegisterCommand("DoWeightEvent",        new Action(Plugin.ECScript.DoWeightEvent));
            ConsoleScreen.Processor.RegisterCommand("DoMaxLLEvent",         new Action(Plugin.ECScript.DoMaxLLEvent));
            ConsoleScreen.Processor.RegisterCommand("DoRepCorrect",         new Action(Plugin.ECScript.CorrectRep));
            ConsoleScreen.Processor.RegisterCommand("DoLockdownEvent",      new Action(Plugin.ECScript.DoLockDownEvent));
            ConsoleScreen.Processor.RegisterCommand("DoArtilleryEvent",     new Action(Plugin.ECScript.DoArtyEvent));
            ConsoleScreen.Processor.RegisterCommand("RunTrain",             new Action(Plugin.ECScript.RunTrain));
            ConsoleScreen.Processor.RegisterCommand("DoPmcExfil",           new Action(Plugin.ECScript.DoPmcExfilEvent));
            ConsoleScreen.Processor.RegisterCommand("ExfilNow",             new Action(Plugin.ECScript.ExfilNow));
            //ConsoleScreen.Processor.RegisterCommand("DoGearExfil",       new Action(Plugin.ECScript.DoGearExfilEvent));
        }
    }
}