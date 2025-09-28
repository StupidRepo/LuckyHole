namespace LuckyHole.Commands;

public interface ICommandGroup
{
    string Name { get; }
    void RegisterCommands();
}