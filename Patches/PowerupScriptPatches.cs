using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using I2.Loc;
using Panik;
using UnityEngine;

namespace LuckyHole.Patches;

[HarmonyPatch(typeof(PowerupScript))]
public class PowerupScriptPatches
{
    [HarmonyPatch(nameof(PowerupScript.InitializeAll))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> RegisterOurPowerups(IEnumerable<CodeInstruction> instructions)
    {
        // game has a bunch of:
        // PowerupScript.Spawn(...).Initialize(...);
        // for each powerup. we will add our own powerups after all of the new() lines have been called.
        var codes = new List<CodeInstruction>(instructions);
        
        var insertIndex = codes.FindLastIndex(ci =>
            (ci.opcode == OpCodes.Callvirt || ci.opcode == OpCodes.Call) &&
            ci.operand is MethodInfo { Name: "Spawn" } mi &&
            mi.DeclaringType == typeof(PowerupScript)
        ) + 1;

        codes.InsertRange(insertIndex, [
            new CodeInstruction(OpCodes.Call, typeof(PowerupScriptPatches).GetMethod(nameof(AddOurPowerups), BindingFlags.Static | BindingFlags.NonPublic))
        ]);

        return codes.AsEnumerable();
    }
    
    private static void AddOurPowerups()
    {
        Utils.RegisterPowerups();
    }
}