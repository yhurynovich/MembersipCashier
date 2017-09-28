using htm.paymentProcessing.core.Square.DataContracts.Transactions;
using Square.Connect.Api;

namespace htm.paymentProcessing.core.Square.ServiceCalls.Base
{
    internal abstract class CallerBase<ResponseType>
    {
        public abstract ResponseType Execute<T>(T transaction) where T: Interfaces.ITrnRequest;
    }
}
