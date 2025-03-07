using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace SafeSoloScrap
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(GUID_LOBBY_COMPATIBILITY, BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PLUGIN_GUID = "butterystancakes.lethalcompany.safesoloscrap", PLUGIN_NAME = "Safe Solo Scrap", PLUGIN_VERSION = "2.0.1";
        public static ConfigEntry<int> configMaxPlayers;

        const string GUID_LOBBY_COMPATIBILITY = "BMX.LobbyCompatibility";

        internal static new ManualLogSource Logger;

        void Awake()
        {
            Logger = base.Logger;

            if (Chainloader.PluginInfos.ContainsKey(GUID_LOBBY_COMPATIBILITY))
            {
                Logger.LogInfo("CROSS-COMPATIBILITY - Lobby Compatibility detected");
                LobbyCompatibility.Init();
            }

            configMaxPlayers = Config.Bind(
                "Gameplay",
                "MaxPlayers",
                1,
                new ConfigDescription("How many players are allowed in the lobby before vanilla death penalty takes over?", new AcceptableValueRange<int>(1, 3)));

            // clean up config
            Config.Bind("Gameplay", "KeepTwoHanded", false, "Legacy setting, doesn't work");
            Config.Remove(Config["Gameplay", "KeepTwoHanded"].Definition);
            Config.Save();

            new Harmony(PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}