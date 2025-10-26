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
                    "Has a 35% chance of granting <rainb>1-5 Jackpots</rainb> <sprite name=\"PtJ\">, or a 65% chance to grant a 666, on the next round. Then discard this charm.",
                [Translation.Language.Italian] =
                    "Ha il 35% di probabilità di concedere <rainb>1-5 Jackpot</rainb> <sprite name=\"PtJ\">, o il 65% di probabilità di concedere un 666, nel prossimo round. Poi scarta questo amuleto.",
                [Translation.Language.French] =
                    "A 35 % de chances d'accorder <rainb>1-5 jackpots</rainb> <sprite name=\"PtJ\">, ou 65 % de chances d'accorder un 666, au prochain tour. Ensuite, jetez ce charme.",
                [Translation.Language.German] =
                    "Hat eine 35%ige Chance, <rainb>1-5 Jackpots</rainb> <sprite name=\"PtJ\"> zu gewähren, oder eine 65%ige Chance, eine 666 zu gewähren, in der nächsten Runde. Dann wirf diesen Charme weg.",
    
                [Translation.Language.Spanish] =
                    "Tiene un 35% de probabilidad de otorgar <rainb>1-5 jackpots</rainb> <sprite name=\"PtJ\">, o un 65% de probabilidad de otorgar un 666, en la siguiente ronda. Luego descarta este amuleto.",
                [Translation.Language.SpanishAmerica] =
                    "Tiene un 35% de probabilidad de otorgar <rainb>1-5 jackpots</rainb> <sprite name=\"PtJ\">, o un 65% de probabilidad de otorgar un 666, en la siguiente ronda. Luego descarta este amuleto.",
    
                [Translation.Language.Portuguese] =
                    "Tem 35% de chance de conceder <rainb>1-5 Jackpots</rainb> <sprite name=\"PtJ\">, ou 65% de chance de conceder um 666, na próxima rodada. Então descarte este amuleto.",
                [Translation.Language.PortugueseBrazil] =
                    "Tem 35% de chance de conceder <rainb>1-5 Jackpots</rainb> <sprite name=\"PtJ\">, ou 65% de chance de conceder um 666, na próxima rodada. Então descarte este amuleto.",
                     
                [Translation.Language.ChineseSimplified] =
                    "在下一轮中有35%的几率获得<rainb>1-5个大奖</rainb> <sprite name=\"PtJ\">，或65%的几率获得666。然后丢弃这个护符。",
                [Translation.Language.Japanese] =
                    "次のラウンドで<rainb>1-5ジャックポット</rainb> <sprite name=\"PtJ\">を獲得する35％の確率、または666を獲得する65％の確率があります。その後、このチャームを破棄します。",
                     
                [Translation.Language.Ukraine] =
                    "Має 35% шанс отримати <rainb>1-5 Джекпотів</rainb> <sprite name=\"PtJ\">, або 65% шанс отримати 666, у наступному раунді. Потім викинь цей амулет.",
                [Translation.Language.Russian] =
                    "Имеет 35% шанс получить <rainb>1-5 Джекпотов</rainb> <sprite name=\"PtJ\">, или 65% шанс получить 666, в следующем раунде. Затем выбросьте этот амулет.",
                     
                [Translation.Language.Korean] =
                    "다음 라운드에서 <rainb>1-5 잭팟</rainb> <sprite name=\"PtJ\">을(를) 획득할 확률이 35%, 666을(를) 획득할 확률이 65%입니다. 그런 다음 이 부적을 버리십시오.",
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
}