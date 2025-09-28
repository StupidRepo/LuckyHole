using UnityEngine;

namespace LuckyHole;

public static class AssetManager
{
    public static Dictionary<string, UnityEngine.Object> LoadedAssets = new();
    
    private static string ConformToOurName(string assetName)
    {
        // get the last part of the path, without extension
        var lastPart = assetName.Split('/').Last();
        var nameWithoutExtension = lastPart.Split('.').First();

        return nameWithoutExtension.ToLowerInvariant();
    }
    
    public static void LoadAssets(string from)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(from);
        if (stream == null)
        {
            Utils.PLogger.LogError("Failed to find embedded asset bundle!");
            return;
        }

        var assetBundle = AssetBundle.LoadFromStream(stream);
        if (assetBundle == null) 
        {
            Utils.PLogger.LogError("Failed to load asset bundle from stream!");
            return;
        }
        
        var assetNames = assetBundle.GetAllAssetNames();
        foreach (var assetName in assetNames)
        {
            var asset = assetBundle.LoadAsset(assetName);
            if (asset != null)
            {
                var conformed = ConformToOurName(assetName);
                if (!LoadedAssets.TryAdd(conformed, asset)) 
                {
                    Utils.PLogger.LogWarning($"Asset already added to list: {conformed}, skipping.");
                    continue;
                }

                Utils.PLogger.LogInfo($"Loaded asset: {conformed}");
            }
            else
            {
                Utils.PLogger.LogWarning($"Failed to load asset: {assetName}");
            }
        }
        
        assetBundle.Unload(false);
        Utils.PLogger.LogInfo("Assets from asset bundle were loaded, and the asset bundle has now been unloaded from memory.");
    }
    
    public static T? GetAsset<T>(string assetName) where T : UnityEngine.Object
    {
        
        if (LoadedAssets.TryGetValue(ConformToOurName(assetName), out var asset))
        {
            return asset as T;
        }
        
        Utils.PLogger.LogError($"Asset not found: {assetName}");
        return null;
    }
}