namespace LuckyHole;

public abstract class AHaveTranslations
{
    /** Key is the term name (translation key), value is the list of language codes and corresponding translations. For example:
     * <code>"POWERUP_NAME_GOLDEN_PONY" -> (["en", "es"], ["Golden Pony", "Poni Dorado"])</code>
    */
    protected abstract Dictionary<string, (List<string> languageCodes, List<string> translations)> Translations { get; }

    public bool RegisterTranslations()
    {
        var allLanguageCodes = Translations
            .SelectMany(kvp => kvp.Value.languageCodes)
            .Distinct()
            .ToList();

        foreach (var langCode in allLanguageCodes)
        {
            var termDict = new Dictionary<string, string>();

            foreach (var (term, (languageCodes, translations)) in Translations)
            {
                var idx = languageCodes.IndexOf(langCode);
                if (idx >= 0)
                {
                    termDict[term] = translations[idx];
                }
            }

            if (Utils.AddNewTranslationsAndUpdate(termDict, langCode)) continue;
            
            Utils.PLogger.LogError($"Failed to add/update translations for language '{langCode}'.");
            return false;
        }
        return true;
    }
}