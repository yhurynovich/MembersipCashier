﻿using htm.paymentProcessing.core.Square.Interfaces;
using System.Runtime.Serialization;
using System;
using Newtonsoft.Json;
using htm.paymentProcessing.core.Interfaces;

namespace htm.paymentProcessing.core.Square.DataContracts.Transactions
{
    [DataContract]
    public class TrnDeleteCustomerCard : TrnRetrieveCustomer, ITrnDeleteCustomerCard
    {
        [DataMember]
        public string CardNonce { get; set; }
    }
}
