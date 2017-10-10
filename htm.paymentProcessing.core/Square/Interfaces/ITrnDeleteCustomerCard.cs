using htm.paymentProcessing.core.Interfaces;

namespace htm.paymentProcessing.core.Square.Interfaces
{
    public interface ITrnDeleteCustomerCard :
        ITrnRetrieveCustomer,
        IHasCardNonce
    {
    }
}
