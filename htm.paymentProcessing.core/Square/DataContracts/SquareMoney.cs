using htm.paymentProcessing.core.Enumerations;
using htm.paymentProcessing.core.Interfaces;
using Newtonsoft.Json;
using System;
using square = Square.Connect.Model;

namespace htm.paymentProcessing.core.Square.DataContracts
{
    public class SquareMoney : square.Money, IMoney
    {
        [JsonIgnore]
        decimal IMoney.Amount
        {
            get
            {
                return Convert.ToDecimal( base.Amount );
            }
            set
            {
                base.Amount = Convert.ToInt64(value);
            }
        }

        [JsonIgnore]
        CurrencyOptions IMoney.Currency
        {
            get
            {
                return (CurrencyOptions)Enum.Parse(typeof(CurrencyOptions), base.Currency.ToString());
            }
            set
            {
                base.Currency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), value.ToString()); 
            }
        }
    }
}
