using htm.paymentProcessing.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace htm.paymentProcessing.core.Square.Interfaces
{
    public interface ITrnCharge : ITrnRequest, IHasAccessToken, IHasCardNonce, IHasMoney, IHasTransactionId, IHasLocationId
    {
    }
}
