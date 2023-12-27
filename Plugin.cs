using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace SafeSoloScrap
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "butterystancakes.lethalcompany.safesoloscrap", PLUGIN_NAME = "Safe Solo Scrap", PLUGIN_VERSION = "1.0.0";
        public static ConfigEntry<bool> configKeepTwoHanded;

        void Awake()
        {
            configKeepTwoHanded = Config.Bind("Gameplay", "KeepTwoHanded", false, "Retain two-handed items?");

            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}