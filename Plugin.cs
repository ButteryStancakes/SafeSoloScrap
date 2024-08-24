using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace SafeSoloScrap
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "butterystancakes.lethalcompany.safesoloscrap", PLUGIN_NAME = "Safe Solo Scrap", PLUGIN_VERSION = "2.0.0";
        public static ConfigEntry<int> configMaxPlayers;

        internal static new ManualLogSource Logger;

        void Awake()
        {
            configMaxPlayers = Config.Bind(
                "Gameplay",
                "MaxPlayers",
                1,
                new ConfigDescription("How many players are allowed in the lobby before vanilla death penalty takes over?", new AcceptableValueRange<int>(1, 3)));

            // clean up config
            Config.Bind("Gameplay", "KeepTwoHanded", false, "Legacy setting");
            Config.Remove(Config["Gameplay", "KeepTwoHanded"].Definition);

            Logger = base.Logger;

            new Harmony(PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}