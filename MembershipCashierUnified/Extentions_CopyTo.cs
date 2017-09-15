using MembershipCashierUnified.Interfaces;
using SecurityUnified.Interfaces;
using System;
using System.Linq;
using SecurityUnified;
using System.Collections.Generic;

namespace MembershipCashierUnified
{
    public static class Extentions_CopyTo
    {
        public static void CopyTo(this IHasAddressId from, IHasAddressId to, bool allowDefaultValues = true)
        {
            if (from == null || to == null)
                return;
            if (allowDefaultValues || from.AddressId != default(int))
                to.AddressId = from.AddressId;
        }

        public static void CopyTo(this IMayHaveAddressId from, IMayHaveAddressId to, bool allowDefaultValues = true)
        {
            if (from == null || to == null)
                return;
            if (allowDefaultValues || (from.AddressId.HasValue && from.AddressId.Value != default(int)))
                to.AddressId = from.AddressId;
        }

        //public static void CopyTo(this IMayHaveAddressId from, IMayHaveAddressId to, bool allowDefaultValues = true)
        //{
        //    if (from == null || to == null)
        //        return;

        //    if (allowDefaultValues || (from.AddressId.HasValue && from.AddressId != default(int)))
        //        to.AddressId = from.AddressId;
        //}

        public static void CopyTo(this IIAddress from, IIAddress to, bool allowDefaultValues = true)
        {
            if (from == null || to == null)
                return;

            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.Address1))
                to.Address1 = from.Address1;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.City))
                to.City = from.City;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.Country))
                to.Country = from.Country;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.PostalCode))
                to.PostalCode = from.PostalCode;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.Province))
                to.Province = from.Province;
        }

        public static void CopyTo(this IAddress from, IAddress to, bool allowDefaultValues = true)
        {
            if (from == null || to == null)
                return;

            (from as IHasAddressId).CopyTo(to, allowDefaultValues);
            (from as IIAddress).CopyTo(to, allowDefaultValues);
            if (allowDefaultValues || from.IsResidential.HasValue)
                to.IsResidential = from.IsResidential;
            if (allowDefaultValues || from.ValidityLevel != default(byte))
                to.ValidityLevel = from.ValidityLevel;
            if (allowDefaultValues || !string.IsNullOrWhiteSpace(from.ProvinceName))
                to.ProvinceName = from.ProvinceName;
            if (allowDefaultValues || (from.ValidatedTime.HasValue && from.ValidatedTime.Value != default(DateTime)))
                to.ValidatedTime = from.ValidatedTime;
        }

        public static void CopyTo(this IHasCreditTransactionId from, IHasCreditTransactionId to, bool allowDefaultValues = true)
        {
            to.CreditTransactionId = from.CreditTransactionId;
        }

        public static void CopyTo(this ICreditTransaction from, ICreditTransaction to, bool allowDefaultValues = true)
        {
            (from as IHasCreditTransactionId).CopyTo(to, allowDefaultValues);
            (from as IHasProductId).CopyTo(to, allowDefaultValues);
            (from as IHasLocationId).CopyTo(to, allowDefaultValues);
            (from as IHasUserId).CopyTo(to, allowDefaultValues);
            (from as IHasDescription).CopyTo(to, allowDefaultValues);
            (from as IHasBallanceUnits).CopyTo(to, allowDefaultValues);
            
            if (allowDefaultValues || (from.TransactionTime != null && from.TransactionTime != DateTime.MinValue))
                to.TransactionTime = from.TransactionTime;
        }

        //public static void CopyTo(this IUserProfile from, IUserProfile to, bool allowDefaultValues = true)
        //{
        //    to.UserId = from.UserId;
        //    if (!string.IsNullOrEmpty(from.UserName))
        //        to.UserName = from.UserName;
        //    if (allowDefaultValues || !string.IsNullOrEmpty(from.EmailId))
        //        to.EmailId = from.EmailId;
        //    if (allowDefaultValues || !string.IsNullOrEmpty(from.FirstName))
        //        to.FirstName = from.FirstName;
        //    if (allowDefaultValues || !string.IsNullOrEmpty(from.LastName))
        //        to.LastName = from.LastName;
        //    if (allowDefaultValues || from.UserStatusId!=byte.MinValue)
        //        to.UserStatusId = from.UserStatusId;
        //}

        public static void CopyTo(this IHasLocationId from, IHasLocationId to, bool allowDefaultValues = true)
        {
            to.LocationId = from.LocationId;
        }

        public static void CopyTo(this ILocation from, ILocation to, bool allowDefaultValues = true)
        {
            (from as IHasLocationId).CopyTo(to);
            (from as IMayHaveAddressId).CopyTo(to);
            (from as IHasDescription).CopyTo(to);
            if (allowDefaultValues || !string.IsNullOrEmpty(from.LocationCode))
                to.LocationCode = from.LocationCode;
            if (allowDefaultValues || !string.IsNullOrEmpty(from.TimeZoneCode))
                to.TimeZoneCode = from.TimeZoneCode;
            if (allowDefaultValues || from.IsCredeitReversed!=default(bool))
                to.IsCredeitReversed = from.IsCredeitReversed;
        }

        public static void CopyTo(this IHasOwnerId from, IHasOwnerId to, bool allowDefaultValues = true)
        {
            to.OwnerId = from.OwnerId;
        }

        public static void CopyTo(this IOwner from, IOwner to, bool allowDefaultValues = true)
        {
            (from as IHasOwnerId).CopyTo(to, allowDefaultValues);
            (from as IHasUserId).CopyTo(to, allowDefaultValues);
            //to.OwnerUserId = from.OwnerUserId; //same as UserId
        }

        public static void CopyTo(this IProfileCredit from, IProfileCredit to, bool allowDefaultValues = true)
        {
            (from as IHasProductId).CopyTo(to);
            (from as IHasUserId).CopyTo(to);
            (from as IHasLocationId).CopyTo(to);
            (from as IHasBallanceUnits).CopyTo(to, allowDefaultValues);
            (from as IHasBallance).CopyTo(to, allowDefaultValues);
            if (allowDefaultValues || from.Ballance != 0)
                to.Ballance = from.Ballance;
        }

        public static void CopyTo(this IHasBallanceUnits from, IHasBallanceUnits to, bool allowDefaultValues = true)
        {
            if (allowDefaultValues || from.BallanceUnits!=default(int))
                to.BallanceUnits = from.BallanceUnits;
        }

        public static void CopyTo(this IHasBallance from, IHasBallance to, bool allowDefaultValues = true)
        {
            if (allowDefaultValues || from.Ballance != default(int))
                to.Ballance = from.Ballance;
        }

        public static void CopyTo(this IOwnerVsLocation from, IOwnerVsLocation to, bool allowDefaultValues = true)
        {
            (from as IHasOwnerId).CopyTo(to, allowDefaultValues);
            (from as IHasLocationId).CopyTo(to);
            if (allowDefaultValues || from.IsCurrent != default(bool))
                to.IsCurrent = from.IsCurrent;
        }

        public static void CopyTo(this IUserProfile2 from, IUserProfile2 to, bool allowDefaultValues = true)
        {
            (from as IUserProfile).CopyTo(to, allowDefaultValues);
            if (allowDefaultValues || from.Photo != null)
                to.Photo = from.Photo;
        }

        public static void CopyTo(this IHasProductId from, IHasProductId to, bool allowDefaultValues = true)
        {
            to.ProductId = from.ProductId;
        }

        public static void CopyTo(this IHasDescription from, IHasDescription to, bool allowDefaultValues = true)
        {
            to.Description = from.Description;
        }

        public static void CopyTo(this IProductCore from, IProductCore to, bool allowDefaultValues = true)
        {
            (from as IHasDescription).CopyTo(to, allowDefaultValues);
            (from as IHasProductId).CopyTo(to);
        }

        public static void CopyTo(this IHasProductPriceHistories from, IHasProductPriceHistories to, bool allowDefaultValues = true)
        {
            if (allowDefaultValues || from.ProductPriceHistories != null)
                to.ProductPriceHistories = from.ProductPriceHistories.Clone<MembershipCashierUnified.Contracts.ProductPriceHistoryImplementor, IProductPriceHistory>();
        }

        private static IEnumerable<R> Clone<R, T>(this IEnumerable<T> data)
            where T : ICanCopyTo<T>
            where R : T, new()
        {
            if (data == null)
                return null;

            var cnt = data.Count();
            var ret = new List<R>(cnt);

            foreach (var item in data)
            {
                var x = new R();
                ((ICanCopyTo<T>)item).CopyTo(x);
                ret.Add(x);
            }

            return ret;
        }

        public static void CopyTo(this IProduct from, IProduct to, bool allowDefaultValues = true)
        {
            (from as IProductCore).CopyTo(to, allowDefaultValues);
            (from as IHasProductPriceHistories).CopyTo(to, allowDefaultValues);
        }

        public static void CopyTo(this IMayHavePrice from, IMayHavePrice to, bool allowDefaultValues = true)
        {
            if (allowDefaultValues || (from.Price.HasValue && from.Price.Value != default(decimal)))
                to.Price = from.Price;
        }

        public static void CopyTo(this IHasPrice from, IHasPrice to, bool allowDefaultValues = true)
        {
            if (allowDefaultValues || from.Price != default(decimal))
                to.Price = from.Price;
        }

        public static void CopyTo(this IProductPriceHistory from, IProductPriceHistory to, bool allowDefaultValues = true)
        {
            (from as IHasProductId).CopyTo(to, allowDefaultValues);
            (from as IHasPrice).CopyTo(to, allowDefaultValues);

            if (allowDefaultValues || from.ChangeDate != default(DateTime))
                to.ChangeDate = from.ChangeDate;
        }

        public static void CopyTo(this IUserProfileWithLDAP from, IUserProfileWithLDAP to, bool allowDefaultValues = true)
        {
            if (from == null || to == null)
                return;

            (from as IUserProfile).CopyTo(to, allowDefaultValues);

            if (allowDefaultValues || !string.IsNullOrEmpty(from.LdapAccount))
                to.LdapAccount = from.LdapAccount;
        }

        //public static void CopyTo(this IHasPaymentId from, IPayment to, bool allowDefaultValues = true)
        //{
        //    if (allowDefaultValues || from.PaymentId != default(long))
        //        to.PaymentId = from.PaymentId;
        //}

        public static void CopyTo(this IPayment from, IPayment to, bool allowDefaultValues = true)
        {
            if (from == null || to == null)
                return;

            (from as IHasCreditTransactionId).CopyTo(to, allowDefaultValues);
            if (allowDefaultValues || from.Sequence != default(short))
                to.Sequence = from.Sequence;
            if (allowDefaultValues || from.PaymentMethod != default(char))
                to.PaymentMethod = from.PaymentMethod;
            if (allowDefaultValues || from.Amount.HasValue && from.Amount.Value != default(decimal))
                to.Amount = from.Amount;
            if (allowDefaultValues || !string.IsNullOrEmpty(from.Currency))
                to.Currency = from.Currency;
        }
    }
}
