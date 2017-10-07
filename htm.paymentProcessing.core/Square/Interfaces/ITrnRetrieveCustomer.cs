namespace htm.paymentProcessing.core.Square.Interfaces
{
    public interface ITrnRetrieveCustomer : ITrnRequest,
        IHasAccessToken,
        IHasCustomerId
    {
    }
}
