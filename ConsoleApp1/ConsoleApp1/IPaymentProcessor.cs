
public interface IPaymentProcessor
{
    bool ValidateDenomination(int amount);
    List<decimal> GetChangeBreakdown(decimal amount);
}
