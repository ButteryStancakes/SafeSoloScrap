using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace SafeSoloScrap.Patches
{
    [HarmonyPatch]
    class SafeSoloScrapPatches
    {
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyTranspiler]
        [HarmonyPriority(Priority.LowerThanNormal)]
        [HarmonyAfter(Plugin.GUID_LATE_GAME_UPGRADES)]
        public static IEnumerable<CodeInstruction> RoundManager_Trans_DespawnPropsAtEndOfRound(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();

            FieldInfo isScrap = AccessTools.Field(typeof(Item), nameof(Item.isScrap)), allPlayersDead = AccessTools.Field(typeof(StartOfRound), nameof(StartOfRound.allPlayersDead));
            for (int i = 6; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldfld && (FieldInfo)codes[i].operand == isScrap && codes[i - 2].opcode == OpCodes.Ldelem_Ref && codes[i - 3].opcode == OpCodes.Ldloc_S && codes[i - 4].opcode == OpCodes.Ldloc_0 && codes[i - 6].opcode == OpCodes.Ldfld && (FieldInfo)codes[i - 6].operand == allPlayersDead)
                {
                    codes.InsertRange(i + 1, [
                        new(OpCodes.Ldloc_0),
                        new(OpCodes.Ldloc_S, codes[i - 3].operand),
                        new(OpCodes.Ldelem_Ref),
                        new(OpCodes.Call, AccessTools.Method(typeof(ScrapSafe), nameof(ScrapSafe.IsItemLost))),
                    ]);
                    Plugin.Logger.LogDebug("Transpiler (Penalty): Redirect scrap check to custom function");
                    return codes;
                }
            }

            Plugin.Logger.LogError("Penalty transpiler failed");
            return instructions;
        }
    }
}