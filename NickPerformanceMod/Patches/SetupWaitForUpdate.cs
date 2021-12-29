using HarmonyLib;
using Nick;
using System;
using System.Collections.Generic;
using System.Text;

namespace NickPerformanceMod.Patches
{
    [HarmonyPatch(typeof(GameInstance), "PrepareInstance")]
    class GameInstance_PrepareInstance
    {
        static void Postfix()
        {
            Plugin.WaitingForUpdate = true;
        }
    }

    [HarmonyPatch(typeof(OnlineWaitMatchScreen), "GameStarted")]
    class OnlineWaitMatchScreen_GameStarted
    {
        static void Postfix()
        {
            Plugin.WaitingForUpdate = true;
        }
    }
}
