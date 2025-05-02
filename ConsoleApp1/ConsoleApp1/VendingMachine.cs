
public class VendingMachine : IVendingMachine
{
    private readonly Dictionary<string, Product> _inventory;
    private readonly IPaymentProcessor _paymentProcessor;
    private decimal _balance;

    public VendingMachine(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
        _inventory = new Dictionary<string, Product>();
        _balance = 0;
    }

    public void Initialize()
    {
        _inventory.Add("A001", new Product { Name = "Cola", Price = 2.5m, Quantity = 10});
        _inventory.Add("A002", new Product { Name = "Water", Price = 1.0m, Quantity = 15});
        _inventory.Add("B001", new Product { Name = "Chips", Price = 1.8m, Quantity = 20});
        _inventory.Add("B002", new Product { Name = "Chocolate", Price = 3.0m, Quantity = 8});
    }

    public void DisplayStatus()
    {
        Console.WriteLine("\nAvailable Products:");
        Console.WriteLine("Slot Code | Product    | Price  | Quantity");
        Console.WriteLine("---------|------------|--------|----------|");

        foreach (var item in _inventory)
        {
            Console.WriteLine($"{item.Key,-9} | {item.Value.Name,-10} | {item.Value.Price,6:C} | {item.Value.Quantity,8}");
        }

        Console.WriteLine($"\nCurrent Balance: {_balance:C}");
    }

    public bool Deposit(int amount, PurchaseChannel channel = PurchaseChannel.VendingMachine)
    {
        if (channel == PurchaseChannel.VendingMachine)
        {
            if (!_paymentProcessor.ValidateDenomination(amount))
                return false;
        }

        _balance += amount;
        return true;
    }

    public bool Purchase(string slotCode, PurchaseChannel channel, int quantity = 1)
    {
        if (!_inventory.ContainsKey(slotCode))
            return false;

        var product = _inventory[slotCode];

        if (product.Quantity < quantity)
            return false;

        decimal totalPrice = product.Price * quantity;
        if (channel == PurchaseChannel.OnlineShop)
            totalPrice *= 0.8m; // 20% discount for online purchases

        if (_balance < totalPrice)
            return false;

        _balance -= totalPrice;
        product.Quantity -= quantity;
        return true;
    }

    public decimal Withdraw()
    {
        decimal amount = _balance;
        _balance = 0;
        return amount;
    }

    public decimal GetBalance() => _balance;

    public decimal GetProductPrice(string slotCode)
    {
        return _inventory.ContainsKey(slotCode) ? _inventory[slotCode].Price : 0;
    }
}
