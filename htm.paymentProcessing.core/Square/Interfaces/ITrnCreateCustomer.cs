using htm.paymentProcessing.core.Interfaces;

namespace htm.paymentProcessing.core.Square.Interfaces
{
    public interface ITrnCreateCustomer : 
        ITrnRequest, 
        IHasAccessToken,
        IHasParty,
        IHasAddress
    {
        string CompanyName { get; set; }
    }
}

