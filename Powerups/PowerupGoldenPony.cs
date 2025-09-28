using System.Runtime.CompilerServices;

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

    public override PowerupScript.PowerupEvent OnEquip { get; } = PowerupScript.PFunc_OnEquip_OneTrickPony;
    public override PowerupScript.PowerupEvent OnUnequip { get; } = PowerupScript.PFunc_OnUnequip_OneTrickPony;
    
    protected override Dictionary<string, (List<string> languageCodes, List<string> translations)> Translations { get; } = new()
    {
        {
            POWERUP_NAME_PREFIX + "GOLDEN_PONY", (["en"], ["Golden Pony"])
        },
        {
            POWERUP_DESC_PREFIX + "GOLDEN_PONY", (["en"],
            [
                "Has a 50% chance of granting <rainb>1-3 Jackpots</rainb> <sprite name=\"PtJ\"> or triggering a 666, on the next round. Then, discard this charm."
            ])
        },
    };
}