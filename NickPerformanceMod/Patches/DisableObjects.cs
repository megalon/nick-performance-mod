using HarmonyLib;
using Nick;
using System;
using System.Collections.Generic;
using System.Text;

namespace NickPerformanceMod.Patches
{
    [HarmonyPatch(typeof(GameInstance), "DoFrame")]
    class GameInstance_DoFrame
    {
        static void Postfix(ref GameAgent[] ___updagents, ref GameInstance __instance, ref int ___agentsAdded)
        {
            if (Plugin.WaitingForUpdate)
            {
                Plugin.WaitingForUpdate = false;
                // Disable the gameobjects here

                Plugin.LogInfo("Disabling objects...");
            }
        }
    }
}
