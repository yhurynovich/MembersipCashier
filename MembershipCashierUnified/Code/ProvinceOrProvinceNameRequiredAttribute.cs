using MembershipCashierUnified.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MembershipCashierUnified.Code
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProvinceOrProvinceNameRequiredAttribute : ValidationAttribute
    {
        string[] countriesToApplyTo;

        public override bool IsValid(object value)
        {
            if (!typeof(IAddress).IsInstanceOfType(value))
                return true;

            IAddress address = (IAddress)value;
            string country = address.Country;
            if (string.IsNullOrWhiteSpace(country))
                return false;
            country = country.ToUpper();

            if (!countriesToApplyTo.Any(x => x == country))
                return true;

            return !(string.IsNullOrWhiteSpace(address.Province) && string.IsNullOrWhiteSpace(address.ProvinceName));
        }

        public ProvinceOrProvinceNameRequiredAttribute()
        {
            countriesToApplyTo = new string[] { "US", "CA" };
        }

        public ProvinceOrProvinceNameRequiredAttribute(params string[] countriesToApplyTo)
        {
            this.countriesToApplyTo = countriesToApplyTo;
            for (var i = 0; i < this.countriesToApplyTo.Length; i++)
                this.countriesToApplyTo[i] = this.countriesToApplyTo[i].ToUpper();
        }
    }
}
