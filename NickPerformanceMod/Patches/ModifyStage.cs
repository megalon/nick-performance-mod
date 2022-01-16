using HarmonyLib;
using Nick;
using NickPerformanceMod.Management;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NickPerformanceMod.Patches
{
    [HarmonyPatch(typeof(GameInstance), "DoFrame")]
    class GameInstance_DoFrame
    {
        static void Postfix(ref GameAgent[] ___updagents, ref GameInstance __instance, ref int ___agentsAdded) => GameInstance_DoFrame.UpdateLogic(ref ___updagents, ref __instance, ref ___agentsAdded);

        public static void UpdateLogic(ref GameAgent[] ___updagents, ref GameInstance __instance, ref int ___agentsAdded)
        {
            if (Plugin.WaitingForUpdate)
            {
                Plugin.WaitingForUpdate = false;

                // Find the stage gameobject
                GameObject stageObj = null;
                foreach (Transform child in __instance.transform)
                {
                    if (child.name.StartsWith("stage_"))
                    {
                        stageObj = child.gameObject;
                        break;
                    }
                }

                if (stageObj == null)
                {
                    Debug.LogError("Could not find stage object!");
                    return;
                }

                ModifyStage(stageObj);
            }
        }

        private static void ModifyStage(GameObject stageObj)
        {
            string stageName = stageObj.name.Contains("(Clone)")
                ? stageObj.name.Substring(0, stageObj.name.IndexOf("(Clone)"))
                : stageObj.name;

            if (!StageConfigLoader.stageConfigDict.ContainsKey(stageName))
            {
                Plugin.LogWarning($"No StageConfig found for stage \"{stageName}\"");
                return;
            }

            EnableObjectsInStage(stageObj, stageName);
            DisableObjectsInStage(stageObj, stageName);
        }

        private static void EnableObjectsInStage(GameObject stageObj, string stageName)
        {
            Plugin.LogInfo("Enabling gameobjects...");
            EnableOrDisableObjectsInStage(stageObj, StageConfigLoader.stageConfigDict[stageName].ObjectsToEnable, true);
        }

        private static void DisableObjectsInStage(GameObject stageObj, string stageName)
        {
            Plugin.LogInfo("Disabling gameobjects...");
            EnableOrDisableObjectsInStage(stageObj, StageConfigLoader.stageConfigDict[stageName].ObjectsToDisable, false);
        }

        private static void EnableOrDisableObjectsInStage(GameObject stageObj, string[] objects, bool enabled)
        {
            if (objects == null || objects.Length == 0)
            {
                Plugin.LogInfo($"No objects to {(enabled ? "enable" : "disable")} in config");
                return;
            }

            string basePath = $"game instance(Clone)/{stageObj.name}/";

            foreach (string objName in objects)
            {
                if (objName.StartsWith(basePath))
                {
                    string tempObjName = objName.Substring(basePath.Length);

                    Transform t = stageObj.transform.Find(tempObjName);

                    if (t == null)
                    {
                        Plugin.LogError($"Could not find object \"{tempObjName}\"");
                        continue;
                    }

                    Plugin.LogDebug($"{(enabled ? "Enabling" : "Disabling")} gameobject \"{objName}\"");
                    t.gameObject.SetActive(enabled);
                }
            }
        }
    }


    [HarmonyPatch(typeof(GameInstance), "OnlineDoFrameStoreFXDT")]
    class OnlineUpdatePatch
    {
        static void Postfix(ref GameAgent[] ___updagents, ref GameInstance __instance, ref int ___agentsAdded) => GameInstance_DoFrame.UpdateLogic(ref ___updagents, ref __instance, ref ___agentsAdded);
    }
}
