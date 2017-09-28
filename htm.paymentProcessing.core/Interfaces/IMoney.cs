using htm.paymentProcessing.core.Enumerations;

namespace htm.paymentProcessing.core.Interfaces
{
    public interface IMoney
    {
        decimal Amount { get; set; }
        CurrencyOptions Currency { get; set; }
    }
}
