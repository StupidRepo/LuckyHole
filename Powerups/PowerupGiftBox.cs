using Panik;
using UnityEngine;

namespace LuckyHole.Powerups;

public class PowerupGiftBox : APowerUp
{
    public override PowerupScript.Identifier ID { get; } = Utils.Giftbox;

    protected override string NameKey { get; } = POWERUP_NAME_PREFIX + "GIFTBOX";
    protected override string DescriptionKey { get; } = POWERUP_DESC_PREFIX + "GIFTBOX";
    protected override string UnlockMissionKey { get; } = "POWERUP_UNLOCK_MISSION_NONE";

    protected override int MaxBuyTimes { get; } = -1;
    protected override int StartingPrice { get; } = 3;
    protected override float StoreRerollChance { get; } = 0.15f;

    public override bool RegisterTranslations()
    {
        return 
            Utils.AddNewTranslation(POWERUP_NAME_PREFIX + "GIFTBOX",
                new Utils.ModLocalizedString(new Dictionary<Translation.Language, string>
                {
                    [Translation.Language.English] = "Gift Box",
                    [Translation.Language.Italian] = "Scatola Regalo",
                    [Translation.Language.French] = "Boîte Cadeau",
                    [Translation.Language.German] = "Geschenkbox",

                    [Translation.Language.Spanish] = "Caja de Regalo",
                    [Translation.Language.SpanishAmerica] = "Caja de Regalo",

                    [Translation.Language.Portuguese] = "Caixa de Presente",
                    [Translation.Language.PortugueseBrazil] = "Caixa de Presente",

                    [Translation.Language.ChineseSimplified] = "礼物盒",
                    [Translation.Language.Japanese] = "ギフトボックス",

                    [Translation.Language.Ukraine] = "Подарункова Коробка",
                    [Translation.Language.Russian] = "Подарочная Коробка",

                    [Translation.Language.Korean] = "선물 상자",
                }))
            && Utils.AddNewTranslation(POWERUP_DESC_PREFIX + "GIFTBOX", new Utils.ModLocalizedString(new Dictionary<Translation.Language, string>
            {
                [Translation.Language.English] =
                    "[K_RANDOM_ACTIVATION] (2.5%):\n" +
                    "Picks a random ability and applies it. 10% chance of discarding afterwards.",
                [Translation.Language.Italian] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Sceglie un'abilità casuale e la applica. 10% di probabilità di scartarla in seguito.",
                [Translation.Language.French] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Choisit une compétence aléatoire et l'applique. 10% de chance d'être défaussé après.",
                [Translation.Language.German] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Wählt eine zufällige Fähigkeit aus und wendet sie an. 10% Chance, danach weggeworfen zu werden.",
                
                [Translation.Language.Spanish] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Elige una habilidad aleatoria y la aplica. 10% de probabilidad de descartarse después.",
                [Translation.Language.SpanishAmerica] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Elige una habilidad aleatoria y la aplica. 10% de probabilidad de descartarse después.",
                
                [Translation.Language.Portuguese] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Escolhe uma habilidade aleatória e a aplica. 10% de chance de ser descartado depois.",
                [Translation.Language.PortugueseBrazil] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Escolhe uma habilidade aleatória e a aplica. 10% de chance de ser descartado depois.",
                
                [Translation.Language.ChineseSimplified] =
                    "[K_RANDOM_ACTIVATION] (2.5%):\n" +
                    "随机选择一个能力并应用它。之后有10%的几率被丢弃。",
                [Translation.Language.Japanese] =
                    "[K_RANDOM_ACTIVATION] (2.5%):\n" +
                    "ランダムにアビリティを選んで適用します。その後10%の確率で捨てられます。",
                
                [Translation.Language.Ukraine] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Вибирає випадкову здібність і застосовує її. 10% шанс бути викинутим після цього.",
                [Translation.Language.Russian] =
                    "[K_RANDOM_ACTIVATION] (2,5%):\n" +
                    "Выбирает случайную способность и применяет её. 10% шанс быть выброшенным после этого.",
                
                [Translation.Language.Korean] =
                    "[K_RANDOM_ACTIVATION] (2.5%):\n" +
                    "무작위 능력을 선택하여 적용합니다. 이후 10% 확률로 폐기됩니다.",
            }));
    }

    public override bool RegisterAssets(string name)
    {
        AssetMaster.AddSound(AssetManager.GetAsset<AudioClip>("ModSound_Powerup_WrapBox")); // ✅ (see below) todo: fix fmod issue not loading sound because "file not found"
                                                                                            // ==HOW TO FIX==
                                                                                            // change the import properties in unity to that one that's, like, "compressed in memory"
                                                                                            // or whatever, then rebuild the bundle.
                                                                                            // (this is being recalled from memory from weeks ago -
                                                                                                // if it still gives errors, keep trying the different
                                                                                                // compression options or whatever until it works! good luck <3)
        return base.RegisterAssets("powerup giftbox");
    }

    protected override PowerupScript.PowerupEvent OnEquip { get; } = _ =>
    {
        // SlotMachineScript.instance.OnRoundBeing += PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin;
        // SlotMachineScript.instance.OnScoreEvaluationBegin += Trigger;
        SlotMachineScript.instance.OnScoreEvaluationBegin += Trigger;
    };

    protected override PowerupScript.PowerupEvent OnUnequip { get; } = _ =>
    {
        // SlotMachineScript.instance.OnRoundBeing -= PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin;
        // SlotMachineScript.instance.OnScoreEvaluationBegin -= Trigger;
        SlotMachineScript.instance.OnScoreEvaluationBegin -= Trigger;
    };
    
    private static void Trigger()
    {
        if (R.Rng_Powerup(Utils.Giftbox).Value >= 0.025f * GameplayData.ActivationLuckGet()) return;
        var randIndex = R.Rng_Powerup(Utils.Giftbox).Value * AbilityScript.list_All.Count % AbilityScript.list_All.Count;
        
        var abilityScript = AbilityScript.list_All[(int)randIndex];
        var ability = AbilityScript.AbilityGet(abilityScript.IdentifierGet());
        
        PowerupScript.PlayTriggeredAnimation(Utils.Giftbox);
        
        ability.Pick();

        if (!(R.Rng_Powerup(Utils.Giftbox).Value <= 0.1f)) return;
        
        PowerupScript.ThrowAway(Utils.Giftbox, false);
    }
}