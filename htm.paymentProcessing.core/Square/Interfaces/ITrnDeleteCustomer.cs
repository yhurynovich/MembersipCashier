namespace htm.paymentProcessing.core.Square.Interfaces
{
    public interface ITrnDeleteCustomer : ITrnRequest, 
        IHasAccessToken,
        IHasCustomerId
    {
    }
}
