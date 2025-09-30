using System.Numerics;
using Panik;
using UnityEngine;
using UnityEngine.Events;

namespace LuckyHole.Powerups;

public abstract class APowerUp : AHaveTranslations
{
    internal static readonly string POWERUP_NAME_PREFIX = "POWERUP_NAME_";
    internal static readonly string POWERUP_DESC_PREFIX = "POWERUP_DESC_";
    
    public abstract PowerupScript.Identifier ID { get; }
    protected virtual PowerupScript.Category Category { get; } = PowerupScript.Category.normal;
    protected virtual PowerupScript.Archetype Archetype { get; } = PowerupScript.Archetype.generic;

    protected abstract string NameKey { get; }
    protected abstract string DescriptionKey { get; }
    protected abstract string UnlockMissionKey { get; }

    protected virtual bool IsInstantPowerup { get; } = false;

    protected abstract int MaxBuyTimes { get; }

    protected virtual int StartingPrice { get; } = 2;
    protected virtual BigInteger UnlockPrice { get; } = -1L;

    protected virtual float StoreRerollChance { get; } = 0.5f;
    
    public virtual bool RegisterAssets(string name)
    {
        var prefabToAdd = AssetManager.GetAsset<GameObject>(name);
        
        if (prefabToAdd == null)
        {
            Utils.PLogger.LogError($"Failed to load prefab for powerup {ID} with asset name '{name}'.");
            return false;
        }
        
        AssetMaster.AddPrefab(prefabToAdd);
        
        if(!PowerupScript.dict_IdentifierToPrefabName.ContainsKey(ID))
            PowerupScript.dict_IdentifierToPrefabName.Add(ID, prefabToAdd.name);
        return true;
    }

    public bool RegisterPowerup()
    {
        try
        {
            PowerupScript.Spawn(ID).Initialize(
                false,
                Category, ID, Archetype,
                IsInstantPowerup,
                MaxBuyTimes, StoreRerollChance, StartingPrice, UnlockPrice,
                NameKey, DescriptionKey, UnlockMissionKey,
                OnEquip, OnUnequip, OnPutInDrawer, OnThrowAway);
        } catch (Exception ex)
        {
            Utils.PLogger.LogError($"Error registering powerup {ID}: {ex}");
            return false;
        }

        return true;
    }

    protected abstract PowerupScript.PowerupEvent OnEquip { get; }
    protected abstract PowerupScript.PowerupEvent OnUnequip { get; }

    protected virtual PowerupScript.PowerupEvent? OnPutInDrawer { get; } = null;
    protected virtual PowerupScript.PowerupEvent? OnThrowAway { get; } = null;
}