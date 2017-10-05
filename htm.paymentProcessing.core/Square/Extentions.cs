using htm.paymentProcessing.core.Interfaces;
using System;
using square = Square.Connect.Model;

namespace htm.paymentProcessing.core.Square
{
    public static class Extentions
    {
        public static square.Money ToNativeSquareMoney(this IMoney data)
        {
            if (typeof(square.Money).IsInstanceOfType(data))
                return (square.Money)data;
            else
            {
                square.Money.CurrencyEnum currency = (square.Money.CurrencyEnum)Enum.Parse(typeof(square.Money.CurrencyEnum), data.Currency.ToString());
                return new square.Money(Convert.ToInt64(data.Amount), currency);
            }
        }

        public static square.Address ToNativeSquareAddress(this IAddress data)
        {
            if (typeof(square.Address).IsInstanceOfType(data))
                return (square.Address)data;
            else
            {
                var ret = new DataContracts.SquareAddress();
                data.CopyTo(ret);
                return ret;
            }
        }
        
    }
}
