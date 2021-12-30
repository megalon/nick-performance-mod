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
        static void Postfix(ref GameAgent[] ___updagents, ref GameInstance __instance, ref int ___agentsAdded)
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

                DisableObjectsForStage(stageObj);
            }
        }

        private static void DisableObjectsForStage(GameObject stageObj)
        {
            string stageName = stageObj.name.Contains("(Clone)")
                ? stageObj.name.Substring(0, stageObj.name.IndexOf("(Clone)"))
                : stageObj.name;

            if (!StageConfigLoader.stageConfigDict.ContainsKey(stageName))
            {
                Plugin.LogWarning($"No StageConfig found for stage \"{stageName}\"");
                return;
            }

            Plugin.LogInfo("Disabling objects...");

            string basePath = $"game instance(Clone)/{stageObj.name}/";

            foreach (string objName in StageConfigLoader.stageConfigDict[stageName].ObjectsToDisable)
            {
                string tempObjName = objName;
                if (objName.StartsWith(basePath))
                {
                    tempObjName = objName.Substring(basePath.Length);

                    Transform t = stageObj.transform.Find(tempObjName);

                    if (t == null)
                    {
                        Plugin.LogError($"Could not find object \"{tempObjName}\"");
                        continue;
                    }

                    Plugin.LogDebug($"Disabling gameobject \"{objName}\"");
                    t.gameObject.SetActive(false);
                }
            }
        }
    }
}
