using System.Numerics;
using HarmonyLib;
using LuckyHole.Powerups;
using Panik;
using UnityEngine;

namespace LuckyHole.Patches;

[HarmonyPatch(typeof(SlotMachineScript))]
public class SlotMachineScriptPatches
{
    // TODO: this feels really bad. find a better way to do this.
    [HarmonyPatch(nameof(SlotMachineScript.PatternInfoSetup))]
    [HarmonyPrefix]
    private static void OnPatternInfoSetup_Prefix(PatternScript.Kind _patternKind, BigInteger _coins, List<Vector2Int> _positionsToCopy)
    {
        if (_patternKind != PatternScript.Kind.jackpot || !PowerupGoldenPony.ShouldGiveJackpots) return;
        PowerupGoldenPony.ShouldGiveJackpots = false;

        Utils.PLogger.LogInfo(_coins);
        Utils.PLogger.LogInfo(_positionsToCopy);
        
        var jackpotsToGive = R.Rng_Powerup(Utils.GoldenPony).Range(1, 6);
        Utils.PLogger.LogInfo($"Golden Pony giving +{jackpotsToGive} extra jackpots.");
        for (var i = 0; i < jackpotsToGive; i++)
        {
            SlotMachineScript.instance.PatternInfoSetup(
                PatternScript.Kind.jackpot, _coins, _positionsToCopy);
            
        }
    }
}