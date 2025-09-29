using System.Runtime.CompilerServices;
using Panik;
using UnityEngine;

namespace LuckyHole.Powerups;

public class PowerupGoldenPony : APowerUp
{
    public override PowerupScript.Identifier ID { get; } = Utils.GoldenPony;
    
    public override string NameKey { get; } = POWERUP_NAME_PREFIX + "GOLDEN_PONY";
    public override string DescriptionKey { get; } = POWERUP_DESC_PREFIX + "GOLDEN_PONY";
    public override string UnlockMissionKey { get; } = "POWERUP_UNLOCK_MISSION_ONE_TRICK_PONY";

    public override int MaxBuyTimes { get; } = 1;
    public override int StartingPrice { get; } = 4;
    public override float StoreRerollChance { get; } = 0.35f;

    public override bool RegisterAssets(string name)
    {
        return base.RegisterAssets("powerup golden pony");
    }

    public override PowerupScript.PowerupEvent OnEquip { get; } = _ =>
    {
        SlotMachineScript.instance.OnRoundBeing += PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin;
        SlotMachineScript.instance.OnScoreEvaluationBegin += Trigger;
        SlotMachineScript.instance.OnPatternEvaluationEnd += GiveJackpots;
    };

    public override PowerupScript.PowerupEvent OnUnequip { get; } = _ =>
    {
        SlotMachineScript.instance.OnRoundBeing -= PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin;
        SlotMachineScript.instance.OnScoreEvaluationBegin -= Trigger;
    };

    private static void Trigger()
    {
        if (GameplayData.Powerup_OneTrickPony_TargetSpinIndexGet() != GameplayData.SpinsLeftGet()) return;
        if (SlotMachineScript.Has666()) return; // won't trigger if there's already a 666, but if there's a jackpot then potentially f*** you hahaha
        Utils.PLogger.LogInfo("Golden Pony triggered.");
        if (R.Rng_Powerup(Utils.GoldenPony).Value <= 0.50f)
        {
            // TODO: trigger 1-5 jackpots
            SlotMachineScript.Symbol_ReplaceAllVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, true);
        }
        else
        {
            // trigger 666
            SlotMachineScript.Symbol_ReplaceVisible(SymbolScript.Kind.six, SymbolScript.Modifier.none, 1, 1, true);
            SlotMachineScript.Symbol_ReplaceVisible(SymbolScript.Kind.six, SymbolScript.Modifier.none, 2, 1, true);
            SlotMachineScript.Symbol_ReplaceVisible(SymbolScript.Kind.six, SymbolScript.Modifier.none, 3, 1, true);
            
            PowerupScript.ThrowAway(Utils.GoldenPony, false);
        }
        
        PowerupScript.PlayTriggeredAnimation(Utils.GoldenPony);
    }
    
    private static void GiveJackpots(SlotMachineScript.PatternInfos ev)
    {
        if(ev.patternKind != PatternScript.Kind.jackpot || !PowerupScript.IsEquipped_Quick(Utils.GoldenPony)) return;
        int jackpotsToGive = R.Rng_Powerup(Utils.GoldenPony).Range(1, 6);
        Utils.PLogger.LogInfo($"Golden Pony giving +{jackpotsToGive} extra jackpots.");
        for (int i = 0; i < jackpotsToGive; i++)
        {
            SlotMachineScript.instance._patternInfos.Add(ev);
        }
        PowerupScript.ThrowAway(Utils.GoldenPony, false);
    }
    
    protected override Dictionary<string, (List<string> languageCodes, List<string> translations)> Translations { get; } = new()
    {
        {
            POWERUP_NAME_PREFIX + "GOLDEN_PONY", (["en"], ["Golden Pony"])
        },
        {
            POWERUP_DESC_PREFIX + "GOLDEN_PONY", (["en"],
            [
                "Has a 45% chance of granting <rainb>1-5 Jackpots</rainb> <sprite name=\"PtJ\">, or a 55% chance to grant a 666, on the next round. Then discard this charm."
            ])
        },
    };
}