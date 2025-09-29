using System.Reflection.Emit;
using HarmonyLib;

namespace LuckyHole.Patches;

[HarmonyPatch(typeof(GameplayData))]
public class GameplayDataPatches
{
    [HarmonyPatch(nameof(GameplayData._EnsurePowerupDataArray))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> RegisterOurPowerups(IEnumerable<CodeInstruction> instructions)
    {
        // first line sets an int variable called newSize to 164 (the current number of powerups)
        // we need to change that to 500.
        var codes = new List<CodeInstruction>(instructions);
        var index = codes.FindIndex(ci => ci.opcode == OpCodes.Ldc_I4 && ((int)ci.operand == 164 || (int)ci.operand == (int)PowerupScript.Identifier.count));
        if (index != -1) {
            codes[index].operand = 500;
        } else {
            Utils.PLogger.LogError("Failed to patch GameplayData._EnsurePowerupDataArray: couldn't find ldc.i4 164");
        }
        
        return codes.AsEnumerable();
    }
}