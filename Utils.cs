using System.Collections;
using BepInEx.Logging;
using UnityEngine.Events;

namespace LuckyHole;

internal static class Utils
{
    internal static ManualLogSource PLogger;
    
    public static void MakeCommand(string[] aliases, string description, UnityAction action)
    {
        new ConsolePrompt.Command(aliases, description, action);
        PLogger.LogInfo($"Command '{string.Join(", ", aliases)}' registered.");
    }
}