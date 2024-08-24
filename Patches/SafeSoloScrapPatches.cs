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
        public static IEnumerable<CodeInstruction> TransDespawnPropsAtEndOfRound(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();

            FieldInfo isScrap = AccessTools.Field(typeof(Item), nameof(Item.isScrap));
            for (int i = 2; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Brfalse && codes[i - 1].opcode == OpCodes.Ldfld && (FieldInfo)codes[i - 1].operand == isScrap)
                {
                    codes[i - 1].opcode = OpCodes.Call;
                    codes[i - 1].operand = AccessTools.Method(typeof(ScrapSafe), nameof(ScrapSafe.IsItemLost));
                    codes.RemoveAt(i - 2);
                    Plugin.Logger.LogDebug("Transpiler (Penalty): Redirect scrap check to custom function");
                    return codes;
                }
            }

            Plugin.Logger.LogError("Penalty transpiler failed");
            return codes;
        }
    }
}