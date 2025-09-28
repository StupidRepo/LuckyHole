using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using I2.Loc;

namespace LuckyHole.Patches;

[HarmonyPatch(typeof(AbilityScript))]
public class AbilityScriptPatches
{
    [HarmonyPatch(nameof(AbilityScript.InitializeAll))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> RegisterOurAbilities(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
    //     
    //     var insertIndex = codes.FindLastIndex(ci =>
    //         (ci.opcode == OpCodes.Callvirt || ci.opcode == OpCodes.Call) &&
    //         ci.operand is MethodInfo { Name: "Initialize" } mi &&
    //         mi.DeclaringType == typeof(AbilityScript)
    //     ) + 1;
    //
    //     codes.InsertRange(insertIndex, [
    //         new CodeInstruction(OpCodes.Call, typeof(AbilityScriptPatches).GetMethod(nameof(AddOurAbilities), BindingFlags.Static | BindingFlags.NonPublic))
    //     ]);
    //
        return codes.AsEnumerable();
    }
    
    // our abilities
    // private static void AddOurAbilities()
    // {
    //     Utils.PLogger.LogInfo("Registering custom abilities...");
    //     
    //     try {
    //         Utils.AddNewTranslationsAndUpdate(new Dictionary<string, string>
    //         {
    //             { "ABILITY_NAME_TEST", "My luck is as good as my morning coffee!" },
    //             { "ABILITY_DESCR_TEST", "Has a 50% chance of granting <rainb>1-3 Jackpots</rainb> <sprite name=\"PtJ\"> or triggering a 666, on the next round." },
    //             { "ABILITY_REPLY_TEST", "You're not as lucky as you think you are..." }
    //         });
    //     }
    //     catch (Exception ex)
    //     {
    //         Utils.PLogger.LogError($"Error registering custom localization source: {ex}");
    //     }
    //     
    //     try
    //     {
    //         // Test Ability - on pick, the last round of the deadline has a 50% chance to give up to 3 jackpots (random from 1-3), or trigger a 666 (resets all coins earnt that round)
    //         // new AbilityScript().Initialize(Utils.TestAbility,
    //         //     AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.common,
    //         //     "ABILITY_NAME_TEST", "ABILITY_DESCR_TEST",
    //         //     [ "ABILITY_REPLY_TEST" ],
    //         //     "SpriteAbility_Generic_ExtraSpace", null,
    //         //     -1,
    //         //     AbilityScript.AFunc_OnPick_ExtraSpace);
    //     }
    //     catch (Exception ex)
    //     {
    //         Utils.PLogger.LogError($"Error registering custom ability: {ex}");
    //     }
    // }
}