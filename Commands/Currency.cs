namespace LuckyHole.Commands;

public class Currency : ICommandGroup
{
    public string Name => "Money Commands";
    public void RegisterCommands()
    {
        #region Coins - Change coin values
        // +1K coins
        Utils.MakeCommand(["kaching", "+1kc"], "Grants you 1,000 coins.", () =>
        {
            GameplayData.CoinsAdd(1_000, false);
        });
        // +50K coins
        Utils.MakeCommand(["motherlode", "+50kc"], "Grants you 50,000 coins.", () =>
        {
            GameplayData.CoinsAdd(50_000, false);
        });
        
        // Double coins
        Utils.MakeCommand(["damnrich", "*c"], "Doubles your coins.", () =>
        {
            GameplayData.CoinsAdd(GameplayData.CoinsGet(), false);
        });
        #endregion
        #region Tickets
        // +10 tickets
        Utils.MakeCommand(["ticketblast", "+10t"], "Grants you 10 tickets.",
            () => { GameplayData.CloverTicketsAdd(10, false); });
        // +100 tickets
        Utils.MakeCommand(["ticketstorm", "+100t"], "Grants you 100 tickets.",
            () => { GameplayData.CloverTicketsAdd(100, false); });
        
        // Double tickets
        Utils.MakeCommand(["tickettycoon", "*t"], "Doubles your tickets.",
            () => { GameplayData.CloverTicketsAdd(GameplayData.CloverTicketsGet(), false); });
        #endregion
        #region Interest Rate
        // +1% interest rate
        Utils.MakeCommand(["slightinterest", "+1i"], "Increases your interest rate by 1%.", () =>
        {
            GameplayData.InterestRateAdd(1f);
        });
        // +5% interest rate
        Utils.MakeCommand(["biginterest", "+5i"], "Increases your interest rate by 5%.", () =>
        {
            GameplayData.InterestRateAdd(5f);
        });
        
        // -1% interest rate
        Utils.MakeCommand(["boringasf", "-1i"], "Decreases your interest rate by 1%.", () =>
        {
            GameplayData.InterestRateAdd(-1f);
        });
        // -5% interest rate
        Utils.MakeCommand(["deadbrain", "-5i"], "Decreases your interest rate by 5%.", () =>
        {
            GameplayData.InterestRateAdd(-5f);
        });
        
        // Double interest rate
        Utils.MakeCommand(["x2 interest", "*i"], "Doubles your interest rate.", () =>
        {
            GameplayData.InterestRateAdd(GameplayData.InterestRateGet());
        });
        
        // Max interest rate
        Utils.MakeCommand(["maxinterest", "!i"], "Sets your interest rate to the maximum (100%).", () =>
        {
            GameplayData.InterestRateSet(1000f);
        });
        // Reset interest rate
        Utils.MakeCommand(["resetinterest", ".i"], "Resets your interest rate to the default (7%).", () =>
        {
            GameplayData.InterestRateSet(7f);
        });
        #endregion
    }
}