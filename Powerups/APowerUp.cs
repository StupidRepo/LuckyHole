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
    public virtual PowerupScript.Category Category { get; } = PowerupScript.Category.normal;
    public virtual PowerupScript.Archetype Archetype { get; } = PowerupScript.Archetype.generic;

    public abstract string NameKey { get; }
    public abstract string DescriptionKey { get; }
    public abstract string UnlockMissionKey { get; }
    
    public virtual bool IsInstantPowerup { get; } = false;

    public abstract int MaxBuyTimes { get; }
    
    public virtual int StartingPrice { get; } = 2;
    public virtual BigInteger UnlockPrice { get; } = -1L;
    
    public virtual float StoreRerollChance { get; } = 0.5f;

    public virtual bool RegisterAssets(string name)
    {
        var prefabToAdd = AssetManager.GetAsset<GameObject>(name);
        
        if (prefabToAdd == null)
        {
            Utils.PLogger.LogError($"Failed to load prefab for powerup {ID} with asset name '{name}'.");
            return false;
        }
        
        AssetMaster.AddPrefab(prefabToAdd);
        PowerupScript.dict_IdentifierToPrefabName.Add(ID, prefabToAdd.name);
        
        Utils.PLogger.LogInfo($"Registered assets for powerup {ID} with prefab '{prefabToAdd.name}'!");
        return true;
    }

    public bool RegisterPowerup()
    {
        PowerupScript.Spawn(ID).Initialize(
            false,
            Category, ID, Archetype,
            IsInstantPowerup,
            MaxBuyTimes, StoreRerollChance, StartingPrice, UnlockPrice,
            NameKey, DescriptionKey, UnlockMissionKey,
            OnEquip, OnUnequip, OnPutInDrawer, OnThrowAway);
        return true;
    }
    
    public abstract PowerupScript.PowerupEvent OnEquip { get; }
    public abstract PowerupScript.PowerupEvent OnUnequip { get; }

    public virtual PowerupScript.PowerupEvent? OnPutInDrawer { get; } = null;
    public virtual PowerupScript.PowerupEvent? OnThrowAway { get; } = null;
}