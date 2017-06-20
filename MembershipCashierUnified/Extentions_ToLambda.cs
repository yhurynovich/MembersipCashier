using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityUnified.Serialization;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MembershipCashierUnified
{
    public static class Extentions_ToLambda
    {
        public static Expression<Func<IAddress, bool>> ToLambda(this IIAddress filter, byte validityLevel = 0)
        {
            if (filter == null)
                return x => false;

            if (filter.ValidateDataAnnotations().Any(v => !string.IsNullOrEmpty(v.ErrorMessage)))
                return x => false;

            IAddress trimmedAddress = new AddressImplementor();
            filter.CopyTo(trimmedAddress, true);
            if (trimmedAddress.Address1 != null)
                trimmedAddress.Address1 = trimmedAddress.Address1.Trim().ToUpper();
            if (trimmedAddress.City != null)
                trimmedAddress.City = trimmedAddress.City.Trim().ToUpper();
            if (trimmedAddress.Country != null)
                trimmedAddress.Country = trimmedAddress.Country.Trim().ToUpper();
            if (trimmedAddress.PostalCode != null)
                trimmedAddress.PostalCode = trimmedAddress.PostalCode.Replace(" ", "").Trim().ToUpper();
            if (trimmedAddress.Province != null)
                trimmedAddress.Province = trimmedAddress.Province.Trim().ToUpper();
            if (trimmedAddress.ProvinceName != null)
                trimmedAddress.ProvinceName = trimmedAddress.ProvinceName.Trim().ToUpper();

            trimmedAddress.ValidityLevel = validityLevel;

            Expression res;
            Expression<Func<IAddress, bool>> ret = x => true;
            var expParam = ret.Parameters.First();
            res = ret.Body;

            //???: Do we need to serve IsResidential ?

            if (trimmedAddress.AddressId != default(int))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("AddressId"), Expression.Constant(trimmedAddress.AddressId)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.Address1))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Address1"), Expression.Constant(trimmedAddress.Address1)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.City))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("City"), Expression.Constant(trimmedAddress.City)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.Country))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Country"), Expression.Constant(trimmedAddress.Country)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.PostalCode))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("PostalCode"), Expression.Constant(trimmedAddress.PostalCode)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.Province))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Province"), Expression.Constant(trimmedAddress.Province)), res);
            //if (!string.IsNullOrEmpty(trimmedAddress.ProvinceName))
            //    res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("ProvinceName"), Expression.Constant(trimmedAddress.ProvinceName)), res);
            if (trimmedAddress.ValidityLevel != default(byte))
                res = Expression.AndAlso(Expression.GreaterThanOrEqual(expParam.GetFieldAccessExpression("ValidityLevel"), Expression.Constant(trimmedAddress.ValidityLevel)), res);

            return Expression.Lambda<Func<IAddress, bool>>(res, expParam);
        }

        public static Expression<Func<IAddress, bool>> ToLambda(this IAddress filter)
        {
            if (filter == null)
                return x => false;

            if (filter.ValidateDataAnnotations().Any(v => !string.IsNullOrEmpty(v.ErrorMessage)))
                return x => false;

            AddressImplementor trimmedAddress = new AddressImplementor();
            filter.CopyTo(trimmedAddress, true);
            if (trimmedAddress.Address1 != null)
                trimmedAddress.Address1 = trimmedAddress.Address1.Trim().ToUpper();
            if (trimmedAddress.City != null)
                trimmedAddress.City = trimmedAddress.City.Trim().ToUpper();
            if (trimmedAddress.Country != null)
                trimmedAddress.Country = trimmedAddress.Country.Trim().ToUpper();
            if (trimmedAddress.PostalCode != null)
                trimmedAddress.PostalCode = trimmedAddress.PostalCode.Replace(" ", "").Trim().ToUpper();
            if (trimmedAddress.Province != null)
                trimmedAddress.Province = trimmedAddress.Province.Trim().ToUpper();
            if (trimmedAddress.ProvinceName != null)
                trimmedAddress.ProvinceName = trimmedAddress.ProvinceName.Trim().ToUpper();

            Expression res;
            Expression<Func<IAddress, bool>> ret = x => true;
            var expParam = ret.Parameters.First();
            res = ret.Body;

            //???: Do we need to serve IsResidential ?

            if (trimmedAddress.AddressId != default(int))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("AddressId"), Expression.Constant(trimmedAddress.AddressId)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.Address1))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Address1"), Expression.Constant(trimmedAddress.Address1)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.City))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("City"), Expression.Constant(trimmedAddress.City)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.Country))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Country"), Expression.Constant(trimmedAddress.Country)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.PostalCode))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("PostalCode"), Expression.Constant(trimmedAddress.PostalCode)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.Province))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Province"), Expression.Constant(trimmedAddress.Province)), res);
            if (!string.IsNullOrEmpty(trimmedAddress.ProvinceName))
                res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("ProvinceName"), Expression.Constant(trimmedAddress.ProvinceName)), res);
            if (trimmedAddress.ValidityLevel != default(byte))
                res = Expression.AndAlso(Expression.GreaterThanOrEqual(expParam.GetFieldAccessExpression("ValidityLevel"), Expression.Constant(trimmedAddress.ValidityLevel)), res);

            return Expression.Lambda<Func<IAddress, bool>>(res, expParam);
        }

        //public static Expression<Func<IPartyCore, bool>> ToLambda(this IParty filter)
        //{
        //    if (filter == null)
        //        return x => false;

        //    IPartyCore trimmedParty = new PartyImplementor();
        //    filter.CopyTo(trimmedParty, true);

        //    if (trimmedParty.CompanyName != null)
        //        trimmedParty.CompanyName = trimmedParty.CompanyName.Trim().ToUpper();
        //    if (trimmedParty.DeliveryInstruction != null)
        //        trimmedParty.DeliveryInstruction = trimmedParty.DeliveryInstruction.Trim().ToUpper();
        //    if (trimmedParty.FirstName != null)
        //        trimmedParty.FirstName = trimmedParty.FirstName.Trim().ToUpper();
        //    if (trimmedParty.LastName != null)
        //        trimmedParty.LastName = trimmedParty.LastName.Trim().ToUpper();
        //    if (trimmedParty.Language != null)
        //        trimmedParty.Language = trimmedParty.Language.Trim().ToUpper();
        //    //if (trimmedParty.Email != null)
        //    //    trimmedParty.Email = trimmedParty.Email.Trim().ToUpper();
        //    //if (trimmedParty.Phone != null)
        //    //    trimmedParty.Phone = trimmedParty.Phone.Trim().ToUpper();

        //    Expression res;
        //    Expression<Func<IPartyCore, bool>> ret = x => true;
        //    var expParam = ret.Parameters.First();
        //    res = ret.Body;
        //    if (trimmedParty.PartyId != 0)
        //        res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("PartyId"), Expression.Constant(trimmedParty.PartyId)), res);
        //    if (trimmedParty.AddressId.HasValue && trimmedParty.AddressId.Value != default(int))
        //        res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("AddressId"), Expression.Constant(trimmedParty.AddressId, typeof(int?))), res);
        //    if (!string.IsNullOrEmpty(trimmedParty.FirstName))
        //        res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("FirstName"), Expression.Constant(trimmedParty.FirstName)), res);
        //    if (!string.IsNullOrEmpty(trimmedParty.LastName))
        //        res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("LastName"), Expression.Constant(trimmedParty.LastName)), res);
        //    if (!string.IsNullOrEmpty(trimmedParty.CompanyName))
        //        res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("CompanyName"), Expression.Constant(trimmedParty.CompanyName)), res);
        //    if (!string.IsNullOrEmpty(trimmedParty.DeliveryInstruction))
        //        res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("DeliveryInstruction"), Expression.Constant(trimmedParty.DeliveryInstruction)), res);
        //    if (!string.IsNullOrEmpty(trimmedParty.Language))
        //        res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Language"), Expression.Constant(trimmedParty.Language)), res);

        //    return Expression.Lambda<Func<IPartyCore, bool>>(res, expParam);
        //}
    }
}
