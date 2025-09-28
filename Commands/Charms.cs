namespace LuckyHole.Commands;

public class Charms : ICommandGroup
{
    public string Name => "Lucky Charm Commands";
    public void RegisterCommands()
    {
        // Max Charm Slots
        Utils.MakeCommand(["slotmaxxing", "sm", "max slots"], $"Gives you the maximum amount of Lucky Charms ({ItemOrganizerScript.CharmsSlotN()}).", () =>
        {
            GameplayData.MaxEquippablePowerupsSet(ItemOrganizerScript.CharmsSlotN());
        });
        
        // Generate charm give commands
        var organisedCharmList = GenerateCharmMaps();
        foreach (var kvp in organisedCharmList)
        {
            var commandName = kvp.Key;
            var charm = kvp.Value;
            
            Utils.MakeCommand([$"give {commandName}", $"gc {commandName}"], $"Gives you the '{charm.NameGet(false, false)}' charm.", () =>
            {
                if (PowerupScript.Equip(charm.identifier, false, true))
                {
                    PowerupScript.PlayTriggeredAnimation(charm.identifier);
                }
            });
        }
    }
    
    private static Dictionary<string, PowerupScript> GenerateCharmMaps()
    {
        // get all charms and put them into a dictionary (command name, charm)
        // command name is either: "name" or "name-id" if there are duplicates (there are like 5 "corpse" charms)
        var charmDict = new Dictionary<string, PowerupScript>();
        foreach (var kvp in PowerupScript.dict_IdentifierToInstance)
        {
            var charm = kvp.Value;
            var id = kvp.Key;
            
            var commandName = charm.NameGet(false, false).ToLower().Replace(" ", "_");
            if (charmDict.ContainsKey(commandName)) { commandName = $"{commandName}-{(int)id}"; }
            charmDict[commandName] = charm;
        }
        
        return charmDict;
    }
}