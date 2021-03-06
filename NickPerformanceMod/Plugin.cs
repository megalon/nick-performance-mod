using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;
using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.IO;
using NickPerformanceMod.Management;

namespace NickPerformanceMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;
        internal static string PerformanceModRootPath;
        public static bool WaitingForUpdate = false;

        private void Awake()
        {
            Logger.LogDebug($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");

            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            var config = this.Config;
            config.SettingChanged += OnConfigSettingChanged;

            PerformanceModRootPath = Path.Combine(Paths.BepInExRootPath, "Performance Mod");
            Directory.CreateDirectory(PerformanceModRootPath);

            StageConfigLoader.Init();

            // Harmony patches
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        static void OnConfigSettingChanged(object sender, EventArgs args)
        {
            LogDebug($"{PluginInfo.PLUGIN_NAME} OnConfigSettingChanged");
            Plugin.Instance?.Config?.Reload();
        }

        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
    }
}
