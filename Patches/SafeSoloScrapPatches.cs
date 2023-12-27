using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace SafeSoloScrap.Patches
{
    [HarmonyPatch]
    class SafeSoloScrapPatches
    {
        [HarmonyPatch(typeof(RoundManager), "DespawnPropsAtEndOfRound")]
        [HarmonyPrefix]
        public static bool DespawnPropsAtEndOfRound(RoundManager __instance, bool despawnAllItems)
        {
            if (!__instance.IsServer)
                return false;

            GrabbableObject[] array = Object.FindObjectsOfType<GrabbableObject>();
            for (int i = 0; i < array.Length; i++)
            {
                if (despawnAllItems || (!array[i].isHeld && !array[i].isInShipRoom) || array[i].deactivated || (StartOfRound.Instance.allPlayersDead && array[i].itemProperties.isScrap && (StartOfRound.Instance.connectedPlayersAmount > 0 || (array[i].itemProperties.twoHanded && !Plugin.configKeepTwoHanded.Value))))
                {
                    if (array[i].isHeld && array[i].playerHeldBy != null)
                        array[i].playerHeldBy.DropAllHeldItems();

                    array[i].gameObject.GetComponent<NetworkObject>().Despawn();
                }
                else
                    array[i].scrapPersistedThroughRounds = true;

                if (__instance.spawnedSyncedObjects.Contains(array[i].gameObject))
                    __instance.spawnedSyncedObjects.Remove(array[i].gameObject);
            }

            GameObject[] array2 = GameObject.FindGameObjectsWithTag("TemporaryEffect");
            for (int j = 0; j < array2.Length; j++)
                Object.Destroy(array2[j]);

            return false;
        }
    }
}