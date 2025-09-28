using HarmonyLib;

namespace LuckyHole.Patches;

[HarmonyPatch(typeof(GameplayMaster))]
public class GameplayMasterPatches
{
    [HarmonyPatch(nameof(GameplayMaster.Start))]
    [HarmonyPostfix]
    public static void StartPostfix()
    {
        foreach (var commandGroup in typeof(LuckyHolePlugin).Assembly.GetTypes()
                     .Where(t => typeof(Commands.ICommandGroup).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                     .Select(t => Activator.CreateInstance(t) as Commands.ICommandGroup))
        {
            if (commandGroup == null) continue;
            
            Utils.PLogger.LogInfo($"Registering command group: {commandGroup.Name}");
            commandGroup.RegisterCommands();
        }
    }
}