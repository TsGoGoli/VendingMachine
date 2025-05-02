
class Program
{
    static void Main(string[] args)
    {
        IPaymentProcessor paymentProcessor = new PaymentProcessor();
        IVendingMachine vendingMachine = new VendingMachine(paymentProcessor);
        vendingMachine.Initialize();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to Smart Shop System!");
            Console.WriteLine("1. Vending Machine");
            Console.WriteLine("2. Online Shop");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect shopping mode: ");

            if (!int.TryParse(Console.ReadLine(), out int shoppingMode) || shoppingMode < 1 || shoppingMode > 3)
            {
                Console.WriteLine("Invalid selection. Press any key to try again.");
                Console.ReadKey();
                continue;
            }

            if (shoppingMode == 3)
                break;

            PurchaseChannel channel = shoppingMode == 1 ? PurchaseChannel.VendingMachine : PurchaseChannel.OnlineShop;
            ActionsHandler.HandleShoppingSession(vendingMachine, paymentProcessor, channel);
        }
    }
}
