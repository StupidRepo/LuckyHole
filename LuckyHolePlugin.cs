using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LuckyHole;

[BepInPlugin(PGuid, PName, PVersion)]
public class LuckyHolePlugin : BaseUnityPlugin
{
    internal const string PGuid = "io.github.stupidrepo.LuckyHole";
    internal const string PName = "LuckyHole";
    internal const string PVersion = "1.0.0";
    
    private void Awake()
    {
        Utils.PLogger = Logger;
        
        Harmony.CreateAndPatchAll(typeof(LuckyHolePlugin).Assembly, PGuid);
        Logger.LogInfo($"{PName} ({PVersion}) loaded! Commands will load once the game loads a save.");
        
        AssetManager.LoadAssets("LuckyHole.bundle");
    }
}