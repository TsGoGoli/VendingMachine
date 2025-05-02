
public class PaymentProcessor : IPaymentProcessor
{
    private readonly int[] _acceptedDenominations = { 500, 100, 50, 20 };

    public bool ValidateDenomination(int amount)
    {
        return _acceptedDenominations.Contains(amount);
    }

    public List<decimal> GetChangeBreakdown(decimal amount)
    {
        var breakdown = new List<decimal>();
        decimal remaining = amount;

        foreach (var denomination in _acceptedDenominations.OrderByDescending(x => x))
        {
            while (remaining >= denomination)
            {
                breakdown.Add(denomination);
                remaining -= denomination;
            }
        }

        if (remaining > 0)
            breakdown.Add(remaining);

        return breakdown;
    }
}
