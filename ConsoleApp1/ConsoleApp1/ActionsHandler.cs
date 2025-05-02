internal static class ActionsHandler
{

    internal static void HandleShoppingSession(IVendingMachine vendingMachine, IPaymentProcessor paymentProcessor, PurchaseChannel channel)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {channel} Mode ===");
            vendingMachine.DisplayStatus();

            Console.WriteLine("\nSelect operation:");
            if (channel == PurchaseChannel.VendingMachine)
            {
                Console.WriteLine("1. Deposit Money");
                Console.WriteLine("2. Purchase Product");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Return to Main Menu");
            }
            else // Online Shop
            {
                Console.WriteLine("1. Purchase Product (20% Discount!)");
                Console.WriteLine("2. Return to Main Menu");
            }

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid selection. Press any key to continue.");
                Console.ReadKey();
                continue;
            }

            if (channel == PurchaseChannel.VendingMachine)
            {
                switch (choice)
                {
                    case 1:
                        HandleDeposit(vendingMachine);
                        break;
                    case 2:
                        HandlePurchase(vendingMachine, channel);
                        break;
                    case 3:
                        HandleWithdraw(vendingMachine, paymentProcessor);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }
            else // Online Shop
            {
                switch (choice)
                {
                    case 1:
                        HandleOnlinePurchase(vendingMachine, channel);
                        break;
                    case 2:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    private static void HandleDeposit(IVendingMachine vendingMachine)
    {
        Console.WriteLine("\nAccepted denominations: 20, 50, 100, 500");
        Console.Write("Enter amount to deposit: ");
        if (int.TryParse(Console.ReadLine(), out int amount))
        {
            if (vendingMachine.Deposit(amount))
                Console.WriteLine("Deposit successful!");
            else
                Console.WriteLine("Invalid denomination!");
        }
        else
        {
            Console.WriteLine("Invalid amount!");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void HandleOnlinePurchase(IVendingMachine vendingMachine, PurchaseChannel channel)
    {
        Console.Write("\nEnter slot code (e.g., A001): ");
        string slotCode = Console.ReadLine();

        Console.Write("Enter quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        decimal price = vendingMachine.GetProductPrice(slotCode) * quantity * 0.8m;
        Console.WriteLine($"\nTotal price (with 20% discount): {price:C}");

        Console.Write("Enter the amount to pay: ");
        if (int.TryParse(Console.ReadLine(), out int amount))
        {
            if (amount < price)
            {
                Console.WriteLine("Insufficient payment amount!");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            if (vendingMachine.Deposit(amount, channel))
            {
                if (vendingMachine.Purchase(slotCode, channel, quantity))
                {
                    Console.WriteLine("Purchase successful!");
                    decimal change = vendingMachine.Withdraw();
                    if (change > 0)
                        Console.WriteLine($"Your change: {change:C}");
                }
                else
                {
                    Console.WriteLine("Purchase failed! Please check product availability and maximum quantity limit.");
                    decimal refund = vendingMachine.Withdraw();
                    Console.WriteLine($"Refunded amount: {refund:C}");
                }
            }
            else
                Console.WriteLine("Payment failed!");
        }
        else
            Console.WriteLine("Invalid amount!");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void HandlePurchase(IVendingMachine vendingMachine, PurchaseChannel channel)
    {
        Console.Write("\nEnter slot code (e.g., A001): ");
        string slotCode = Console.ReadLine();

        if (vendingMachine.Purchase(slotCode, channel))
            Console.WriteLine("Purchase successful!");
        else
            Console.WriteLine("Purchase failed! Please check your balance and slot availability.");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }


    private static void HandleWithdraw(IVendingMachine vendingMachine, IPaymentProcessor paymentProcessor)
    {
        decimal amount = vendingMachine.Withdraw();
        if (amount > 0)
        {
            var changeBreakdown = paymentProcessor.GetChangeBreakdown(amount);
            Console.WriteLine($"\nWithdrawn amount: {amount:C}");
            Console.WriteLine("Change breakdown:");
            foreach (var denomination in changeBreakdown)
            {
                Console.WriteLine($"- {denomination:C}");
            }
        }
        else
        {
            Console.WriteLine("\nNo money to withdraw.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}