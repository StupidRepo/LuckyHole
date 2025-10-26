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
    
    public override bool RegisterTranslations()
    {
        return 
            Utils.AddNewTranslation(POWERUP_NAME_PREFIX + "GOLDEN_PONY",
                new Utils.ModLocalizedString(new Dictionary<Translation.Language, string>
                {
                    [Translation.Language.English] = "Golden Pony",
                    [Translation.Language.Italian] = "Pony d'Oro",
                    [Translation.Language.French] = "Poney Doré",
                    [Translation.Language.German] = "Goldenes Pony",

                    [Translation.Language.Spanish] = "Poni Dorado",
                    [Translation.Language.SpanishAmerica] = "Poni Dorado",

                    [Translation.Language.Portuguese] = "Pônei Dourado",
                    [Translation.Language.PortugueseBrazil] = "Pônei Dourado",

                    [Translation.Language.ChineseSimplified] = "金色小马",
                    [Translation.Language.Japanese] = "ゴールデンポニー",

                    [Translation.Language.Ukraine] = "Золотий Поні",
                    [Translation.Language.Russian] = "Золотой Пони",

                    [Translation.Language.Korean] = "황금 조랑말",
                }))
            && Utils.AddNewTranslation(POWERUP_DESC_PREFIX + "GOLDEN_PONY", new Utils.ModLocalizedString(new Dictionary<Translation.Language, string>
            {
                [Translation.Language.English] =
                    "On the next round at the slot machine, grant up to <rainb>5 consecutive Jackpots</rainb> <sprite name=\"PtJ\"> (34%) or grant a 666 (66%). Then discard this charm.",
            }));
    }

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
        if (R.Rng_Powerup(Utils.GoldenPony).Value <= 0.34f * GameplayData.ActivationLuckGet()) // 34% chance
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
}