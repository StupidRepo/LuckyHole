using System.Collections;
using BepInEx.Logging;
using I2.Loc;
using LuckyHole.Powerups;
using Panik;
using UnityEngine;
using UnityEngine.Events;

namespace LuckyHole;

internal static class Utils
{
    internal static ManualLogSource PLogger;

    internal static bool AddNewTranslation(string term, string translation, string languageCode = "en")
        {
            var existingResource = Resources.FindObjectsOfTypeAll<LanguageSourceAsset>();
            if (existingResource.Length == 0)
            {
                PLogger.LogError("No LanguageSourceAsset found in resources, cannot add new translation.");
                return false;
            }
            
            var source = existingResource[0];
            if (source == null)
            {
                PLogger.LogError("LanguageSourceAsset in resources is null, cannot add new translation.");
                return false;
            }
    
            var data = source.SourceData;
            var newTerm = data.AddTerm(term);
            if (newTerm == null) {
                PLogger.LogError($"Failed to add new term '{term}' to LanguageSourceAsset.");
                return false;
            }
            
            var languageIndex = data.GetLanguageIndex(LocalizationManager.CurrentLanguage);
            if (languageIndex < 0)
            {
                PLogger.LogError($"Language '{LocalizationManager.CurrentLanguage}' not found in LanguageSourceAsset.");
                return false;
            }
            
            newTerm.SetTranslation(languageIndex, translation);
            PLogger.LogInfo($"Added new translation for term '{term}' in language '{LocalizationManager.CurrentLanguage}'.");
            
            return true;
        }
    internal static bool AddNewTranslationsAndUpdate(Dictionary<string, string> translations, string languageCode = "en")
    {
        var allSuccess = true;
        foreach (var (term, translation) in translations)
        {
            var success = AddNewTranslation(term, translation, languageCode);
            if (!success) allSuccess = false;
        }

        LocalizationManager.UpdateSources();
        return allSuccess;
    }
    
    public static void MakeCommand(string[] aliases, string description, UnityAction action)
    {
        _ = new ConsolePrompt.Command(aliases, description, action);
        PLogger.LogInfo($"Command '{string.Join(", ", aliases)}' registered.");
    }
    
    public static void RegisterPowerups()
    {
        PLogger.LogInfo($"RegisterPowerups called from {new System.Diagnostics.StackTrace()}");
        PLogger.LogInfo("Registering custom powerups...");

        foreach (var powerup in typeof(LuckyHolePlugin).Assembly.GetTypes()
                     .Where(t => typeof(APowerUp).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                     .Select(t => Activator.CreateInstance(t) as APowerUp))
        {
            if (powerup == null) continue;
            try
            {
                PLogger.LogInfo($"Registering powerup {powerup.ID}...");
                if (!powerup.RegisterTranslations())
                {
                    PLogger.LogError($"Failed to register translations for powerup {powerup.ID}.");
                    continue;
                }

                if (!powerup.RegisterAssets(powerup.ID.ToString().ToLower()))
                {
                    PLogger.LogError($"Failed to register assets for powerup {powerup.ID}.");
                    continue;
                }

                if (!powerup.RegisterPowerup())
                {
                    PLogger.LogError($"Failed to register powerup {powerup.ID}.");
                    continue;
                }

                PLogger.LogInfo($"Registered powerup {powerup.ID} successfully.");
            } catch (Exception ex)
            {
                PLogger.LogError($"Error registering powerup {powerup.ID}: {ex}");
            }
        }
        
        PLogger.LogInfo("Finished registering custom powerups.");
    }

    #region Custom Abilities
    private const AbilityScript.Identifier ModAbilityIdentifierOffset = AbilityScript.Identifier.count + 5;
    // ---start
    internal const AbilityScript.Identifier TestAbility = ModAbilityIdentifierOffset + 1;
    // ---end
    #endregion
    #region Custom Powerups
    private const PowerupScript.Identifier ModPowerupIdentifierOffset = PowerupScript.Identifier.count + 5;
    // ---start
    internal const PowerupScript.Identifier GoldenPony = ModPowerupIdentifierOffset + 1;
    internal const PowerupScript.Identifier Giftbox = ModPowerupIdentifierOffset + 2;
    // --- end
    #endregion
}