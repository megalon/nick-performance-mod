using BepInEx;
using Newtonsoft.Json;
using NickPerformanceMod.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NickPerformanceMod.Management
{
    class StageConfigLoader
    {
        internal static Dictionary<string, StageConfigData> stageConfigDict;
        static string stageConfigsPath;

        public static void Init()
        {
            stageConfigsPath = Path.Combine(Plugin.PerformanceModRootPath, "Stages");

            Directory.CreateDirectory(stageConfigsPath);

            stageConfigDict = new Dictionary<string, StageConfigData>();

            foreach (string filePath in 
                from x in Directory.GetFiles(stageConfigsPath) 
                where x.ToLower().EndsWith(".json") 
                select x
                )
            {
                LoadStageConfig(filePath);
            }
        }

        private static void LoadStageConfig(string jsonPath)
        {
			if (File.Exists(jsonPath))
			{
				try
				{
					string jsonFile = File.ReadAllText(jsonPath);

					var stageConfigData = JsonConvert.DeserializeObject<StageConfigData>(jsonFile);

                    if (stageConfigDict.ContainsKey(stageConfigData.StageId))
                    {
                        Plugin.LogError($"Duplicate config file found for \"{stageConfigData.StageId}\"\n{jsonFile}");
                    } else
                    {
                        stageConfigDict.Add(stageConfigData.StageId, stageConfigData);
                    }
                }
				catch (Exception e)
				{
					Plugin.LogError($"Error reading json data for {jsonPath}");
					Plugin.LogError(e.Message);
				}
			}
			else
			{
				Plugin.LogError($"Could not find JSON file: \"{jsonPath}\"");
			}
		}
    }
}
