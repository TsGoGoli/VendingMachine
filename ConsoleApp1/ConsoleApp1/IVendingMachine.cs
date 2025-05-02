
public interface IVendingMachine
{
    void Initialize();
    void DisplayStatus();
    bool Deposit(int amount, PurchaseChannel channel = PurchaseChannel.VendingMachine);
    bool Purchase(string slotCode, PurchaseChannel channel, int quantity = 1);
    decimal Withdraw();
    decimal GetBalance();
    decimal GetProductPrice(string slotCode);
}
