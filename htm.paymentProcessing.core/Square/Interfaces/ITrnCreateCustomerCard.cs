using htm.paymentProcessing.core.Interfaces;

namespace htm.paymentProcessing.core.Square.Interfaces
{
    public interface ITrnCreateCustomerCard :
        ITrnDeleteCustomerCard,
        IHasAddress
    {
        string CardholderName { get; set; }
    }
}
