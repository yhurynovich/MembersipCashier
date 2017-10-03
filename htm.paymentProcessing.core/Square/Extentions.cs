using htm.paymentProcessing.core.Interfaces;
using System;
using square = Square.Connect.Model;

namespace htm.paymentProcessing.core.Square
{
    public static class Extentions
    {
        public static square.Money ToSquareMoney(this IMoney data)
        {
            if (typeof(square.Money).IsInstanceOfType(data))
                return (square.Money)data;
            else
            {
                square.Money.CurrencyEnum currency = (square.Money.CurrencyEnum)Enum.Parse(typeof(square.Money.CurrencyEnum), data.Currency.ToString());
                return new square.Money(Convert.ToInt64(data.Amount), currency);
            }
        }
    }
}
