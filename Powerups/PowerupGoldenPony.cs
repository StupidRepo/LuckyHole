using Panik;

namespace LuckyHole.Powerups;

public class PowerupGoldenPony : APowerUp
{
    public static bool ShouldGiveJackpots = false;
    
    public override PowerupScript.Identifier ID { get; } = Utils.GoldenPony;

    protected override string NameKey { get; } = POWERUP_NAME_PREFIX + "GOLDEN_PONY";
    protected override string DescriptionKey { get; } = POWERUP_DESC_PREFIX + "GOLDEN_PONY";
    protected override string UnlockMissionKey { get; } = "POWERUP_UNLOCK_MISSION_ONE_TRICK_PONY";

    protected override int MaxBuyTimes { get; } = 1;
    protected override int StartingPrice { get; } = 4;
    protected override float StoreRerollChance { get; } = 0.25f;

    public override bool RegisterAssets(string name)
    {
        return base.RegisterAssets("powerup golden pony");
    }

    protected override PowerupScript.PowerupEvent OnEquip { get; } = _ =>
    {
        SlotMachineScript.instance.OnRoundBeing += PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin;
        SlotMachineScript.instance.OnScoreEvaluationBegin += Trigger;
    };

    protected override PowerupScript.PowerupEvent OnUnequip { get; } = _ =>
    {
        SlotMachineScript.instance.OnRoundBeing -= PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin;
        SlotMachineScript.instance.OnScoreEvaluationBegin -= Trigger;
    };

    private static void Trigger()
    {
        if (GameplayData.Powerup_OneTrickPony_TargetSpinIndexGet() != GameplayData.SpinsLeftGet()) return;
        if (SlotMachineScript.Has666()) return; // won't trigger if there's already a 666, but if there's a jackpot then potentially f*** you hahaha
        Utils.PLogger.LogInfo("Golden Pony triggered.");
        
        PowerupScript.PlayTriggeredAnimation(Utils.GoldenPony);
        if (R.Rng_Powerup(Utils.GoldenPony).Value <= 0.35f * GameplayData.ActivationLuckGet())
        {
            // TODO: trigger 1-5 jackpots
            SlotMachineScript.Symbol_ReplaceAllVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, true);
            ShouldGiveJackpots = true;
            
            PowerupScript.ThrowAway(Utils.GoldenPony, false);
        }
        else
        {
            // trigger 666
            SlotMachineScript.Symbol_ReplaceVisible(SymbolScript.Kind.six, SymbolScript.Modifier.none, 1, 1, true);
            SlotMachineScript.Symbol_ReplaceVisible(SymbolScript.Kind.six, SymbolScript.Modifier.none, 2, 1, true);
            SlotMachineScript.Symbol_ReplaceVisible(SymbolScript.Kind.six, SymbolScript.Modifier.none, 3, 1, true);
            
            PowerupScript.ThrowAway(Utils.GoldenPony, false);
        }
    }
    
    protected override Dictionary<string, (List<string> languageCodes, List<string> translations)> Translations { get; } = new()
    {
        {
            POWERUP_NAME_PREFIX + "GOLDEN_PONY", (["en"], ["Golden Pony"])
        },
        {
            POWERUP_DESC_PREFIX + "GOLDEN_PONY", (["en"],
            [
                "Has a 35% chance of granting <rainb>1-5 Jackpots</rainb> <sprite name=\"PtJ\">, or a 65% chance to grant a 666, on the next round. Then discard this charm."
            ])
        },
    };
}