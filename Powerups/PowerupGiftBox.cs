using System.Runtime.CompilerServices;
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

    public override bool RegisterAssets(string name)
    {
        AssetMaster.AddSound(AssetManager.GetAsset<AudioClip>("ModSound_Powerup_WrapBox")); // todo: fix fmod issue not loading sound because "file not found"
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
    
    protected override Dictionary<string, (List<string> languageCodes, List<string> translations)> Translations { get; } = new()
    {
        {
            POWERUP_NAME_PREFIX + "GIFTBOX", (["en"], ["Gift Box"])
        },
        {
            POWERUP_DESC_PREFIX + "GIFTBOX", (["en"],
            [
                "[K_RANDOM_ACTIVATION] (2.5%):\n" +
                "Picks a random ability and applies it. 10% chance of discarding afterwards."
            ])
        },
    };
}